using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OpenGameScene()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenDescriptionScene()
    {
        SceneManager.LoadScene(2);

    }

    public void Exit()
    {
        Application.Quit();
    }
}
