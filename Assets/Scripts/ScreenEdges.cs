using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ScreenEdges : MonoBehaviour
{
    [SerializeField] private GameObject upEdgeObject;

    EdgeCollider2D edgeCollider;
    EdgeCollider2D upEdgeCollider;

    private void Awake()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        upEdgeCollider = upEdgeObject.GetComponent<EdgeCollider2D>();
    }

    void Start()
    {
        List<Vector2> screenPoints = new List<Vector2>();
        screenPoints.Add(Camera.main.ScreenToWorldPoint(new Vector2(0,0)));
        screenPoints.Add(Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)));
        screenPoints.Add(Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)));
        screenPoints.Add(Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)));

        edgeCollider.SetPoints(screenPoints);
        upEdgeCollider.SetPoints(new List<Vector2>() { screenPoints[1], screenPoints[2] });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<BubbleShoot>()?.Bounce();
    }
}
