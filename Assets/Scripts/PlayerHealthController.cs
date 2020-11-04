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
        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = "HEALTH: " + maxHealth + "/" + currentHealth;
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
            UIController.instance.ShowDamage();
            
            if (currentHealth <= 0){
                gameObject.SetActive(false);
                currentHealth = 0;
                GameManager.instance.PlayerDied();
            }
            invincibleCounter = invincibleLength; 

            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = "HEALTH: " + maxHealth + "/" + currentHealth;
        }
    }

    public void HealPlayer(int healAmount) 
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth) 
        {
            currentHealth = maxHealth;
        }
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = "HEALTH: " + maxHealth + "/" + currentHealth;
    }
}
