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
    bool isAdvancing = false;

    public void StopMoving()
    {
        agent.ResetPath();
        isAdvancing = false;
    }

    #region Server

    [Command]
    public void CmdMoveToPosition(Vector3 position)
    {
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }

        agent.ResetPath();
        isAdvancing = false;
        agent.SetDestination(hit.position);
    }

    
    public void CmdMoveToVicinityOfDefendant(Defendable defendant)
    {
        if (!NavMesh.SamplePosition(defendant.gameObject.transform.position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) { return; }

        agent.ResetPath();
        isAdvancing = false;
        // TODO: Change this to some point on a unit circle around the defendant
        agent.SetDestination(hit.position);
    }

    [Command]
    public void CmdMoveForward()
    {
        agent.ResetPath();
        isAdvancing = true;
    }

    #endregion

    #region Client

    [ClientCallback]
    private void Update()
    {
        if (!isAdvancing) { return; }
        Advance();
    }

    public override void OnStartAuthority()
    {
        mainCamera = Camera.main;
    }

    void Advance()
    {
        agent.ResetPath();
        agent.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        agent.Move(gameObject.transform.forward * Time.deltaTime * speed);
    }

    #endregion
}
