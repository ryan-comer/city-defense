using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{

    public float maxHealth;  // Max health of the character
    public HealthBar healthBar; // Optional: Used to represent the health

    private float currentHealth;  // The current health of the character

    public delegate void DeathDelegate(GameObject deadCharacter);
    public event DeathDelegate OnDeath;

    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }
    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        updateHealthBar();
    }

    // Take damage
    public void Damage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);

        if(currentHealth == 0)
        {
            // Avoid duplicate events
            if (!isDead)
            {
                OnDeath.Invoke(gameObject);
            }
            else
            {
                Debug.Log("DUPLICATE");
            }

            isDead = true;
            Destroy(gameObject);
        }

        updateHealthBar();
    }

    private void updateHealthBar()
    {
        if (healthBar)
        {
            healthBar.SetPercentage(currentHealth/maxHealth);
        }
    }

}
