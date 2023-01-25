using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UISceneTransitionController : Singleton<UISceneTransitionController>
{
    public UISceneTransition Transition { get => transition; }

    [Header("View References")]
    [SerializeField]
    private UISceneTransition transition;

    [Header("Events")]
    [SerializeField]
    private UnityEvent OnOpenTransitionStarted;
    [SerializeField]
    private UnityEvent OnOpenTransitionEnded;
    [SerializeField]
    private UnityEvent OnCloseTransitionStarted;
    [SerializeField]
    private UnityEvent OnCloseTransitionEnded;

    public void OpenTransition()
    {
        OnOpenTransitionStarted?.Invoke();
        transition.Open();
    }
    public void CloseTransition()
    {
        OnCloseTransitionStarted?.Invoke();
        transition.Close();
    }

    private void OnEnable()
    {
       transition.OnOpenTransitionEnded += CallOpenEvents;
       transition.OnCloseTransitionEnded += CallCloseEvents;
    }

    private void OnDisable()
    {
        if (!this.gameObject.scene.isLoaded)
            return;
        transition.OnOpenTransitionEnded -= CallOpenEvents;
        transition.OnCloseTransitionEnded -= CallCloseEvents;
    }

    private void CallOpenEvents() => OnOpenTransitionEnded?.Invoke();
    private void CallCloseEvents() => OnCloseTransitionEnded?.Invoke();
}
