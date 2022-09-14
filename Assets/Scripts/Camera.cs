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
    public float back = 10;
    //public float rotation = 0;
    private Vector3 velocity = Vector3.zero;

    //METHODS
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitDist = 0.0f;

        //Make camera follow player
        Vector3 targetPoint = ray.GetPoint(hitDist);
        Vector3 pos = new Vector3();

        Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
        
        targetRotation.x = 0;//Dont rotate vertically 
        targetRotation.z = 0;

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 4f * Time.deltaTime);

        pos.x = player.position.x + back;
        pos.z = (player.position.z - distance);
        pos.y = player.position.y + height;

        transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, delay);
        Debug.Log(transform.rotation.eulerAngles.y);

       
       
    }
}
