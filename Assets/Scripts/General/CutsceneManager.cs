using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : Singleton<CutsceneManager>
{
    public bool IsPlayingCutscene { get; private set; }

    [Header("References")]
    [SerializeField]
    private PlayableDirector director;

    [SerializeField]
    private List<PlayableAsset> cutscenes;

    public void PlayCutscene(int index)
    {
        if (index >= cutscenes.Count)
            return;

        director.Play(cutscenes[index]);
    }

    public void PlayCutscene(string cutsceneName)
    {
        foreach(PlayableAsset cutscene in cutscenes)
        {
            if (cutscene.name.Equals(cutsceneName))
            {
                director.Play(cutscene);
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        IsPlayingCutscene = false;
    }

    private void OnEnable()
    {
        director.played += OnPlayableDirectorPlayed;
        director.stopped += OnPlayableDirectorStopped;
    }

    private void OnDisable()
    {
        director.played -= OnPlayableDirectorPlayed;
        director.stopped -= OnPlayableDirectorStopped;
    }

    private void OnPlayableDirectorPlayed(PlayableDirector director)
    {
        if (this.director != director)
            return;

        IsPlayingCutscene = true;
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if (this.director != director)
            return;

        IsPlayingCutscene = false;
    }
}
