using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PathDrawer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LineRenderer wideLineRenderer;

    public void Draw(Vector3 direction, bool isWidePath, float spreadAngle = 0)
    {
        wideLineRenderer.gameObject.SetActive(isWidePath);

        List<Vector3> points = GetBouncePoints(transform.position, direction);

        if (!isWidePath)
        {
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPositions(points.ToArray());
        }
        else
        {
            var leftPoints = new List<Vector3>() { points[0] } ;
            var rightPoints = new List<Vector3>() { points[0] } ;

            float leftShift = 0;
            float rightShift = 0;

            for (int i = 1; i < points.Count; i++)
            {
                leftShift += GetShift(points[i - 1], points[i], spreadAngle, direction.x !< 0);
                leftPoints.Add(new Vector3(points[i].x - leftShift, points[i].y));
                rightShift += GetShift(points[i - 1], points[i], spreadAngle, direction.x < 0);
                rightPoints.Add(new Vector3(points[i].x + rightShift, points[i].y));
            }

            lineRenderer.positionCount = leftPoints.Count;
            lineRenderer.SetPositions(leftPoints.ToArray());
            wideLineRenderer.positionCount = rightPoints.Count;
            wideLineRenderer.SetPositions(rightPoints.ToArray());
        }        
    }

    private List<Vector3> GetBouncePoints(Vector3 startPosition, Vector3 direction, List<Vector3> points = null)
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
            points = GetBouncePoints(point, new Vector3(-direction.x, direction.y), points);
        }
        else
        {
            points.Add(hit.point);
        }

        return points;
    }

    public static float GetShift(Vector3 startPosition, Vector3 endPosition, float spreadAngle, bool isInner)
    {
        spreadAngle *= Mathf.Deg2Rad;
        Vector2 line = endPosition - startPosition;
        float againstlineangle = isInner ? Mathf.PI - spreadAngle - Mathf.Asin(line.normalized.y) : Mathf.Asin(line.normalized.y) - spreadAngle;
        return line.magnitude * Mathf.Sin(spreadAngle) / Mathf.Sin(againstlineangle);
    }

    public void Clear()
    {
        lineRenderer.positionCount = 0;
        wideLineRenderer.positionCount = 0;
    }
}
