using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCenter : MonoBehaviour
{
    Vector2 initial_distance;

    private void Awake()
    {
        initial_distance = new Vector2(transform.localPosition.x, transform.localPosition.y);
    }

    // Start is called before the first frame update
    void Update()
    {
        // vector from camera to object

        //global to local camera position
        Vector3 campos = transform.InverseTransformPoint(Camera.main.transform.position);

        Vector3 v = transform.localPosition - campos;
        //normalize vector
        v.Normalize();

        //position object at distance from center to camera at distance on inital distance
        transform.localPosition = new Vector3(- v.x * initial_distance.x, - v.y * initial_distance.y, transform.localPosition.z);
    }

}
