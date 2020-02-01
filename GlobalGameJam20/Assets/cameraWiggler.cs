using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraWiggler : MonoBehaviour
{

    [SerializeField]
    private float cameraShakeFactor = 0f;
    [SerializeField]
    private Vector3 min;
    [SerializeField]
    private Vector3 max;
    [SerializeField]
    private float smoothTime;
    [SerializeField]
    private float maxSpeed;

    private Vector3 vel;

    private Vector3 destination;

    private Transform cameraStartPos;

    private void Start()
    {
        //cameraStartPos.position = transform.position;
        destination = transform.position;
    }

    private void Update()
    {
    
            if(Vector3.Distance(transform.position, destination) <= 0.1f)
        {

            float x = Random.Range(min.x, max.x);
            float y = Random.Range(min.y, max.y);
            float z = Random.Range(min.z, max.z);

            destination = new Vector3(x, y, z);



        }

        transform.position = Vector3.SmoothDamp(transform.position, destination, ref vel, smoothTime, maxSpeed);

    }

}
