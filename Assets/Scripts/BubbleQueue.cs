using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BubbleQueue : MonoBehaviour
{
    [SerializeField] private int bubbleCount;
    private int prevBubbleCount = 0;
    [SerializeField] private bool isRandomFill;
    [SerializeField] private List<EBubbleColor> bubbleColorsList;
    private Queue<EBubbleColor> bubbleColors;

    [SerializeField] private Transform bubbleSpawnpoint;
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private NextBubble swapButton;
    private Bubble bubbleToShoot;

    public delegate void BubbleQueueDeleagte();
    public static event BubbleQueueDeleagte OnQueueEnd;

    private void Start()
    {
        swapButton.GetComponent<Button>().onClick.AddListener(SwapColors);

        bubbleColors = new Queue<EBubbleColor>(bubbleColorsList);
        BubbleShoot.OnStick += SpawnBubble;
        SpawnBubble();
    }

    [ExecuteAlways]
    private void OnValidate()
    {
        if (bubbleCount == prevBubbleCount || bubbleCount <= 0)
        {
            bubbleCount = prevBubbleCount = bubbleColorsList.Count;
        }
        else
        {
            prevBubbleCount = bubbleCount;
            bubbleColorsList = bubbleColorsList.Concat(Enumerable.Repeat(EBubbleColor.Red, bubbleCount)).Take(bubbleCount).ToList();
        }

        if (isRandomFill)
        {
            var random = new System.Random();
            var colorArray = Enum.GetValues(typeof(EBubbleColor));
            bubbleColorsList.Select(x => (EBubbleColor)colorArray.GetValue(random.Next(colorArray.Length)));
        }
    }

    public void SwapColors()
    {
        var tempColor = bubbleToShoot.Color;
        bubbleToShoot.SetColor(bubbleColors.Peek());

        var list = bubbleColors.ToList();
        list[0] = tempColor;
        bubbleColors = new Queue<EBubbleColor>(list);

        swapButton.SetColor(tempColor);
    }

    private void SpawnBubble()
    {
        if (bubbleColors.Count == 0)
        {
            OnQueueEnd?.Invoke();
            return;
        }

        EBubbleColor color = bubbleColors.Dequeue();
        var bubbleObj = Instantiate(bubblePrefab, bubbleSpawnpoint.position, bubbleSpawnpoint.rotation);

        Bubble bubble = bubbleObj.GetComponent<Bubble>();
        bubble.SetColor(color);
        bubble.SetState(EBubbleState.Shoot);

        bubbleToShoot = bubble;

        swapButton.SetCount(bubbleColors.Count);
        if (bubbleColors.Count == 0)
        {
            swapButton.gameObject.SetActive(false);
        }
        else
        {
            swapButton.SetColor(bubbleColors.Peek());
        }
    }
}
