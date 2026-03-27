using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerShoot : MonoBehaviour
{
    public GameObject bullet;
    public Transform shootPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Shoot(InputAction.CallbackContext context){
        if(context.started){
            GameObject bulletPrefab = Instantiate(bullet, shootPos.position, shootPos.rotation);
            Destroy(bulletPrefab,1);
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
