using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NextBubble : MonoBehaviour
{
    [SerializeField] private TMP_Text count;
    private Image image;

    public void SetColor(EBubbleColor color)
    {
        switch (color)
        {
            case EBubbleColor.Red: image.color = Color.red; break;
            case EBubbleColor.Green: image.color = Color.green; break;
            case EBubbleColor.Blue: image.color = Color.blue; break;
            case EBubbleColor.Purple: image.color = Color.magenta; break;
        }
    }

    public void SetCount(int _count)
    {
        count.text = _count.ToString();
    }

    void Start()
    {
        image = GetComponent<Image>();
    }
}
