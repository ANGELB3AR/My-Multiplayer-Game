using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : NetworkBehaviour
{
    [SerializeField] Targetable target = null;
    [SerializeField] float attackRange = 2f;
    [SerializeField] GameObject projectilePrefab = null;
    [SerializeField] Transform projectileSpawnPoint = null;
    [SerializeField] float fireRate = 1f;

    float lastFireTime;

    public Targetable GetTarget()
    {
        return target;
    }

    public void SetTarget(Targetable newTarget)
    {
        target = newTarget;
    }

    private void Update()
    {
        if (target == null) { return; }
        if (Vector3.Distance(gameObject.transform.position, target.transform.position) > attackRange) { return; }
        if (Time.time !> (1/ fireRate) + lastFireTime) { return; }

        AttackTarget();
    }

    [Command]
    void AttackTarget()
    {
        GameObject projectileInstance = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        NetworkServer.Spawn(projectileInstance, connectionToClient);

        lastFireTime = Time.time;
    }
}
