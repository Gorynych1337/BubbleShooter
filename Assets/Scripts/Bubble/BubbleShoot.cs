using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class BubbleShoot : BubbleState
{
    [SerializeField][Range(30f, 80f)] private float maxShotAngle;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float maxShotPull;
    [SerializeField] private float shotAcceleration;

    private Touch touch;
    private Vector3 startDragPosition;
    private Vector3 shotDirection;
    private float shotPull;
    private PathDrawer pathDrawer;

    private bool isBallFlight;
    private bool UITouch;

    public override void Instantiate()
    {
        pathDrawer = GetComponent<PathDrawer>();
        pathDrawer.Instantiate();
    }

    void Update()
    {
        if (Input.touchCount > 0) touch = Input.GetTouch(0);

        if (UITouchCheck(touch)) return;

        switch (touch.phase)
        {
            case TouchPhase.Began: Touch(); break;
            case TouchPhase.Moved: Drag(); break;
            case TouchPhase.Ended: Release(); break;
            default: break;
        }

        if (isBallFlight) BallFlight();
    }

    private bool UITouchCheck(Touch touch)
    {
        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) UITouch = true;

        if (touch.phase == TouchPhase.Ended && UITouch)
        {
            UITouch = false;
            return true;
        }

        return UITouch;
    }

    private void Touch()
    {
        startDragPosition = Camera.main.ScreenToWorldPoint(touch.position);
        isBallFlight = false;
    }

    private void Drag()
    {
        Vector3 dragDirection = startDragPosition - Camera.main.ScreenToWorldPoint(touch.position);
        shotPull = dragDirection.y + Mathf.Abs(dragDirection.x);
        float rotation = -dragDirection.x * rotationSpeed;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Clamp(rotation, -maxShotAngle, maxShotAngle)));
        shotDirection = transform.TransformDirection(Vector3.up);

        if (shotPull > 0f) pathDrawer.Draw(shotDirection, shotPull >= maxShotPull);
    }

    private void Release()
    {
        pathDrawer.Clear();
        isBallFlight = true;
    }

    private void BallFlight()
    {
        transform.position += shotDirection * Time.deltaTime * shotAcceleration;
    }

    public void Bounce()
    {
        shotDirection = new Vector3(-shotDirection.x, shotDirection.y);
    }

    public override void CollisionEnterHandler(Collision2D collision)
    {
        Bubble bubble = collision.gameObject.GetComponent<Bubble>();

        if (bubble is null) return;

        if (bubble.State == EBubbleState.Hang)
        {
            GetComponent<Bubble>().SetState(EBubbleState.Hang);
            GetComponent<BubbleHang>().AddNeighbour(collision.gameObject);

            if (bubble.Color.Equals(GetComponent<Bubble>().Color))
            {
                GetComponent<BubbleHang>().Pop();
            }
        }
    }

    public override void OnSetState()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        gameObject.layer = LayerMask.NameToLayer("PlayableBubbles");
    }
}
