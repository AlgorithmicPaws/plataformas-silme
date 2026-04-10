using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{   
    public int maxHealth;
    [HideInInspector] public int currentHealth;

    private Animator animator;
   
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    IEnumerator ReceibeDamage()
    {
        Physics2D.IgnoreLayerCollision(3,6,true);
        currentHealth--;
        animator.SetBool("isDamage", true);
        yield return new WaitForSeconds(2f);
        Physics2D.IgnoreLayerCollision(3,6,false);
        animator.SetBool("isDamage", false);

    }

    public void Damage()
    {
        if (currentHealth>1)
        {
            StartCoroutine(ReceibeDamage());

        }
        else{
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
