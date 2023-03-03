using Cinemachine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : NetworkBehaviour
{
    [SerializeField] Transform playerCameraTransform = null;
    [SerializeField] CinemachineTargetGroup cinemachineTargetGroup = null;

    Transform otherPlayerTransform = null;

    [ClientCallback]
    private void Update()
    {
        if (otherPlayerTransform != null) { return; }
        if (((RTSNetworkManager)NetworkManager.singleton).Players.Count != 2) { return; }

        PositionPlayerCamera();
    }

    void PositionPlayerCamera()
    {
        playerCameraTransform.gameObject.SetActive(true);

        foreach (RTSPlayer player in ((RTSNetworkManager)NetworkManager.singleton).Players)
        {
            if (player.connectionToClient.identity == connectionToClient.identity) { continue; }

            otherPlayerTransform = player.connectionToClient.identity.GetComponent<Transform>();
        }

        cinemachineTargetGroup.AddMember(otherPlayerTransform, 1, 0);
        cinemachineTargetGroup.AddMember(gameObject.transform, 1, 0);
    }

    
}
