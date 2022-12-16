using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueManager : Singleton<DialogueManager>
{
    public static event Action OnDialogueEnter;
    public static event Action OnDialogueExit;
    public static event Action<int> OnChoiceChosen;
    public bool IsDialoguePlaying { get; private set; }

    [Header("Dialogue UI")]
    [SerializeField] 
    private GameObject dialoguePanel;
    [SerializeField]
    private TextMeshProUGUI dialogueText;
    [SerializeField]
    private GameObject continueIcon;

    [Header("Choices UI")]
    [SerializeField]
    private GameObject[] choices;

    [Header("Audio")]
    [SerializeField]
    private DialogueAudioInfoSO defaultAudioInfo;
    [SerializeField]
    private DialogueAudioInfoSO[] audioInfos;
    [SerializeField]
    private bool makePredictable;

    private DialogueAudioInfoSO _currentAudioInfo;
    private Dictionary<string,DialogueAudioInfoSO> _audioInfoDictionary;
    private AudioSource _audioSource;

    private Story _currentStory;
    private bool _canShowNextLine;
    private TextMeshProUGUI[] _choicesText;

    // TAGS Section
    private const string AUDIO_TAG = "audio";

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _currentStory = new Story(inkJSON.text);
        IsDialoguePlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();

        OnDialogueEnter?.Invoke();
    }

    public void MakeChoice(int choiceIndex)
    {
        _currentStory.ChooseChoiceIndex(choiceIndex);
        DisplayChoices(); // Fast Reset
        OnChoiceChosen?.Invoke(choiceIndex);
        ContinueStory();
    }

    protected override void Awake()
    {
        base.Awake();

        _audioSource = this.gameObject.AddComponent<AudioSource>();
        _currentAudioInfo = defaultAudioInfo;
    }

    private void Start()
    {
        IsDialoguePlaying = false;
        _canShowNextLine = true;
        dialoguePanel.SetActive(false);

        // get all choices text
        _choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            _choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }

        InitializeAudioInfoDictionary();
    }

    private void Update()
    {
        if (!IsDialoguePlaying) return;

        // Continue to next line
        if (InputManager.Instance.GetContinueDialoguePressed())
        {
            if (_canShowNextLine)
                ContinueStory();
        }
    }

    public void ExitDialogueMode()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        IsDialoguePlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        foreach (GameObject choice in choices)
        {
            choice.SetActive(false);
        }

        SetCurrentAudioInfo(defaultAudioInfo.ID);

        OnDialogueExit?.Invoke();
    }

    private void ContinueStory()
    {
        if (_currentStory.canContinue)
        {
            //dialogueText.text = _currentStory.Continue(); // Next line
            
            string nextLine = _currentStory.Continue();
            HandleTags(_currentStory.currentTags); 

            StartCoroutine(ShowAnimationText(nextLine, 0.06f, () => {

                if (_currentStory.currentChoices.Count > 0)
                    continueIcon.SetActive(false);
                else
                    continueIcon.SetActive(true);
                DisplayChoices(); // if any

            }));
        }
        else
        {
            // Empty JSON or Finish
            ExitDialogueMode();
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = _currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choice given: " + currentChoices.Count);
        }
        // Show 
        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            _choicesText[index].text = choice.text;
            index++;
        }
        // Hide remaining
        for (int i = index; i < choices.Length; ++i)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator ShowAnimationText(string text, float delay, Action Callback)
    {
        continueIcon.SetActive(false);
        _canShowNextLine = false;
        string currentText = "";
        for (int i = 0; i < text.Length; i++)
        {
            PlayDialogueSound(currentText.Length, text[i]);
            currentText = text.Substring(0, i);
            dialogueText.text = currentText;
            yield return new WaitForSeconds(delay);
        }
        _canShowNextLine = true;
        Callback?.Invoke();
        
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null); // prima pulisci
        yield return new WaitForEndOfFrame(); // aspetta la fine del frame
        EventSystem.current.SetSelectedGameObject(choices[0]); // setta
    }

    private void PlayDialogueSound(int currentDisplayedCharacterCount, char currentCharacter)
    {
        // Set variables for the below based on our config
        AudioClip[] dialogueTypingSoundClips = _currentAudioInfo.DialogueTypingSoundClips;
        int frequencyLevel = _currentAudioInfo.FrequencyLevel;
        float minPitch = _currentAudioInfo.MinPitch;
        float maxPitch = _currentAudioInfo.MaxPitch;
        bool stopAudioSource = _currentAudioInfo.StopAudioSource;

        // Play sound based on config
        if (currentDisplayedCharacterCount % frequencyLevel == 0)
        {
            if (stopAudioSource)
            {
                _audioSource.Stop();
            }
            
            AudioClip soundClip = null;

            if (makePredictable)
            {
                // Make predictable through hashing
                int hashCode = currentCharacter.GetHashCode();
                // Get sound clip
                int predictIndex = hashCode % dialogueTypingSoundClips.Length;
                soundClip = dialogueTypingSoundClips[predictIndex];
                // Pitch
                int minPitchInt = (int)(minPitch * 100);
                int maxPitchInt = (int)(maxPitch * 100);
                int pitchRangeInt = maxPitchInt - minPitchInt;
                // Cannot divide by 0, so if there is no range then skip the selection
                if(pitchRangeInt != 0)
                {
                    int preditPitchInt = (hashCode % pitchRangeInt) + minPitchInt;
                    float predictPitch = preditPitchInt / 100f;
                    _audioSource.pitch = predictPitch;
                }
                else
                {
                    _audioSource.pitch = minPitch;
                }
            }
            else
            {
                // Get sound clip
                int randomIndex = UnityEngine.Random.Range(0, dialogueTypingSoundClips.Length);
                soundClip = dialogueTypingSoundClips[randomIndex];
                // Pitch
                _audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
            }
          
            // Play sound
            _audioSource.PlayOneShot(soundClip);
        }
    }

    private void InitializeAudioInfoDictionary()
    {
        _audioInfoDictionary = new Dictionary<string, DialogueAudioInfoSO>();
        // Default
        _audioInfoDictionary.Add(defaultAudioInfo.ID, defaultAudioInfo);
        // Others
        foreach(DialogueAudioInfoSO audioInfo in audioInfos)
        {
            _audioInfoDictionary.Add(audioInfo.ID, audioInfo);
        }
    }

    private void SetCurrentAudioInfo(string ID)
    {
        DialogueAudioInfoSO audioInfo = null;
        _audioInfoDictionary.TryGetValue(ID, out audioInfo);

        if (audioInfo == null)
            return;

        _currentAudioInfo = audioInfo;
    }

    private void HandleTags(List<string> currentTags)
    {
        // loop through each tag and handle it accordingly
        foreach (string tag in currentTags)
        {
            // parse the tag
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            // handle the tag
            switch (tagKey)
            {
                // Add Other tags to handle here
                case AUDIO_TAG:
                    SetCurrentAudioInfo(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }
}