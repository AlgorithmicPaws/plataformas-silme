using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D _rb;
    public float speed;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update() { }

    private void OnEnable()
    {
        _rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject, 0.1f);
            UIManager.instance?.UpdateTextScore(1);
        }
    }
}