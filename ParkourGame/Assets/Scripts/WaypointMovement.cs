using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointWalker : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 2f;
    public int waitTime = 2;
    public bool pingPongMovement = false;
    public bool shouldWaitAtEndpoints = false;

    private int currentWaypoint = 0;
    private int waypointDirection = 1;
    private bool isWaiting = false;
    private float waitTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        if (waypoints.Length == 0) return;

        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f) isWaiting = false;
            return;
        }

        Transform wp = waypoints[currentWaypoint];

        transform.position = Vector3.MoveTowards(
            transform.position,
            wp.position,
            speed * Time.deltaTime
        );

        Vector3 direction = (wp.position - transform.position).normalized;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);

        if (Vector3.Distance(transform.position, wp.position) <= 0.2f)
        {
            int next = currentWaypoint + waypointDirection;

            if (pingPongMovement)
            {
                if (next >= waypoints.Length || next < 0)
                {
                    waypointDirection *= -1;
                    next = currentWaypoint + waypointDirection;

                    isWaiting = true;
                    waitTimer = waitTime;
                }
            }
            else
            {
                if (next >= waypoints.Length)
                {
                    next = 0;
                }
            }

            currentWaypoint = next;
        }
    }
}
