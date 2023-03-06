using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;

    int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void DealDamage(int amountOfDamage)
    {
        currentHealth = Mathf.Clamp(currentHealth - amountOfDamage, 0, maxHealth);
    }
}
