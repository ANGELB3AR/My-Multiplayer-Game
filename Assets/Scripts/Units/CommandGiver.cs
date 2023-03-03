using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandGiver : MonoBehaviour
{
    RTSPlayer player;

    private void Update()
    {
        if (player == null)
        {
            player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
        }
    }
}
