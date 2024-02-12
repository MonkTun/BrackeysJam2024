using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavmeshPathVisualizer : MonoBehaviour
{
    public LineRenderer lr;
    public Transform tr;
    public NavMeshAgent agent;
    bool toggle;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();

        //tr = GetComponent<Transform>();

//        agent = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            toggle = !toggle;
        }
        if (toggle) {
            lr.SetPosition(0, transform.position);
            DrawPath(agent.path);
        }
    }
    void DrawPath(NavMeshPath path)
    {
        if (path.corners.Length < 2) { return; }
        lr.positionCount=1+path.corners.Length;
        for(int i = 1; i <= path.corners.Length; i++)
        {
            lr.SetPosition(i, path.corners[i - 1]);
        }
    }
}
