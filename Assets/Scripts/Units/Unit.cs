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

    Targeter targeter = null;
    UnitMovement unitMovement = null;

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

    public UnitState GetUnitState()
    {
        return currentState;
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

        targeter = GetComponent<Targeter>();
        unitMovement = GetComponent<UnitMovement>();
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

    [ClientCallback]
    private void Update()
    {

    }

    [Client]
    public void Hold()
    {
        if (NetworkClient.connection.identity != connectionToClient.identity) { return; }

        targeter.SetTarget(null);
        unitMovement.StopMoving();
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

        targeter.SetTarget(null);
        unitMovement.CmdMoveToPosition(position);
        UpdateUnitState(UnitState.Advancing);
    }

    [Client]
    public void Attack(Targetable target)
    {
        if (NetworkClient.connection.identity != connectionToClient.identity) { return; }

        targeter.SetTarget(target);
        unitMovement.CmdMoveToPosition(target.transform.position);
        UpdateUnitState(UnitState.Attacking);
    }

    [Client]
    public void Defend(Defendable unitToDefend)
    {
        if (NetworkClient.connection.identity != connectionToClient.identity) { return; }

        targeter.SetTarget(null);
        unitMovement.CmdMoveToVicinityOfDefendant(unitToDefend.transform.position);
        UpdateUnitState(UnitState.Defending);
    }

    [Client]
    void UpdateUnitState(UnitState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case UnitState.Holding:
                // If unit gets attacked, defend itself but don't leave this spot
                break;
            case UnitState.Advancing:
                // Keep moving forward
                // If unit comes near an enemy, attack it and keep moving forward
                // If attacked, defend itself and keep moving forward
                // If unit gets to the enemy's turrets or base then attack them
                break;
            case UnitState.Moving:
                // Move to a specific position
                // Do not engage in battle until the unit reaches its destination
                targeter.SetTarget(null);
                break;
            case UnitState.Attacking:
                // Attack a specific enemy
                // Once the enemy is dead then switch to holding state
                break;
            case UnitState.Defending:
                // Move to be within the vicinity of the unitToDefend
                // Attack any enemy that comes near but never leave the unitToDefend
                // Also need to follow the unitToDefend if it ever moves
                break;
        }
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
