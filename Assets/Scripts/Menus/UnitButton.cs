using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Unit unit = null;
    [SerializeField] Image unitIcon = null;
    [SerializeField] TextMeshProUGUI priceText = null;
    [SerializeField] LayerMask floorMask = new LayerMask();

    Camera mainCamera;
    RTSPlayer player;
    CapsuleCollider unitCollider;
    GameObject unitPreviewInstance;
    Renderer unitRendererInstance;

    private void Start()
    {
        mainCamera = Camera.main;
        unitIcon.sprite = unit.GetIcon();
        priceText.text = unit.GetPrice().ToString();
        unitCollider = unit.GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if (player == null)
        {
            player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
        }

        if (unitPreviewInstance == null) { return; }

        UpdateUnitPreview();
    }

    void UpdateUnitPreview()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorMask)) { return; }

        unitPreviewInstance.transform.position = hit.point;

        if (!unitPreviewInstance.activeSelf)
        {
            unitPreviewInstance.SetActive(true);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) { return; }

        unitPreviewInstance = Instantiate(unit.GetUnitPreview());
        unitRendererInstance = unitPreviewInstance.GetComponentInChildren<Renderer>();
        unitPreviewInstance.SetActive(false);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (unitPreviewInstance == null) { return; }

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorMask))
        {
            player.CmdTryPlaceUnit(unit.GetId(), hit.point);
            Destroy(unitPreviewInstance);
        }

        // TODO: [BUG] Need to get the right renderer so preview color updates
        Color color = player.CanPlaceUnit(unitCollider, hit.point) ? Color.green : Color.red;
        unitRendererInstance.material.SetColor("_BaseColor", color);
    }
}
