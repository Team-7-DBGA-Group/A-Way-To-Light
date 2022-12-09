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
    private UnityEvent OnOpenTransitionEnded;
    [SerializeField]
    private UnityEvent OnCloseTransitionEnded;

    public void OpenTransition() => transition.Open();

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
