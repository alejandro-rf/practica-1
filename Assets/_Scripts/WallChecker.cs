using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChecker : MonoBehaviour
{
    public Transform LeftWallCheckPoint;
    public Transform RightWallCheckPoint;
    public LayerMask WhatIsGround;

    public bool IsGrounded => _isGrounded;

    private const float _groudCheckRadius = 0.1f;
    private bool _isGrounded;
    private bool _wasGrounded;

    public static Action OnLanding;


    // Update is called once per frame
    void FixedUpdate()
    {
        CheckGround();
    }

    void CheckGround()
    {
        var leftOverlap = Physics2D.OverlapCircle(LeftWallCheckPoint.position, _groudCheckRadius, WhatIsGround);
        var rightOverlap = Physics2D.OverlapCircle(RightWallCheckPoint.position, _groudCheckRadius, WhatIsGround);
        _isGrounded = leftOverlap != null || rightOverlap != null;

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
