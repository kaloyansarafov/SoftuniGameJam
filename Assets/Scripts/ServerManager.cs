using System.Text.RegularExpressions;
using StarterAssets;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class ServerManager : NetworkBehaviour
{
    private bool gameStarted = false;
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();
        }

        GUILayout.EndArea();
    }

    static void StartButtons()
    {
        var ip = GUILayout.TextField("127.0.0.1");
        
        if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
        if (GUILayout.Button("Client"))
        {
            var utpTransport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
            if (utpTransport)
            {
                utpTransport.SetConnectionData(Sanitize(ip), 7777);
            }
            if (!NetworkManager.Singleton.StartClient())
            {
                Debug.LogError("Failed to start client.");
            }
        }
        if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
    }
    
    public static string Sanitize(string dirtyString)
    {
        // sanitize the input for the ip address
        return Regex.Replace(dirtyString, "[^A-Za-z0-9.]", "");
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
                        NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }
    
    void Update()
    {
        if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
        {
            var players = FindObjectsOfType<RobotController>();
            if (players.Length > 1)
            {
                if (!gameStarted)
                    if(GUILayout.Button("Begin!")) StartGame(players);
            }
        }
    }
    
    private void StartGame(RobotController[] players)
    {
        gameStarted = true;
        foreach (var player in players)
        {
            player.gameStarted.Value = true;
        }
    }
}