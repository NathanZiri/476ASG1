using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestMover : MonoBehaviour
{
    public LayerMask lm;
    private void OnTriggerEnter(Collider other)
    {
        transform.position = new Vector3(Random.Range(-14.5f, 14.5f), 1f, Random.Range(-14.5f, 14.5f));
    }
}
