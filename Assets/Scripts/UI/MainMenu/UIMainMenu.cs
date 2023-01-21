using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIMainMenu : MonoBehaviour
{
    public event Action OnEnterPressed;
    public bool IsOpen { get; private set; }

    [Header("UI References")]
    [SerializeField]
    private GameObject mainMenuTitle;
    [SerializeField]
    private GameObject pressText;
    [SerializeField]
    private GameObject fadePanel;
    [SerializeField]
    private GameObject btnsContainer;

    private bool _pressEnterOnce = false;

    public void OpenUI()
    {
        if (IsOpen)
            return;

        IsOpen = true;

        mainMenuTitle.GetComponent<Animator>().SetTrigger("MoveUp");
        pressText.gameObject.SetActive(false);
        fadePanel.GetComponent<Animator>().SetTrigger("FadeOut");
        StartCoroutine(COWaitForAction(1.1f, () => { btnsContainer.SetActive(true); }));
    }

    // Close Method maybe?
    private void Awake()
    {
        btnsContainer.SetActive(false);
    }

    private void Update()
    {
        if(InputManager.Instance.GetFirePressed() && !_pressEnterOnce)
        {
            _pressEnterOnce = true;
            OnEnterPressed?.Invoke();
        }
    }

    private IEnumerator COWaitForAction(float sec, Action Callback)
    {
        yield return new WaitForSeconds(sec);
        Callback?.Invoke();
    }
}
