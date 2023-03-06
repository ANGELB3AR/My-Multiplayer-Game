using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class HealthDisplay : MonoBehaviour
{
    [SerializeField] Health health = null;
    [SerializeField] Image healthBarImage = null;

    private void OnEnable()
    {
        health.ClientOnHealthUpdated += HandleHealthUpdated;
    }

    private void OnDisable()
    {
        health.ClientOnHealthUpdated -= HandleHealthUpdated;
    }

    void HandleHealthUpdated(int currentHealth, int maxHealth)
    {
        healthBarImage.fillAmount = (float)currentHealth / maxHealth;
    }
}
