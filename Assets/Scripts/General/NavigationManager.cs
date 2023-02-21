using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationManager : Singleton<NavigationManager>
{
    public bool IsLoadingScene { get; private set; }

    private string _sceneNameToBeLoaded = "";
    private bool _shouldLoadScene;

    public void LoadGameScene()
    {
        string sceneName = PlayerPrefs.GetString("GameScene");
        if (string.IsNullOrEmpty(sceneName))
            return;

        ChangeScene(sceneName,false, true);
    }

    public void BackToTitle()
    {
        ChangeScene("MainMenu", false, false);
    }

    public void QuitGame() => Application.Quit();
    
    public void OpenSurvey() => Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSdZsJ465OSGwVLCUJm4jvJIVWhfzw4nso7EGf0JpCkrwWdTzQ/viewform");

    public void ChangeScene(string sceneName)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (IsLoadingScene)
            return;
       
        _shouldLoadScene = true;

        if (sceneName.Contains("Level"))
        {
            PlayerPrefs.SetString("GameScene", sceneName);
        }

        _sceneNameToBeLoaded = sceneName;
        SpawnManager.Instance.ResetManager();
        DataPersistenceManager.Instance.SaveGame();
        UISceneTransitionController.Instance.OpenTransition();
    }

    public void ChangeScene(string sceneName, bool resetManager, bool cursorLocked)
    {
        if (IsLoadingScene)
            return;

        _shouldLoadScene = true;

        if(cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        _sceneNameToBeLoaded = sceneName;
        if (resetManager)
        {
            SpawnManager.Instance.ResetManager();
            DataPersistenceManager.Instance.SaveGame();
        }
        UISceneTransitionController.Instance.OpenTransition();
    }

    protected override void Awake()
    {
        base.Awake();
        IsLoadingScene = false;
        _sceneNameToBeLoaded = "";
        _shouldLoadScene = false;
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

    private void LoadScene() 
    {
        if (!_shouldLoadScene)
            return;
        StartCoroutine(COLoadSceneAsync(_sceneNameToBeLoaded));
    } 

    private IEnumerator COLoadSceneAsync(string sceneName)
    {
        IsLoadingScene = true;
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        _sceneNameToBeLoaded = "";
        yield return async;
        IsLoadingScene = false;
    }
}
