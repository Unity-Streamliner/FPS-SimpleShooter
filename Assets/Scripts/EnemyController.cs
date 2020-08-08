using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private bool chasing;
    public float distanceToChase = 10f;
    public float distanceToLose = 15f;
    public float distanceToStop = 2f;
    private Vector3 targetPoint;
    public NavMeshAgent navMeshAgent;
    private Vector3 initialPosition;
    public float keepChasingTime = 5f;
    private float chaseCounter;
    public GameObject bullet;
    public Transform firePoint;
    public float fireRate;
    private float fireCount;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        targetPoint = PlayerController.instance.transform.position;
        targetPoint.y = transform.position.y;
        if (!chasing)
        {
            if (Vector3.Distance(transform.position, targetPoint) < distanceToChase) 
            {
                chasing = true;
                fireRate = 1f;
            }

            if (chaseCounter > 0)
            {
                chaseCounter -= Time.deltaTime;
                if (chaseCounter <= 0)
                {
                    navMeshAgent.SetDestination(initialPosition);
                }
            }
        } else 
        {
            if (Vector3.Distance(transform.position, targetPoint) > 2f)
            {
                navMeshAgent.destination = targetPoint;
            } else
            {
                navMeshAgent.destination = transform.position;
            }
            if (Vector3.Distance(transform.position, targetPoint) > distanceToLose)
            {
                chasing = false;
                chaseCounter = keepChasingTime;
            } 
            fireCount -= Time.deltaTime;
            if (fireCount <= 0)
            {
                fireCount = fireRate;
                Instantiate(bullet, firePoint.position, firePoint.rotation);
            }
        }

        
    }
}
