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
    // Double jumps
    private int jumpCount = 0;
    private int extraJumps = 2;

    //Gravity Area
    public bool IsInGravityArea { get; set; }

    public float MaxTimePressButton = 0.5f;

    private GroundChecker _groundChecker;
    private RoofChecker _roofChecker;
    private WallChecker _wallChecker;


    float _lastVelocityY;

    float _buttonPressedTime;


    private void OnEnable()
    {
        PlayerInput.WantToJump += OnJumpStarted;
        PlayerInput.SpaceReleased += OnJumpFinished;
        GroundChecker.OnLanding += OnLanding;
        RoofChecker.OnLanding += OnRoofing;
        WallChecker.OnLanding += OnWalling;
        HighJump.OnJumpBoost += JumpBoost;

    }

    private void OnDisable()
    {
        PlayerInput.WantToJump -= OnJumpStarted;
        PlayerInput.SpaceReleased -= OnJumpFinished;
        GroundChecker.OnLanding -= OnLanding;
        RoofChecker.OnLanding -= OnRoofing;
        WallChecker.OnLanding -= OnWalling;
        HighJump.OnJumpBoost -= JumpBoost;
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
            if (IsInGravityArea)
            {
                InvertGravity();
                IsInGravityArea = false;
            }
            TweakGravity();
        }

        _lastVelocityY = _rigidbody.velocity.y;
    }

    void OnJump()
    {
        if (CanJump())
        {
            Jump();
        }
    }

    private bool CanJump()
    {
        return _groundChecker.IsGrounded || _roofChecker.IsGrounded || _wallChecker.IsGrounded || !_groundChecker.IsGrounded && jumpCount < extraJumps || !_roofChecker.IsGrounded && jumpCount < extraJumps || !_wallChecker.IsGrounded && jumpCount < extraJumps;
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
        jumpCount++;
    }

    private float GetStartSpeed()
    {
        return 2 * JumpHeight / TimeToPeak;
    }

    void ApplyGravity()
    {
        if(jumpCount > 0)
        {
            _rigidbody.gravityScale = -2 * (JumpHeight*2) / (TimeToPeak * TimeToPeak) / Physics2D.gravity.y;
        }
        else
        {
            _rigidbody.gravityScale = GetRegularGravity();
        } 
    }

    private float GetRegularGravity()
    {
        return  -2 * JumpHeight / (TimeToPeak * TimeToPeak) / Physics2D.gravity.y;
    }

    void TweakGravity()
    {
        _rigidbody.gravityScale = GetRegularGravity() * 2;
    }

    void TweakGravityLongJump(float f)
    {
        _rigidbody.gravityScale *= 1 / f;
    }

    public bool OnZenit()
    {
        if (_rigidbody.gravityScale >= 0)
        {
            return (_lastVelocityY > 0 && _rigidbody.velocity.y <= 0f);
        }
        
        return (_lastVelocityY < 0 && _rigidbody.velocity.y >= 0f);
    }

    private void OnLanding()
    {
        _rigidbody.gravityScale = 1;
        jumpCount = 0;
    }

    private void OnRoofing()
    {
        _rigidbody.gravityScale = -1;
        jumpCount = 0;
    }

    private void OnWalling()
    {
        jumpCount = 0;
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
