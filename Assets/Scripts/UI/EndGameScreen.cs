using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text header;
    [SerializeField] private string WinHeader;
    [SerializeField] private string LoseHeader;
    [SerializeField] private TMP_Text bestScore;
    [SerializeField] private string bestScoreTemplate;
    [SerializeField] private TMP_Text score;
    [SerializeField] private string scoreTemplate;


    public void Instantiate(bool IsWin)
    {
        header.text = IsWin ? WinHeader : LoseHeader;
        bestScore.text = bestScoreTemplate + ScoreSystem.Instance.BestScore.ToString(); 
        score.text = scoreTemplate + ScoreSystem.Instance.Score.ToString();
    }
}
