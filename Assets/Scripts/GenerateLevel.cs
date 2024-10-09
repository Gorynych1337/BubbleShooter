using Codice.CM.Client.Differences;
using log4net.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[ExecuteInEditMode]
public class GenerateLevel : MonoBehaviour
{
    [SerializeField] TextAsset file;
    [SerializeField] GameObject bubbleTemplate;
    [SerializeField] GameObject levelRightUpPoint;

    private List<List<char>> level;
    private List<GameObject> bubbles;

    public void Generate()
    {
        GameObject.FindGameObjectsWithTag("Bubble").ToList().ForEach(x => { DestroyImmediate(x); });

        bubbles?.ForEach(x => DestroyImmediate(x?.gameObject));

        bubbles = new List<GameObject>();
        level = new List<List<char>>();

        List<string> rows = new List<string>();
        rows = file.text.Split('\n').ToList();
        rows.ForEach(x =>
        {
            level.Add(x.ToList());
        });

        float xPosition = levelRightUpPoint.transform.position.x;
        float yPosition = levelRightUpPoint.transform.position.y;
        int rowCounter = 0;
        List<char> chars = new List<char>() { 'R', 'G', 'B', 'P' };

        level.ForEach(row => {
            row.Where(x => chars.Contains(x)).ToList().ForEach(x => {
                var b = Instantiate(bubbleTemplate);
                bubbles.Add(b);
                b.transform.position = new Vector3(xPosition, yPosition);
                xPosition += bubbleTemplate.transform.lossyScale.x - 0.01f;

                if (rowCounter == 0) b.GetComponent<BubbleHang>().SetRooted();
                switch (x)
                {
                    case 'R': b.GetComponent<Bubble>().SetColor(EBubbleColor.Red); break;
                    case 'G': b.GetComponent<Bubble>().SetColor(EBubbleColor.Green); break;
                    case 'B': b.GetComponent<Bubble>().SetColor(EBubbleColor.Blue); break;
                    case 'P': b.GetComponent<Bubble>().SetColor(EBubbleColor.Purple); break;
                }
                b.GetComponent<Bubble>().Instantiate();

            });
            rowCounter++;
            yPosition -= bubbleTemplate.transform.lossyScale.x * 2 / 3;
            xPosition = levelRightUpPoint.transform.position.x;
            xPosition += rowCounter % 2 == 0 ? 0 : bubbleTemplate.transform.lossyScale.x / 2;
        });
    }
}

[CustomEditor(typeof(GenerateLevel))]
[CanEditMultipleObjects]
class DecalMeshHelperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var script = target as GenerateLevel;

        if (GUILayout.Button("Generate"))
        {
            script.Generate();
        }
    }
}
