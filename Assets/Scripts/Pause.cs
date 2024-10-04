using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Pause
{
    public static void SetPause()
    {
        Time.timeScale = 0;
    }

    public static void RemovePause()
    {
        Time.timeScale = 1f;
    }
}
