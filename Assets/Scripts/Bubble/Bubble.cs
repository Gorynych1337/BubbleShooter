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

    [SerializeField] private int destroyFrames;
    private BubbleVisual visual;
    [SerializeField] private EBubbleColor color;

    public EBubbleColor Color { get { return color; } }
    public EBubbleState State { get { return state; } }

    public delegate void BubbleDestroyed(GameObject bubble);
    public static event BubbleDestroyed OnBubbleDestroyed;

    private void Awake()
    {
        Instantiate();
    }

    [ExecuteAlways]
    public void Instantiate()
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
        StartCoroutine("DestroyAnimation");
        hangState.DeleteFromNeighbours();
        curentState.OnDestroyHandler();
        if (CreateEvent) OnBubbleDestroyed?.Invoke(gameObject);

        Destroy(gameObject, Time.fixedDeltaTime * destroyFrames);
    }

    private IEnumerator DestroyAnimation()
    {
        for (int i = 0; i < destroyFrames; i++)
        {
            if (visual == null) yield return null; 
            visual.DestroyBubble(1f / destroyFrames);
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject?.GetComponent<Bubble>() != null)
        {
            hangState.AddNeighbour(collision.gameObject);
        }
        curentState?.CollisionEnterHandler(collision);
    }
}
