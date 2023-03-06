using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : NetworkBehaviour
{
    [SerializeField] int maxHealth = 100;

    [SyncVar(hook = nameof(HandleHealthUpdated))]
    int currentHealth;

    public event Action<int, int> ClientOnHealthUpdated;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void DealDamage(int amountOfDamage)
    {
        currentHealth = Mathf.Clamp(currentHealth - amountOfDamage, 0, maxHealth);
    }

    void HandleHealthUpdated(int oldHealth, int newHealth)
    {
        ClientOnHealthUpdated?.Invoke(newHealth, maxHealth);
    }
}
