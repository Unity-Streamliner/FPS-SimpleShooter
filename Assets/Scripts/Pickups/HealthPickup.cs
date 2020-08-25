using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthAmout;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerHealthController.instance.HealPlayer(healthAmout);
            Destroy(this.gameObject);
        }
    }
}
