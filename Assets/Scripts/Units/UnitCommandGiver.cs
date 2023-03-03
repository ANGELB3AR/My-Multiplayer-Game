using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitCommandGiver : MonoBehaviour
{
    [SerializeField] Canvas commandGiverDisplay = null;
    [SerializeField] UnitSelectionHandler unitSelectionHandler = null;

    RTSPlayer player;

    private void OnEnable()
    {
        unitSelectionHandler.ClientOnUnitSelectionUpdated += ClientHandleUnitSelectionUpdated;
    }

    private void OnDisable()
    {
        unitSelectionHandler.ClientOnUnitSelectionUpdated -= ClientHandleUnitSelectionUpdated;
    }

    private void Update()
    {
        if (player == null)
        {
            player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
        }
    }

    [Client]
    void ClientHandleUnitSelectionUpdated()
    {
        commandGiverDisplay.gameObject.SetActive(unitSelectionHandler.selectedUnits.Count >= 1);
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
