using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UISceneTransitionController : MonoBehaviour
{
    [Header("View References")]
    [SerializeField]
    private UISceneTransition transition;

    [Header("Events")]
    [SerializeField]
    private UnityEvent OnOpenTransitionEnded;
    [SerializeField]
    private UnityEvent OnCloseTransitionEnded;

    private void OnEnable()
    {
       transition.OnOpenTransitionEnded += CallOpenEvents;
       transition.OnCloseTransitionEnded += CallCloseEvents;
       
    }

    private void OnDisable()
    {
        transition.OnOpenTransitionEnded -= CallOpenEvents;
        transition.OnCloseTransitionEnded -= CallCloseEvents;
    }

    private void CallOpenEvents() => OnOpenTransitionEnded?.Invoke();
    private void CallCloseEvents() => OnCloseTransitionEnded?.Invoke();
}
