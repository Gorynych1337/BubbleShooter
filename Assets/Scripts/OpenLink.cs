using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLink : MonoBehaviour
{
    [SerializeField] private string link;

    public void OpenLinkClick()
    {
        Application.OpenURL(link);
    }
}
