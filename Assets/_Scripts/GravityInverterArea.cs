using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityInverterArea : MonoBehaviour
{
    public static Action InvertGravity;

    private void OnTriggerEnter2D(Collider2D other)
    {

        var jump = other.GetComponent<Jumper>();
        if (jump)
        {
            jump.IsInGravityArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var jump = other.GetComponent<Jumper>();
        if (jump)
        {
            jump.IsInGravityArea = false;
        }
    }
}
