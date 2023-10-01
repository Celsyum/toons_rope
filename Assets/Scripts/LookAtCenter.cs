using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCenter : MonoBehaviour
{
    float initial_distance;
    Vector3 initial_position;

    public GameObject numberLabel;
    bool _centered = false;
    public bool centered
    {
        get
        {
            return _centered;
        }
        set
        {
            _centered = value;
            initial_distance = Vector2.Distance(Vector2.zero, initial_position) * (_centered ? 0.01f : 1f);
            SetPosition();
        }
    }

    private void Awake()
    {
        initial_distance = Vector2.Distance(Vector2.zero, transform.localPosition);
        initial_position = transform.localPosition;
    }

    void Start()
    {
        SetPosition();
    }

    void SetPosition()
    {
        Vector2 campos = transform.parent.InverseTransformPoint(Camera.main.transform.position);

        Vector2 vl = new Vector2(initial_position.x, initial_position.y);

        Vector2 v = campos - vl;
        v.Normalize();

        transform.localPosition = new Vector3(-v.x * initial_distance, -v.y * initial_distance, initial_position.z);

        numberLabel.transform.localPosition = new Vector3(v.x * initial_distance * 2f, v.y * initial_distance * 2f, transform.localPosition.z);
    }

}
