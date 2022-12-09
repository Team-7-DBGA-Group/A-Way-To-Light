using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationManager : Singleton<NavigationManager>
{
    public bool IsLoadingScene { get; private set; }

    private string _sceneNameToBeLoaded = "";

    public void QuitGame() => Application.Quit();

    public void ChangeScene(string sceneName)
    {
        if (IsLoadingScene)
            return;

        _sceneNameToBeLoaded = sceneName;
        UISceneTransitionController.Instance.OpenTransition();
    }

    protected override void Awake()
    {
        base.Awake();
        IsLoadingScene = false;
        _sceneNameToBeLoaded = "";
    }

    private void OnEnable()
    {
        UISceneTransitionController.Instance.Transition.OnOpenTransitionEnded += LoadScene;
    }

    private void OnDisable()
    {
        if (!this.gameObject.scene.isLoaded) 
            return;
        UISceneTransitionController.Instance.Transition.OnOpenTransitionEnded -= LoadScene;
    }

    private void LoadScene() => StartCoroutine(COLoadSceneAsync(_sceneNameToBeLoaded));

    private IEnumerator COLoadSceneAsync(string sceneName)
    {
        IsLoadingScene = true;
        yield return new WaitForSeconds(1.0f);
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        _sceneNameToBeLoaded = "";
        yield return async;
        IsLoadingScene = false;
    }
}
