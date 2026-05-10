using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapperLoader : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]

    static void Init()
    {
        if (Object.FindFirstObjectByType<Bootstrapper>() != null)
            return;

        SceneManager.LoadScene("Bootstrap", LoadSceneMode.Additive);
    }
}
