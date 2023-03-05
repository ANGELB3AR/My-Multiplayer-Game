using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UnitCommandGiver : MonoBehaviour
{
    [SerializeField] Canvas commandGiverDisplay = null;
    [SerializeField] UnitSelectionHandler unitSelectionHandler = null;
    [SerializeField] LayerMask floorMask = new LayerMask();
    [SerializeField] LayerMask targetMask = new LayerMask();

    RTSPlayer player;
    Camera mainCamera;
    CommandGiverState currentState = CommandGiverState.WaitingForCommand;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    [ClientCallback]
    private void Update()
    {
        if (player == null)
        {
            player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            ToggleCommandGiverDisplay();
        }

        switch (currentState)
        {
            case CommandGiverState.WaitingForCommand:
                if (unitSelectionHandler.GetShouldLookForInput()) { return; }
                break;
            case CommandGiverState.LookingForPositionInput:
                LookForPositionInput();
                break;
            case CommandGiverState.LookingForTargetInput:
                LookForTargetInput();
                break;
            case CommandGiverState.LookingForDefendantInput:
                LookForDefendantInput();
                break;
        }
    }

    void LookForPositionInput()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame) { return; }
        
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorMask)) { return; }

        Vector3 position = hit.point;
        
        foreach (Unit selectedUnit in unitSelectionHandler.GetSelectedUnits())
        {
            selectedUnit.unitMovement.CmdMoveToPosition(position);
        }

        unitSelectionHandler.SetShouldLookForInput(true);
        currentState = CommandGiverState.WaitingForCommand;
    }

    void LookForTargetInput()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame) { return; }

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, targetMask)) { return; }

        if (!hit.transform.gameObject.TryGetComponent<Targetable>(out Targetable target)) { return; }

        //if (player.connectionToClient.identity == target.transform.GetComponent<RTSPlayer>().connectionToClient.identity) { return; }

        foreach (Unit selectedUnit in unitSelectionHandler.GetSelectedUnits())
        {
            selectedUnit.targeter.SetTarget(target);
        }

        unitSelectionHandler.SetShouldLookForInput(true);
        currentState = CommandGiverState.WaitingForCommand;
    }

    void LookForDefendantInput()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame) { return; }

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, targetMask)) { return; }

        if (!hit.transform.gameObject.TryGetComponent<Defendable>(out Defendable defendant)) { return; }

        //if (player.connectionToClient.identity != defendant.transform.GetComponent<RTSPlayer>().connectionToClient.identity) { return; }

        foreach (Unit selectedUnit in unitSelectionHandler.GetSelectedUnits())
        {
            selectedUnit.unitMovement.CmdMoveToPosition(defendant.transform.position);
        }

        unitSelectionHandler.SetShouldLookForInput(true);
        currentState = CommandGiverState.WaitingForCommand;
    }

    void ToggleCommandGiverDisplay()
    {
        if (unitSelectionHandler.GetSelectedUnits().Count == 0) { return; }

        commandGiverDisplay.gameObject.SetActive(!commandGiverDisplay.isActiveAndEnabled);

        if (!commandGiverDisplay.gameObject.activeSelf) { return; }

        unitSelectionHandler.SetShouldLookForInput(true);
        currentState = CommandGiverState.WaitingForCommand;
    }

    public void CommandUnitsToHold()
    {
        foreach (Unit selectedUnit in unitSelectionHandler.GetSelectedUnits())
        {
            selectedUnit.unitMovement.StopMoving();
        }

        commandGiverDisplay.gameObject.SetActive(false);
    }

    public void CommandUnitsToAdvance()
    {
        foreach (Unit selectedUnit in unitSelectionHandler.GetSelectedUnits())
        {
            selectedUnit.GetComponent<UnitMovement>().CmdMoveForward();
            ToggleCommandGiverDisplay();
        }
    }

    public void CommandUnitsToDefend()
    {
        unitSelectionHandler.SetShouldLookForInput(false);
        commandGiverDisplay.gameObject.SetActive(false);
        currentState = CommandGiverState.LookingForDefendantInput;
    }

    public void CommandUnitsToAttack()
    {
        unitSelectionHandler.SetShouldLookForInput(false);
        commandGiverDisplay.gameObject.SetActive(false);
        currentState = CommandGiverState.LookingForTargetInput;
    }

    public void CommandUnitsToMove()
    {
        unitSelectionHandler.SetShouldLookForInput(false);
        commandGiverDisplay.gameObject.SetActive(false);
        currentState = CommandGiverState.LookingForPositionInput;
    }

    public enum CommandGiverState
    {
        WaitingForCommand,
        LookingForPositionInput,
        LookingForDefendantInput,
        LookingForTargetInput
    }
}
