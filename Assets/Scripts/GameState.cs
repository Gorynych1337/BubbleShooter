using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    [SerializeField] private int WinPercent;
    int rootBubbles = 0;
    int currentBubbles = 0;

    public delegate void GameEndDelegate();
    public static event GameEndDelegate OnWin;
    public static event GameEndDelegate OnLose;

    void Start()
    {
        List<Bubble> bubbles = new List<Bubble>((Bubble[])FindObjectsOfType(typeof(Bubble)));

        bubbles.ForEach(x =>
        {
            if (x.GetComponent<BubbleHang>().IsRoot) rootBubbles++;
        });

        currentBubbles = rootBubbles;

        Bubble.OnBubbleDestroyed += OnBubbleDestroyed;
        BubbleQueue.OnQueueEnd += OnQueueEnd;

        OnWin += GameState_OnWin;
        OnLose += GameState_OnLose;
    }

    private void GameEnd()
    {
        Bubble.OnBubbleDestroyed -= OnBubbleDestroyed;
        BubbleQueue.OnQueueEnd -= OnQueueEnd;
        SceneManager.LoadScene(1);
    }

    private void GameState_OnLose()
    {
        if (this == null) return;
        GameEnd();
        Debug.Log("Lose");
    }

    private void GameState_OnWin()
    {
        if (this == null) return;
        GameEnd();
        Debug.Log("Win");
    }

    private void OnQueueEnd()
    {
        OnLose?.Invoke();
    }

    private void OnBubbleDestroyed(GameObject bubble)
    {
        if (!bubble.GetComponent<BubbleHang>().IsRoot) return;

        currentBubbles--;

        if ((float)currentBubbles / rootBubbles * 100 <= WinPercent) OnWin?.Invoke();
    }
}
