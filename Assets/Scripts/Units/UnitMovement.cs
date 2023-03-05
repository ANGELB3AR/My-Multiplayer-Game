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
    [SerializeField] float speed = 3f;

    Camera mainCamera;

    public void StopMoving()
    {
        agent.ResetPath();
    }

    #region Server

    [Command]
    public void CmdMoveToPosition(Vector3 position)
    {
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }

        agent.SetDestination(hit.position);
    }

    [Command]
    public void CmdMoveForward()
    {
        agent.Move(gameObject.transform.forward * Time.deltaTime * speed);
    }

    #endregion

    #region Client

    [ClientCallback]
    private void Update()
    {
        CmdMoveForward();
    }

    public override void OnStartAuthority()
    {
        mainCamera = Camera.main;
    }

    #endregion
}
