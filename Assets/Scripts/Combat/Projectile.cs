using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    [SerializeField] Rigidbody rb = null;
    [SerializeField] int damage = 10;
    [SerializeField] bool isRanged = false;
    [SerializeField] float launchForce = 10f;
    [SerializeField] float destroyAfterSeconds = 5f;

    private void Start()
    {
        if (!isRanged)
        {
            launchForce = 0f;
        }

        rb.velocity = transform.forward * launchForce;
    }

    public override void OnStartServer()
    {
        Invoke(nameof(DestroySelf), destroyAfterSeconds);
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out NetworkIdentity networkIdentity))
        {
            if (networkIdentity.connectionToClient == connectionToClient) { return; }
        }

        if (other.TryGetComponent(out Health health))
        {
            health.DealDamage(damage);
            DestroySelf();
        }
    }

    [Server]
    void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
}
