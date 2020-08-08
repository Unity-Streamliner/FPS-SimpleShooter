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
    public float waitBetweenShots = 2f;
    private float shotWaitCounter;
    public float timeToShoot = 1f;
    private float shootTimeCounter;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;

        shootTimeCounter = timeToShoot;
        shotWaitCounter = waitBetweenShots;
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
                shootTimeCounter = timeToShoot;
                shotWaitCounter = waitBetweenShots;
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
            if (shotWaitCounter > 0) 
            {
                shotWaitCounter -= Time.deltaTime;
                if (shotWaitCounter <= 0) 
                {
                    shootTimeCounter = timeToShoot;
                }
            } else 
            {
                shootTimeCounter -= Time.deltaTime;
                if (shootTimeCounter > 0)
                {
                    fireCount -= Time.deltaTime;
                    if (fireCount <= 0)
                    {
                        fireCount = fireRate;
                        firePoint.LookAt(PlayerController.instance.transform.position + new Vector3(0f, 1.2f, 0f));
                        // check the angle to the player
                        Vector3 targetDirection = PlayerController.instance.transform.position - transform.position;
                        float angle = Vector3.SignedAngle(targetDirection, transform.forward, Vector3.up); 
                        if (Mathf.Abs(angle) < 30f)
                        {
                            Instantiate(bullet, firePoint.position, firePoint.rotation);
                        } else 
                        {
                            shotWaitCounter = waitBetweenShots;
                        }
                    }
                    navMeshAgent.destination = transform.position;
                } else 
                {
                    shotWaitCounter = waitBetweenShots;
                }
            }
        }

        
    }
}
