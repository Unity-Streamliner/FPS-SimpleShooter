using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float moveSpeed;
    public float lifeTime;
    public Rigidbody rigidbody;
    public GameObject impactEffect;
    public int damage;
    public bool damageEnemy;
    public bool damagePlayer;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = transform.forward * moveSpeed;

        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) 
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Enemy" && damageEnemy)
        {
            other.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(damage);
        }
        if (other.tag == "Headshot" && damageEnemy)
        {   
            other.transform.parent.GetComponent<EnemyHealthController>().DamageEnemy(damage * 2);
            Debug.Log("Headshot hit!");
        }
        if (other.tag == "Player" && damagePlayer) {
            Debug.Log("Hit player at " + transform.position);
        }
        Destroy(this.gameObject);
        Instantiate(impactEffect, transform.position + (transform.forward * (-moveSpeed * Time.deltaTime)), transform.rotation);
    }
}
