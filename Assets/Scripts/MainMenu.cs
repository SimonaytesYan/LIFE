using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    const int mainMenuScene = 0;
    const int singlePlayerScene = 1;
    const int multiPlayerScene = 2;
    public void loadMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
    public void loadSinglePlayer()
    {
        SceneManager.LoadScene(singlePlayerScene);
    }
    public void loadMultiPlayer()
    {
        SceneManager.LoadScene(multiPlayerScene);
    }
}
