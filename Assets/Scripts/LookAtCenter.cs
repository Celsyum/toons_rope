using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCenter : MonoBehaviour
{
    float initial_distance;

    public GameObject numberLabel;

    private void Awake()
    {
        initial_distance = Vector2.Distance(Vector2.zero, transform.localPosition);
    }

    void Start()
    {
        Vector2 campos = transform.parent.InverseTransformPoint(Camera.main.transform.position);

        Vector2 vl = new Vector2(transform.localPosition.x, transform.localPosition.y);

        Vector2 v = campos - vl;
        v.Normalize();

        transform.localPosition = new Vector3(- v.x * initial_distance, - v.y * initial_distance, transform.localPosition.z);

        numberLabel.transform.localPosition = new Vector3(v.x * initial_distance *2f, v.y * initial_distance * 2f, transform.localPosition.z);
    }

}
