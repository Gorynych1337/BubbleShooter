using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownEdge : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Bubble>() is null) return;

        Destroy(collision.gameObject);
    }
}
