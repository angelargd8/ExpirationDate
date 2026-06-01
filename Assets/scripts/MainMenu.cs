using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void LoadLevel()
    {
        GameManager.instance.LoadScene("Restaurant");
    }

    public void QuitGame()
    {
        GameManager.instance.QuitGame();
    }
}
