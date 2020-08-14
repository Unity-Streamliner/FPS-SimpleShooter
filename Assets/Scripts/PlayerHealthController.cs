using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;
    public int maxHealth = 10;
    public int currentHealth;
    public float invincibleLength = 1f;
    private float invincibleCounter;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (invincibleCounter > 0) {
            invincibleCounter -= Time.deltaTime;

        }
    }

    public void DamagePlayer(int damage) 
    {
        if (invincibleCounter <= 0)
        {
            currentHealth -= damage;
            if (currentHealth <= 0){
                gameObject.SetActive(false);
            }
            invincibleCounter = invincibleLength; 
        }
    }
}
