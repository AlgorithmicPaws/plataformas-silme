using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    private NewInput _newInput;
    private Rigidbody2D _rigidbody;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        _newInput = GetComponent<NewInput>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
    }

    public void PlayerMove()
    {
        _rigidbody.velocity = new Vector2(_newInput.inputX * speed, _rigidbody.velocity.y);
        Flip();
    }
    public void Flip(){
        if(_newInput.inputX > 0 )
        {
            transform.rotation = Quaternion.Euler(0,0,0);
        }
        else if (_newInput.inputX < 0 )
        {
            transform.rotation = Quaternion.Euler(0,180,0);
        }
    }
}
