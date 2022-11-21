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

    private Story _currentStory;
    private bool _canShowNextLine;
    private TextMeshProUGUI[] _choicesText;

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
    }

    private void Update()
    {
        if (!IsDialoguePlaying) return;

        // Continue to next line
        if (Input.GetKeyDown(KeyCode.K))
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

        OnDialogueExit?.Invoke();
    }

    private void ContinueStory()
    {
        if (_currentStory.canContinue)
        {
            //dialogueText.text = _currentStory.Continue(); // Next line
            StartCoroutine(ShowAnimationText(_currentStory.Continue(), 0.06f, () => {
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
}