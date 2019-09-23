using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField]
    private Transform platform = null;
    [SerializeField]
    private LineRenderer path = null;
    [SerializeField]
    private float speed = 2.0f;

    private Vector3[] pos = null;
    private int pathIndex = 1;
    private bool goingUp = true;

    void Start()
    {
        // Get all positions in path
        pos = new Vector3[path.positionCount];
        path.GetPositions(pos);
    }

    void Update()
    {   
        // If the platform is at the same location as the current path waypoint, get a new waypoint
        if (platform.transform.position == path.transform.TransformPoint(pos[pathIndex]))
        {
            GetNewWaypoint();
        }

        // Move the platform towards the current path waypoint
        platform.transform.position = Vector3.MoveTowards(platform.transform.position, path.transform.TransformPoint(pos[pathIndex]), Time.deltaTime * speed);
    }

    private void GetNewWaypoint() {
        // If the path is a loop, just increment waypoint index unless at end. If at last index, set to beginning
        if (path.loop) 
        {
            if (pathIndex == pos.Length - 1)
            {
                pathIndex = 0;
            }
            else 
            {
                pathIndex++;
            }
        }
        // If not looping
        else 
        {
            // If the pathIndex hits zero, increment index and set increment behaviour to up
            if (pathIndex == 0) 
            {
                pathIndex++;
                goingUp = true;
            }
            // Otherwise if it is at highest index, decrement and set increment behaviour to down
            else if (pathIndex == pos.Length - 1) 
            {
                pathIndex--;
                goingUp = false;
            }
            // Else just increment or decrement depending on increment behaviour
            else 
            {
                if (goingUp) 
                {
                    pathIndex++;
                }
                else 
                {
                    pathIndex--;
                }
            }
        }
    }
}
