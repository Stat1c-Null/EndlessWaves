using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed = 7f;
    public float maxDistance;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        maxDistance += 1 * Time.deltaTime;
        //Destroy bullet if its far away
        if(maxDistance >= 10)
        {
            Destroy(this.gameObject);
        }
    }
}
