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

    RTSPlayer player;

    [ClientCallback]
    private void Update()
    {
        if (player == null)
        {
            player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
        }

        if (!Mouse.current.rightButton.wasPressedThisFrame) { return; }
        ToggleCommandGiverDisplay();
    }

    void ToggleCommandGiverDisplay()
    {
        if (unitSelectionHandler.GetSelectedUnits().Count == 0) { return; }

        commandGiverDisplay.gameObject.SetActive(!commandGiverDisplay.isActiveAndEnabled);
    }

    public void CommandUnitsToHold()
    {
        foreach (Unit selectedUnit in unitSelectionHandler.GetSelectedUnits())
        {
            selectedUnit.unitMovement.StopMoving();
        }
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

    }

    public void CommandUnitsToAttack()
    {

    }

    void CommandUnitsToMove(Vector3 position)
    {
        foreach (Unit selectedUnit in unitSelectionHandler.GetSelectedUnits())
        {
            selectedUnit.unitMovement.CmdMoveToPosition(position);
        }
    }
}
