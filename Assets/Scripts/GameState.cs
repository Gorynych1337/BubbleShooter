using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    void Start()
    {
        List<Bubble> bubbles = new List<Bubble>((Bubble[])FindObjectsOfType(typeof(Bubble)));

        //bubbles.ForEach()
    }
}
