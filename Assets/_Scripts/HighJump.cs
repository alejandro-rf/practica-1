using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighJump : MonoBehaviour
{
    public static Action OnJumpBoost;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerMovement>();
        if (player)
        {
            OnJumpBoost?.Invoke();
            Destroy(gameObject);
        }
    }
}
