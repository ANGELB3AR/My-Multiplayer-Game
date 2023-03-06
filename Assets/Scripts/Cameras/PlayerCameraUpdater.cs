using Cinemachine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraUpdater : NetworkBehaviour
{
    public override void OnStartAuthority()
    {
        if (connectionToClient.connectionId == 0) { return; }

        gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
    }
}
