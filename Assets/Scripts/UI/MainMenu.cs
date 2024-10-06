using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject ConfirmExitTip;

    public void OpenGameScene()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenDescriptionScene()
    {
        SceneManager.LoadScene(2);
    }

    public void OpenConfirmExitTip()
    {
        ConfirmExitTip.SetActive(true);
    }
    public void CloseConfirmExitTip()
    {
        ConfirmExitTip.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
