using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField] private int WinPercent;
    int rootBubbles = 0;
    int currentBubbles = 0;

    delegate void Win();
    static event Win WinEventHandler;

    void Start()
    {
        List<Bubble> bubbles = new List<Bubble>((Bubble[])FindObjectsOfType(typeof(Bubble)));

        bubbles.ForEach(x =>
        {
            if (x.GetComponent<BubbleHang>().IsRoot) rootBubbles++;
        });

        currentBubbles = rootBubbles;

        Bubble.OnBubbleDestroyed += OnBubbleDestroyed;

        WinEventHandler += GameState_WinEventHandler;
    }

    private void GameState_WinEventHandler()
    {
        Debug.Log("Win");
    }

    private void OnBubbleDestroyed(GameObject bubble)
    {
        if (!bubble.GetComponent<BubbleHang>().IsRoot) return;

        currentBubbles--;

        if ((float)currentBubbles / rootBubbles * 100 <= WinPercent) WinEventHandler?.Invoke();
    }
}
