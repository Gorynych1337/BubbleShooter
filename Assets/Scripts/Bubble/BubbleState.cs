using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BubbleState: MonoBehaviour
{
    public abstract void Instantiate();

    public abstract void OnSetState();

    public abstract void CollisionEnterHandler(Collision2D collision);
}
