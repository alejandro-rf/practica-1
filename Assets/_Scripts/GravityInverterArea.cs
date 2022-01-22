using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityInverterArea : MonoBehaviour
{
    public static Action InvertGravity;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D other)
    {

        var jump = other.GetComponent<Jumper>();
        if (jump.OnZenit())
        {
            InvertGravity?.Invoke();
        }
    }
}
