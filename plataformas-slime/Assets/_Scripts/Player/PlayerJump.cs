using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    public float jumpForce;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Jump()
    {   
            {
        if(Mathf.Abs(_rigidbody.velocity.y) < 0.01f)
            _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    }
}
