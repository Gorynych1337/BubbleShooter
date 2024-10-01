using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class BubbleShoot : MonoBehaviour
{
    [SerializeField] private float maxShotAngle;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float maxShotPull;
    [SerializeField] private float shotForce;

    private Touch touch;
    private Vector3 startDragPosition;
    private Vector3 shotDirection;
    private LineRenderer lineRenderer;

    private bool isBallFlight;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0) touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began: Touch(); break;
            case TouchPhase.Moved: Drag(); break;
            case TouchPhase.Ended: Release(); break;
            default: break;
        }

        if (isBallFlight) BallFlight();
    }

    private void OnEnable()
    {
        gameObject.layer = LayerMask.NameToLayer("PlayableBubbles");
    }

    private void Touch()
    {
        startDragPosition = Camera.main.ScreenToWorldPoint(touch.position);
        isBallFlight = false;
    }

    private void Drag()
    {
        Vector3 dragDirection = startDragPosition - Camera.main.ScreenToWorldPoint(touch.position);
        float force = dragDirection.y + Mathf.Abs(dragDirection.x);
        float rotation = -dragDirection.x * rotationSpeed;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Clamp(rotation, -maxShotAngle, maxShotAngle)));
        shotDirection = transform.TransformDirection(Vector3.up);

        if (force <= 0f) return;

        List<Vector3> points = DrawDirection(transform.position, false);
        lineRenderer.widthMultiplier = force / 10;
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    private List<Vector3> DrawDirection(Vector3 startPosition, bool isInverted, List<Vector3> points = null)
    {
        if (points == null) points = new List<Vector3>() { startPosition };

        Vector3 direction = new Vector3(isInverted ? -shotDirection.x : shotDirection.x, shotDirection.y);
        int layermask = 1 << 3;
        layermask = ~layermask;
        RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, Mathf.Infinity, layermask);

        if (hit.collider == null) return points;

        if (hit.collider.TryGetComponent(out ScreenEdges _))
        {
            Vector2 pointShift = new Vector2(1, direction.y / direction.x) * 0.5f * transform.localScale.x;
            Vector3 point = hit.point + (direction.x < 0 ? pointShift : - pointShift);
            points.Add(point);
            points = DrawDirection(point, !isInverted, points);
        }
        else
        {
            points.Add(hit.point);
        }

        return points;
    }

    private void Release()
    {
        isBallFlight = true;
    }

    private void BallFlight()
    {
        transform.position += shotDirection * Time.deltaTime * shotForce;
    }

    public void Bounce()
    {
        shotDirection = new Vector3(-shotDirection.x, shotDirection.y);
    }
}
