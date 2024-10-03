using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleVisual : MonoBehaviour
{
    private SpriteRenderer sprite;

    public void Instantiate()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void SetColor(Color color)
    {
        sprite.color = color;
    }
}
