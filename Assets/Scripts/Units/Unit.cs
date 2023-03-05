using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Unit : NetworkBehaviour
{
    [SerializeField] Sprite icon = null;
    [SerializeField] int price = 100;
    [SerializeField] int id = -1;
    [SerializeField] GameObject unitPreview = null;
    [SerializeField] UnityEvent onSelected = null;
    [SerializeField] UnityEvent onDeselected = null;

    UnitState currentState = UnitState.Holding;

    public Targeter targeter = null;
    public UnitMovement unitMovement = null;

    public static event Action<Unit> ServerOnUnitSpawned;
    public static event Action<Unit> ServerOnUnitDespawned;

    public static event Action<Unit> AuthorityOnUnitSpawned;
    public static event Action<Unit> AuthorityOnUnitDespawned;

    public Sprite GetIcon()
    {
        return icon;
    }

    public int GetPrice()
    {
        return price;
    }

    public int GetId()
    {
        return id;
    }

    public GameObject GetUnitPreview()
    {
        return unitPreview;
    }

    [ClientCallback]
    private void Update()
    {

    }

    [Client]
    public void Hold()
    {
        if (NetworkClient.connection.identity != connectionToClient.identity) { return; }

        UpdateUnitState(UnitState.Holding);
    }

    [Client]
    public void Advance()
    {
        if (NetworkClient.connection.identity != connectionToClient.identity) { return; }

        UpdateUnitState(UnitState.Advancing);
    }

    [Client]
    public void Move(Vector3 position)
    {
        if (NetworkClient.connection.identity != connectionToClient.identity) { return; }

        unitMovement.CmdMoveToPosition(position);
        UpdateUnitState(UnitState.Advancing);
    }

    [Client]
    public void Attack(Targetable target)
    {
        if (NetworkClient.connection.identity != connectionToClient.identity) { return; }

        // Pass target to Targeter
        UpdateUnitState(UnitState.Attacking);
    }

    [Client]
    public void Defend(Defendable unitToDefend)
    {
        if (NetworkClient.connection.identity != connectionToClient.identity) { return; }

        // Pass unitToDefend to Defender
        UpdateUnitState(UnitState.Defending);
    }

    [Client]
    void UpdateUnitState(UnitState newState)
    {
        currentState = newState;

        switch(newState)
        {
            case UnitState.Holding:
                // Code block
                break;
            case UnitState.Advancing:
                // Code block
                break;
            case UnitState.Moving:
                // Code block
                break;
            case UnitState.Attacking:
                // Code block
                break;
            case UnitState.Defending:
                // Code block
                break;
        }
    }



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

    [Client]
    public void Select()
    {
        if (!isOwned) { return; }

        onSelected?.Invoke();
    }

    [Client]
    public void Deselect()
    {
        if (!isOwned) { return; }

        onDeselected?.Invoke();
    }

    #endregion

    public enum UnitState
    {
        Holding,
        Advancing,
        Defending,
        Attacking,
        Moving
    }
}
