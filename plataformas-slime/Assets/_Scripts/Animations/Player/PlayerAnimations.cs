using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _animator;
    private NewInput _input;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _input = GetComponent<NewInput>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        PlayerMoving();
        PlayerJumping();
    }

    public void PlayerMoving()
    {
        _animator.SetBool("isMoving", _input.inputX != 0);
    }

    public void PlayerJumping()
    {
        _animator.SetFloat("isJumping", Mathf.Abs(_rigidbody.velocity.y));
    }


}
