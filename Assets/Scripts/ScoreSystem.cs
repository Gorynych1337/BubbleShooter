using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private int ScoreForBubble;
    private int curentScore;

    public delegate void Score(int score);
    public static event Score OnScoreChanged;

    private void Start()
    {
        Bubble.OnBubbleDestroyed += AddScore;
    }

    private void AddScore(GameObject bubble)
    {
        curentScore += ScoreForBubble;
        OnScoreChanged.Invoke(curentScore);
    }
}
