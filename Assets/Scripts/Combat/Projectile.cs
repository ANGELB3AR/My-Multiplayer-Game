using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    [SerializeField] int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Health health)) { return; }

        health.DealDamage(damage);
    }
}
