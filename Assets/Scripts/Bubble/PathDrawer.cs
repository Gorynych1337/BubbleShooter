using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PathDrawer : MonoBehaviour
{
    [SerializeField] private Material slimPathSprite;
    [SerializeField] private Material widePathSprite;

    private LineRenderer lineRenderer;

    public void Instantiate()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void Draw(Vector3 direction, bool isWidePath)
    {
        List<Vector3> points = DrawDirection(transform.position, direction);
        lineRenderer.material = isWidePath ? widePathSprite : slimPathSprite;
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
        //lineRenderer.widthCurve = AnimationCurve.Linear(0, 1, 1, 3);
    }

    private List<Vector3> DrawDirection(Vector3 startPosition, Vector3 direction, List<Vector3> points = null)
    {
        if (points == null) points = new List<Vector3>() { startPosition };
        int layermask = 1 << 3;
        layermask = ~layermask;
        RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, 1000, layermask);

        if (hit.collider == null) return points;

        if (hit.collider.TryGetComponent(out ScreenEdges _))
        {
            Vector2 pointShift = new Vector2(1, direction.y / direction.x) * 0.5f * transform.lossyScale.x;
            Vector3 point = hit.point + (direction.x < 0 ? pointShift : -pointShift);
            points.Add(point);
            points = DrawDirection(point, new Vector3(-direction.x, direction.y), points);
        }
        else
        {
            points.Add(hit.point);
        }

        return points;
    }

    public void Clear()
    {
        lineRenderer.positionCount = 0;
    }
}
