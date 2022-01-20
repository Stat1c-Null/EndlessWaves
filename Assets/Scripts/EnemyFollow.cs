using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    public float rotSpeed, moveSpeed;
    private float distance;
    public float maxDistance;

    // Start is called before the first frame update
    void Start()
    {
        //Assign player object 
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Move enemy to the player if player is within the distance
        if (Vector3.Distance(player.position ,  gameObject.transform.position) <= maxDistance)
        {
            FollowPlayer();
        }
    }

    void FollowPlayer()
    {
        //Rotate towards the player
        transform.rotation = Quaternion.Slerp(transform.rotation , Quaternion.LookRotation(player.position - transform.position), rotSpeed * Time.deltaTime);
        //Move towards the player
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}