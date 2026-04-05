using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Rigidbody2D _rb;
    public float bounceForce;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            _rb.AddForce(transform.up * bounceForce, ForceMode2D.Impulse);
            collision.GetComponent<Animator>().SetBool("dieAnim", true);
            Destroy(collision.gameObject, 0.5f);
        }
    }
}