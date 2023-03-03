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
        commandGiverDisplay.gameObject.SetActive(unitSelectionHandler.selectedUnits.Count > 0);
    }

    public void CommandUnitsToHold()
    {
        foreach (Unit selectedUnit in unitSelectionHandler.selectedUnits)
        {
            selectedUnit.unitMovement.SetIsTryingToMove(false);
            selectedUnit.unitMovement.StopMoving();
        }
    }

    public void CommandUnitsToAdvance()
    {

    }

    public void CommandUnitsToDefend()
    {

    }

    public void CommandUnitsToAttack()
    {

    }

    public void CommandUnitsToMove()
    {
        foreach (Unit selectedUnit in unitSelectionHandler.selectedUnits)
        {
            selectedUnit.unitMovement.SetIsTryingToMove(true);
        }
    }
}
