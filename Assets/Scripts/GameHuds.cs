using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHuds : MonoBehaviour
{
    [SerializeField] private GameObject ConfirmExitTip;
    [SerializeField] private TMP_Text ScoreText;

    [SerializeField] [InspectorName("Score for one bubble")] private int bubbleScore;
    private int score;

    private void Start()
    {
        Bubble.OnBubbleDestroyed += AddScore;
    }

    private void AddScore(GameObject bubble)
    {
        score += bubbleScore;
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
}
