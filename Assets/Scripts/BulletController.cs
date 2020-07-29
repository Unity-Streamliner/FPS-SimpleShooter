using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float moveSpeed;
    public float lifeTime;
    public Rigidbody rigidbody;
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
        Destroy(this.gameObject);
    }
}
