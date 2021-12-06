using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    //VARIABLES
    public Transform player;//Who camera follows
    public float delay = 0.3f;
    public float distance = 7f;
    public float height;
    private Vector3 velocity = Vector3.zero;

    //METHODS
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Make camera follow player
        Vector3 pos = new Vector3();
        pos.x = player.position.x;
        pos.z = player.position.z - distance;
        pos.y = player.position.y + height;
        transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, delay);
    }
}
