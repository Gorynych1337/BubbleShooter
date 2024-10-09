using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private int ScoreForBubble;

    public delegate void ScoreDelegate(int score);
    public static event ScoreDelegate OnScoreChanged;

    public static ScoreSystem Instance;
    public int Score { get { return Instance.curentScore; } }
    public int BestScore { get { return Instance.bestScore; } }
    private int curentScore;
    private int bestScore;

    private void OnEnable()
    {
        Instance = this;
    }

    private void Start()
    {
        curentScore = 0;
        bestScore = 0;

        Bubble.OnBubbleDestroyed += Instance.AddScore;
        GameState.OnWin += Instance.SaveScore;
        Instance.bestScore = LoadBestScore();
    }

    private void AddScore(GameObject bubble)
    {
        curentScore += ScoreForBubble;
        OnScoreChanged.Invoke(curentScore);
    }

    private void SaveScore()
    {
        if (curentScore <= bestScore) return;

        PlayerPrefs.SetInt("BestScore", curentScore);
        PlayerPrefs.Save();
    }

    private int LoadBestScore()
    {
        return PlayerPrefs.GetInt("BestScore");
    }
}
