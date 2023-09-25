using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    //public float maxVelocity = 5;
    private Collider col;

    private void Start() {
        col = GetComponent<Collider>();
    }

    public void SetCollider(bool newValue) => col.enabled = newValue;
}
