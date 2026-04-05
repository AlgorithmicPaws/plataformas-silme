using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _animator;
    private NewInput _input;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _input = GetComponent<NewInput>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMoving();
    }

    public void PlayerMoving()
    {
        _animator.SetBool( name: "isMoving", _input.inputX != 0);
    }
}
