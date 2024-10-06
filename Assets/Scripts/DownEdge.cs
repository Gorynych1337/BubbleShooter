using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownEdge : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bubble bubble = collision.GetComponent<Bubble>();

        if (bubble?.State == EBubbleState.Hang) bubble.DestroyBubble(true);
    }
}
