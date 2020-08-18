using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float yMultiplier = 1;
    public float velocity;
    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * yMultiplier * velocity);
    }

    private void Awake()
    {
        Destroy(this, 5);
    }
}
