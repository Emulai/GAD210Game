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
    [SerializeField]
    private bool curved = false;

    private Vector3[] pos = null;
    public int pathIndex = 1;
    private bool goingUp = true;
    public Vector3 startPosition;
    private float t = 0.0f;

    void Start()
    {
        // Get all positions in path
        pos = new Vector3[path.positionCount];
        path.GetPositions(pos);
        startPosition = path.transform.TransformPoint(pos[0]);
    }

    void Update()
    {   

        if (curved) {
            t += (Time.deltaTime / 100.0f) * 15.0f;
        }

        // If the platform is at the same location as the current path waypoint, get a new waypoint
        if (platform.transform.position == path.transform.TransformPoint(pos[pathIndex]) || t > 1.0f)
        {
            GetNewWaypoint();
        }

        if (curved) {
            platform.transform.position = Vector3.Slerp(startPosition, path.transform.TransformPoint(pos[pathIndex]), t);
        }
        else {
            // Move the platform towards the current path waypoint
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, path.transform.TransformPoint(pos[pathIndex]), Time.deltaTime * speed);
        }
    }

    private void GetNewWaypoint() {
        if (curved) {
            startPosition = path.transform.TransformPoint(pos[pathIndex]);
            t = 0.0f;
        }

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
