using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float MovementHorizontal { get; private set; }
    public float MovementVertical { get; private set; }

    public static Action WantToJump;
    public static Action SpaceReleased;
   
    // Update is called once per frame
    void Update()
    {
        MovementHorizontal = Input.GetAxis("Horizontal");
        MovementVertical = Input.GetAxis("Vertical");

        if (Input.GetKeyDown("space"))
        {
            WantToJump?.Invoke();
        }

        if (Input.GetKeyUp("space"))
        {
            SpaceReleased?.Invoke();
        }

    }
}
