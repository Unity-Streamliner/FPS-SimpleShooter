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
        if (other.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(damage);
        }
        Destroy(this.gameObject);
        Instantiate(impactEffect, transform.position + (transform.forward * (-moveSpeed * Time.deltaTime)), transform.rotation);
    }
}
