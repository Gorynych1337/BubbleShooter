using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PathDrawerTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void PathDrawerGetInnerShift()
    {
        var startPosition = new Vector3(0, 0);
        var endPosition = new Vector3(1, 1);
        float spreadAngle = 45;
        float shift = PathDrawer.GetShift(startPosition, endPosition, spreadAngle, true);

        Assert.AreEqual(1f, shift, 0.05f);

        startPosition = new Vector3(0, 0);
        endPosition = new Vector3(-3, 3);
        spreadAngle = 45;
        shift = PathDrawer.GetShift(startPosition, endPosition, spreadAngle, true);

        Assert.AreEqual(3f, shift, 0.05f);
    }

    public void PathDrawerGetOuterShift()
    {
        var startPosition = new Vector3(0, 0);
        var endPosition = new Vector3(1, Mathf.Sqrt(3));
        float spreadAngle = 45;
        float shift = PathDrawer.GetShift(startPosition, endPosition, spreadAngle, false);

        Assert.AreEqual(Mathf.Sqrt(3) - 1, shift, 0.05f);

        startPosition = new Vector3(0, 0);
        endPosition = new Vector3(-3, 3 * Mathf.Sqrt(3));
        spreadAngle = 45;
        shift = PathDrawer.GetShift(startPosition, endPosition, spreadAngle, false);

        Assert.AreEqual(3 * (Mathf.Sqrt(3) - 1), shift, 0.05f);
    }
}
