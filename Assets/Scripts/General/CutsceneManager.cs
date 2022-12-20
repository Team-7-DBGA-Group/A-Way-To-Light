using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutsceneManager : Singleton<CutsceneManager>
{
    public bool IsPlayingCutscene { get; private set; }

    [Header("References")]
    [SerializeField]
    private PlayableDirector director;

    [SerializeField]
    private List<PlayableAsset> cutscenes;

    [Header("Settings")]
    [SerializeField]
    private string startingCutsceneName = "";

    private bool _isStartingCutsecenePlayed = false;

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
        SpawnManager.OnPlayerSpawn += OnPlayerSpawnBindings;

    }

    private void OnDisable()
    {
        director.played -= OnPlayableDirectorPlayed;
        director.stopped -= OnPlayableDirectorStopped;
        SpawnManager.OnPlayerSpawn -= OnPlayerSpawnBindings;
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

    private void OnPlayerSpawnBindings(GameObject playerObj)
    {
        TimelineAsset timelineAsset = director.playableAsset as TimelineAsset;
        TrackAsset track1 = (TrackAsset)timelineAsset.GetOutputTrack(2);
        TrackAsset track2 = (TrackAsset)timelineAsset.GetOutputTrack(3);

        director.SetGenericBinding(track1, playerObj.GetComponent<Animator>());
        director.SetGenericBinding(track2, playerObj.GetComponent<Animator>());

        if (!_isStartingCutsecenePlayed)
        {
            PlayCutscene(startingCutsceneName);
            _isStartingCutsecenePlayed = true;
        }
    }
}
