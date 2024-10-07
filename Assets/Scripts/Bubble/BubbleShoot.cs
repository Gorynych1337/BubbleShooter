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
    private bool isBubbleDestroyed;
    private GameObject UITouch;

    public delegate void BubbleShootDelegate();
    public static event BubbleShootDelegate OnStick;

    public override void Instantiate()
    {
        pathDrawer = GetComponent<PathDrawer>();
        pathDrawer.Instantiate();


        isBubbleDestroyed = false;
    }
    public override void OnSetState()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        gameObject.layer = LayerMask.NameToLayer("PlayableBubbles");
    }

    public override void OnDestroyHandler()
    {
        OnStick?.Invoke();
    }

    void Update()
    {
        if (isBallFlight)
        {
            BallFlight();
        }
        else
        {
            if (Input.touchCount > 0) touch = Input.GetTouch(0);

            if (!UITouchCheck(touch))
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began: Touch(); break;
                    case TouchPhase.Moved: Drag(); break;
                    case TouchPhase.Ended: Release(); break;
                    default: break;
                }
            }
        }
    }

    private bool UITouchCheck(Touch touch)
    {
        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        {
            UITouch = EventSystem.current.currentSelectedGameObject;
            return true;
        }

        if (EventSystem.current.currentSelectedGameObject == null) return false;

        if (touch.phase == TouchPhase.Ended && EventSystem.current.currentSelectedGameObject.Equals(UITouch))
        {
            return true;
        }

        return false;
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
        GetComponent<Collider2D>().enabled = true;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        pathDrawer.Clear();
        isBallFlight = true;
    }

    private void BallFlight()
    {
        transform.position += shotDirection * Time.deltaTime * shotAcceleration * shotPull;
    }

    public void Bounce()
    {
        shotDirection = new Vector3(-shotDirection.x, shotDirection.y);
    }

    public override void CollisionEnterHandler(Collision2D collision)
    {
        Bubble bubble = collision.gameObject.GetComponent<Bubble>();

        if (bubble is null) return;

        if (shotPull >= maxShotPull && !isBubbleDestroyed)
        {
            isBubbleDestroyed = true;

            bubble.SetColor(GetComponent<Bubble>().Color);
            GetComponent<Bubble>().DestroyBubble(true);
            bubble.GetComponent<BubbleHang>().Pop();
        }
        else
        {
            GetComponent<Bubble>().SetState(EBubbleState.Hang);
            GetComponent<BubbleHang>().AddNeighbour(collision.gameObject);
            GetComponent<BubbleHang>().Pop();
        }

        OnStick.Invoke();
    }
}
