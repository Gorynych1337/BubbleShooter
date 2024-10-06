using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EBubbleState
{
    Shoot,
    Hang
}

public enum EBubbleColor
{
    Red,
    Green,
    Blue,
    Purple
}

public class Bubble : MonoBehaviour
{
    [SerializeField] private EBubbleState state;
    private BubbleShoot shootState;
    private BubbleHang hangState;
    private BubbleState curentState;
    private bool isDestroyed = false;

    private BubbleVisual visual;
    [SerializeField] private EBubbleColor color;

    public EBubbleColor Color { get { return color; } }
    public EBubbleState State { get { return state; } }

    public delegate void BubbleDestroyed(GameObject bubble);
    public static event BubbleDestroyed OnBubbleDestroyed;

    [ExecuteAlways]
    private void Awake()
    {
        shootState = GetComponentInChildren<BubbleShoot>();
        hangState = GetComponentInChildren<BubbleHang>();
        shootState.enabled = false;
        hangState.enabled = false;
        SetState(state);
        
        shootState.Instantiate();
        hangState.Instantiate();

        visual = GetComponentInChildren<BubbleVisual>();
        visual.Instantiate();
        SetVisualColor();
    }

    private void OnValidate()
    {
        visual = GetComponentInChildren<BubbleVisual>();
        visual.Instantiate();
        SetVisualColor();
    }

    public void SetColor(EBubbleColor _color)
    {
        color = _color;
        SetVisualColor();
    }

    private void SetVisualColor()
    {
        switch (color)
        {
            case EBubbleColor.Red: visual.SetColor(UnityEngine.Color.red); break;
            case EBubbleColor.Green: visual.SetColor(UnityEngine.Color.green); break;
            case EBubbleColor.Blue: visual.SetColor(UnityEngine.Color.blue); break;
            case EBubbleColor.Purple: visual.SetColor(UnityEngine.Color.magenta); break;
        }
    }

    public void SetState(EBubbleState _state)
    {
        state = _state;

        if (curentState is not null) curentState.enabled = false;

        switch (state)
        {
            case EBubbleState.Shoot: curentState = shootState; break;
            case EBubbleState.Hang: curentState = hangState; break;
        }

        curentState.enabled = true;
        curentState.OnSetState();
    }

    public void DestroyBubble(bool CreateEvent = false)
    {
        if (isDestroyed) return;
        isDestroyed = true;

        hangState.DeleteFromNeighbours();

        if (CreateEvent) OnBubbleDestroyed.Invoke(gameObject);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Bubble>() is not null) hangState.AddNeighbour(collision.gameObject);
        curentState?.CollisionEnterHandler(collision);
    }
}
