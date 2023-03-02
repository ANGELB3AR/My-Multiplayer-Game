using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : NetworkBehaviour
{
    [SerializeField] Targeter targeter = null;

    public static event Action<Unit> ServerOnUnitSpawned;
    public static event Action<Unit> ServerOnUnitDespawned;

    public static event Action<Unit> AuthorityOnUnitSpawned;
    public static event Action<Unit> AuthorityOnUnitDespawned;

    #region Server

    public override void OnStartServer()
    {
        ServerOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        ServerOnUnitDespawned?.Invoke(this);
    }

    #endregion

    #region Client

    public override void OnStartClient()
    {
        gameObject.SetActive(true);
    }

    public override void OnStartAuthority()
    {
        AuthorityOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopAuthority()
    {
        AuthorityOnUnitDespawned?.Invoke(this);
    }

    #endregion
}
