using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHuds : MonoBehaviour
{
    [SerializeField] private GameObject ConfirmExitTip;
    [SerializeField] private GameObject EndGameScreen;
    [SerializeField] private TMP_Text ScoreText;

    private void Start()
    {
        ScoreSystem.OnScoreChanged += AddScore;
        GameState.OnWin += OpenWinScreen;
        GameState.OnLose += OpenLoseScreen;
    }

    private void OpenWinScreen() => OpenGameEnd(true);
    private void OpenLoseScreen() => OpenGameEnd(false);

    private void OnDisable()
    {
        ScoreSystem.OnScoreChanged -= AddScore;
        GameState.OnWin -= OpenWinScreen;
        GameState.OnLose -= OpenLoseScreen;
    }

    private void AddScore(int score)
    {
        ScoreText.text = score.ToString();
    }

    public void OpenConfirmExitTip()
    {
        Pause.SetPause();
        ConfirmExitTip.SetActive(true);
    }

    public void CloseConfirmExitTip()
    {
        ConfirmExitTip.SetActive(false);
        Pause.RemovePause();
    }

    public void ExitToMenu()
    {
        Pause.RemovePause();
        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        Pause.RemovePause();
        SceneManager.LoadScene(1);
    }

    public void OpenGameEnd(bool isWin)
    {
        Pause.SetPause();
        EndGameScreen.SetActive(true);
        EndGameScreen.GetComponent<EndGameScreen>().Instantiate(isWin);
    }
}
