using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Escenas")]
    [SerializeField] private string loadingSceneName = "LoadingScene";


    [Header("Loading")]
    [SerializeField] private float loadingMinTime = 3.0f;


    private bool isLoading = false;

    


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);

        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }


    }


    public void LoadScene(string sceneName)
    {
        if (isLoading) return;

        StartCoroutine(LoadSceneAsync(sceneName));
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;

        Debug.Log("Quit Game");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        isLoading = true;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayLoadingMusic();
        }

        if (!SceneManager.GetSceneByName(loadingSceneName).isLoaded)
        {
            SceneManager.LoadScene(loadingSceneName, LoadSceneMode.Additive);

        }


        yield return null;

        var operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(loadingMinTime);


        operation.allowSceneActivation = true;


        while (!operation.isDone)
        {
            yield return null;
        }

        if (SceneManager.GetSceneByName(loadingSceneName).isLoaded)
        {
            yield return SceneManager.UnloadSceneAsync(loadingSceneName);
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusicForScene(sceneName);
        }



        isLoading = false;
    }

    

}
