using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitMovement : NetworkBehaviour
{
    [SerializeField] NavMeshAgent agent = null;

    Camera mainCamera;
    bool isTryingToMove = false;

    public bool GetIsTryingToMove()
    {
        return isTryingToMove;
    }

    public void SetIsTryingToMove(bool state)
    {
        isTryingToMove = state;
    }

    public void StopMoving()
    {
        agent.ResetPath();
    }

    #region Server

    [Command]
    void CmdMove(Vector3 position)
    {
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }

        agent.SetDestination(hit.position);

        SetIsTryingToMove(false);
    }

    #endregion

    #region Client

    public override void OnStartAuthority()
    {
        mainCamera = Camera.main;
    }

    [ClientCallback]
    private void Update()
    {
        if (!isOwned) { return; }
        if (!isTryingToMove) { return; }
        
        ListenForNextPosition();
    }

    void ListenForNextPosition()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame) { return; }

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) { return; }

        CmdMove(hit.point);
    }

    #endregion
}
