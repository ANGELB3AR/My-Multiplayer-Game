using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RTSNetworkManager : NetworkManager
{
    [SerializeField] GameObject playerBasePrefab = null;
    
    bool isGameInProgress = false;

    public List<RTSPlayer> Players { get; } = new List<RTSPlayer>();

    public static event Action ClientOnClientConnected;
    public static event Action ClientOnClientDisconnected;

    #region Server

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if (!isGameInProgress) { return; }

        conn.Disconnect();
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        RTSPlayer player = conn.identity.GetComponent<RTSPlayer>();

        Players.Remove(player);

        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        Players.Clear();

        isGameInProgress = false;
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        RTSPlayer player = conn.identity.GetComponent<RTSPlayer>();

        Players.Add(player);

        // Handle setting player display names and assigning team colors here
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (SceneManager.GetActiveScene().name.StartsWith("Game"))
        {
            // Instantiate GameOverHandler here

            foreach (RTSPlayer player in Players)
            {
                GameObject baseInstance = Instantiate(playerBasePrefab, GetStartPosition().position, Quaternion.identity);
                NetworkServer.Spawn(baseInstance, player.connectionToClient);
            }
        }
    }

    public void StartGame()
    {
        if (Players.Count != 2) { return; }
        isGameInProgress = true;
        ServerChangeScene("Game");
    }

    #endregion

    #region Client

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        ClientOnClientConnected?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        ClientOnClientDisconnected?.Invoke();
    }

    public override void OnStopClient()
    {
        Players.Clear();
    }

    #endregion
}
