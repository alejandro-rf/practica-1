using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public Transform GroundCheckPoint;
    public LayerMask WhatIsGround;

    public bool IsGrounded => _isGrounded;

    private const float _groudCheckRadius = 0.1f;
    private bool _isGrounded;
    private bool _wasGrounded;

    public static Action OnLanding;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckGround();
    }

    void CheckGround()
    {
        var overlap = Physics2D.OverlapCircle(GroundCheckPoint.position, _groudCheckRadius, WhatIsGround);
        _isGrounded = overlap != null;

        if (_isGrounded && !_wasGrounded)
        {
            OnLanding?.Invoke();
        }

        _wasGrounded = _isGrounded;
    }

    public float DistanceToGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, 100, WhatIsGround);
        return hit.distance;
    }
}
