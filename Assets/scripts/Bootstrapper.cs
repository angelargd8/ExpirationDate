using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    public static Bootstrapper instance;

    private void Awake()
    {
        if (instance == null)
        {

            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(instance);
            return;
        }


    }
}
