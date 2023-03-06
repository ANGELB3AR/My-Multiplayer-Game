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

    public event Action ServerOnDie;

    public event Action<int, int> ClientOnHealthUpdated;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void DealDamage(int amountOfDamage)
    {
        if (currentHealth == 0) { return; }

        currentHealth = Mathf.Max(currentHealth - amountOfDamage, 0);

        if (currentHealth != 0) { return; }

        ServerOnDie?.Invoke();
    }

    void HandleHealthUpdated(int oldHealth, int newHealth)
    {
        ClientOnHealthUpdated?.Invoke(newHealth, maxHealth);
    }
}
