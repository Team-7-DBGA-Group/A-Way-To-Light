using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : Singleton<CutsceneManager>
{
    [Header("References")]
    [SerializeField]
    private List<GameObject> Cutscenes;

    public void PlayCutscene(int index)
    {
        if (Cutscenes.Count < index)
            return;
        Cutscenes[index].SetActive(false);
        Cutscenes[index].SetActive(true);
    }

    public void PlayCutscene(string cutsceneName)
    {
        foreach(GameObject cutscene in Cutscenes)
        {
            if (cutscene.gameObject.name.Equals(cutsceneName))
            {
                cutscene.SetActive(false);
                cutscene.SetActive(true);
            }
        }
    }

    private void OnEnable()
    {
        foreach(GameObject cutscene in Cutscenes)
        {
            cutscene.SetActive(false);
        }
    }
}
