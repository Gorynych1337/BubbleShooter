using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class BubbleHang : BubbleState
{
    [SerializeField] private bool isRoot;
    [SerializeField] private List<GameObject> neighbours;
    private List<SpringJoint2D> neighboursJoints;

    [Header("Joits settings")]
    [SerializeField] private float frequency;
    [SerializeField] private float distance;

    private bool isPopped = false;

    public override void Instantiate()
    {
        neighboursJoints = new List<SpringJoint2D>();

        if (isRoot) return;
        neighbours.ForEach(x => CreateJoint(x.GetComponent<Rigidbody2D>()));
    }

    public override void OnSetState()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
        GetComponent<Rigidbody2D>().bodyType = isRoot ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
    }

    public void DeleteFromNeighbours()
    {
        neighbours.ForEach(x => { x.GetComponent<BubbleHang>().DeleteNeighbour(gameObject); });
    }

    public void AddNeighbour(GameObject neighbour)
    {
        if (neighbours.IndexOf(neighbour) != -1) return;

        neighbours.Add(neighbour);
        if (!isRoot) CreateJoint(neighbour.GetComponent<Rigidbody2D>());
    }

    private void CreateJoint(Rigidbody2D connectedBody)
    {
        SpringJoint2D joint = gameObject.AddComponent<SpringJoint2D>();
        joint.enableCollision = true;
        joint.autoConfigureDistance = false;
        joint.distance = distance;
        joint.connectedBody = connectedBody;
        joint.frequency = frequency;
        neighboursJoints.Add(joint);
    }

    public void DeleteNeighbour(GameObject bubble)
    {
        int index = neighbours.IndexOf(bubble);

        if (index != -1)
        {
            neighbours.RemoveAt(index);
            if (isRoot) return;
            Destroy(neighboursJoints[index]);
            neighboursJoints.RemoveAt(index);
        }
    }

    public override void CollisionEnterHandler(Collision2D collision)
    {
        return;
    }

    public int Pop(int count = 0)
    {
        if (!isPopped)
        {
            isPopped = true;

            neighbours.ToList().ForEach(x =>
            {
                if (x.GetComponent<Bubble>().Color.Equals(GetComponent<Bubble>().Color)) count = x.GetComponent<BubbleHang>().Pop(count+1);
            });
        }

        if (count > 2) GetComponent<Bubble>().DestroyBubble(true);
        
        return count;
    }
}
