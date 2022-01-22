using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(GroundChecker))]
[ExecuteInEditMode]
public class Jumper : MonoBehaviour
{
    private PlayerInput _playerInput;

    private Rigidbody2D _rigidbody;


    public float JumpHeight = 6;
    public float TimeToPeak = 1;

    public float MaxTimePressButton = 0.5f;

    private GroundChecker _groundChecker;
    private RoofChecker _roofChecker;
    private WallChecker _wallChecker;

    float DistanceToGround => _groundChecker.DistanceToGround();

    float _lastVelocityY;

    float _buttonPressedTime;


    private void OnEnable()
    {
        PlayerInput.WantToJump += OnJumpStarted;
        PlayerInput.SpaceReleased += OnJumpFinished;
        GroundChecker.OnLanding += OnLanding;
        RoofChecker.OnLanding += OnRoofing;
        GravityInverterArea.InvertGravity += InvertGravity;
        HighJump.OnJumpBoost += JumpBoost;

    }
    private void OnDisable()
    {
        PlayerInput.WantToJump -= OnJumpStarted;
        PlayerInput.SpaceReleased -= OnJumpFinished;
        GroundChecker.OnLanding -= OnLanding;
        RoofChecker.OnLanding -= OnRoofing;
        GravityInverterArea.InvertGravity -= InvertGravity;
        HighJump.OnJumpBoost -= JumpBoost;
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        float h = -DistanceToGround + JumpHeight;
        Vector3 start = transform.position + new Vector3(-1, h, 0);
        Vector3 end = transform.position + new Vector3(+1, h, 0);
        Gizmos.DrawLine(start, end);
        Gizmos.color = Color.white;
    }



    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _groundChecker = GetComponent<GroundChecker>();
        _roofChecker = GetComponent<RoofChecker>();
        _wallChecker = GetComponent<WallChecker>();
        _playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (OnZenit())
        {
            TweakGravity();
        }

        _lastVelocityY = _rigidbody.velocity.y;
    }

    void OnJump()
    {
        if (_groundChecker.IsGrounded || _roofChecker.IsGrounded || _wallChecker.IsGrounded)
        {
            Jump();
        }  
    }

    void OnJumpStarted()
    {
        _buttonPressedTime = Time.time;
        OnJump();
    }
    void OnJumpFinished()
    {
        float pressedTime = Time.time - _buttonPressedTime;
        float f = Mathf.Clamp01(pressedTime / MaxTimePressButton);
        TweakGravityLongJump(f);
    }

    private void Jump()
    {
        ApplyGravity();
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, GetStartSpeed());
    }

    private float GetStartSpeed()
    {
        return 2 * JumpHeight / TimeToPeak;
    }

    void ApplyGravity()
    {
        _rigidbody.gravityScale = -2 * JumpHeight / (TimeToPeak * TimeToPeak) / Physics2D.gravity.y;
    }

    void TweakGravity()
    {
        _rigidbody.gravityScale *= 2f;
    }

    void TweakGravityLongJump(float f)
    {
        _rigidbody.gravityScale *= 1 / f;
    }

    public bool OnZenit()
    {
        if(_rigidbody.gravityScale >= 0)
        {
            return (_lastVelocityY > 0 && _rigidbody.velocity.y <= 0f); //4.0000 = 4.000000000000001; f1==f2 abs(f1-f2) < epsilon
        }
        else
        {
            return (_lastVelocityY < 0 && _rigidbody.velocity.y >= 0f); //4.0000 = 4.000000000000001; f1==f2 abs(f1-f2) < epsilon
        }
        
    }

    private void OnLanding()
    {
        _rigidbody.gravityScale = 1;
    }

    private void OnRoofing()
    {
        _rigidbody.gravityScale = -1;
    }

    private void InvertGravity()
    {
        _rigidbody.gravityScale *= -1;
        JumpHeight *= -1;
    }


    //POWER UP

    private void JumpBoost()
    {
        JumpHeight *= 2;
    }

}
