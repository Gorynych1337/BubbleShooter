using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteAlways]
public class UpEdge : MonoBehaviour
{
    private List<GameObject> rootBubbles;

    private void Awake()
    {
        rootBubbles = new List<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bubble>() is null) return;

        Bubble bubble = collision.GetComponent<Bubble>();

        if (bubble.State == EBubbleState.Shoot)
        {
            bubble.DestroyBubble();
        }
        else
        {
            bubble.GetComponent<BubbleHang>().SetRooted();
            rootBubbles.Add(bubble.gameObject);
        }
    }
}
