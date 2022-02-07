using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolling : MonoBehaviour
{
    public Transform[] waypoints;
    public int speed;

    private int waypointIndex;
    private float dist;
    public float switchRange = 1f;
    private bool move = true;
    public int waitTime = 3;
    [HideInInspector] public bool patroling = true;
    // Start is called before the first frame update
    void Start()
    {
        waypointIndex = 0;
        //Face towards the way point
        transform.LookAt(waypoints[waypointIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        //Find distance between enemy and distance to next waypoint
        dist = Vector3.Distance(transform.position, waypoints[waypointIndex].position);
        //If enemies distance to waypoint is within range, move to next waypoint
        if(dist < switchRange)
        {
            IncreaseIndex();
            move = false;
        }
        //Wait for 5 seconds before moving to next way point
        if(!move)
        {
            StartCoroutine(WaitBeforeMoving());
        }
        //If enemy didnt come close enough to the player, they will keep patroling
        if(patroling && move) 
        {
            Patrol();
        }
    }
    //Wait for 5 seconds before moving
    IEnumerator WaitBeforeMoving()
    {
        yield return new WaitForSeconds(waitTime);
        move = true;
    }

    void Patrol ()
    {
        //Move forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void IncreaseIndex()
    {
        //Change number of waypoint so AI will move to next waypoint
        waypointIndex++;
        //Reset if we exceeded the number of waypoints in the array
        if(waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0;
        }
        //Update the way point AI is looking at
        transform.LookAt(waypoints[waypointIndex].position);
    }
}
