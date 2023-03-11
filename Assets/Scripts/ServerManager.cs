using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using StarterAssets;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class ServerManager : NetworkBehaviour
{
    public GameObject blackHolePrefab;
    private bool gameStarted = false;

    private IReadOnlyList<NetworkClient> _clients;
    private RobotController[] players;

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 40, 300, 300));
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
        if (GUILayout.Button("Host"))
        {
            var utpTransport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
            if (utpTransport)
            {
                var ip = GameObject.Find("InputField").GetComponent<TMPro.TMP_InputField>().text;
                Debug.Log(ip);
                utpTransport.SetConnectionData(ip, (ushort)7777);
                Debug.Log(utpTransport.ConnectionData.Address + " " + utpTransport.ConnectionData.Port);
            }
            if (!NetworkManager.Singleton.StartHost())
            {
                Debug.LogError("Failed to start host.");
            }
        }
        if (GUILayout.Button("Client"))
        {
            var utpTransport = (UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
            if (utpTransport)
            {
                var ip = GameObject.Find("InputField").GetComponent<TMPro.TMP_InputField>().text;
                Debug.Log(ip);
                utpTransport.SetConnectionData(ip, (ushort)7777);
                Debug.Log(utpTransport.ConnectionData.Address + " " + utpTransport.ConnectionData.Port);
            }
            if (!NetworkManager.Singleton.StartClient())
            {
                Debug.LogError("Failed to start client.");
            }
        }
        if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
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
            players = FindObjectsOfType<RobotController>();
            _clients = NetworkManager.Singleton.ConnectedClientsList;
            
            if (!gameStarted && Keyboard.current.pKey.wasPressedThisFrame)
            {
                StartGame(players);
            }

            if (gameStarted)
            {
                if (OnlyOnePlayerAlive())
                {
                    EndGame();
                }
            }
        }
    }
    

    private bool OnlyOnePlayerAlive()
    {
        var alivePlayers = 0;
        foreach (var player in players)
        {
            if (player.isAlive.Value)
            {
                alivePlayers++;
            }
        }

        return alivePlayers == 1;
    }

    private void StartGame(RobotController[] players)
    {
        gameStarted = true;
        foreach (var player in players)
        {
            player.gameStarted.Value = true;
            player.isAlive.Value = true;
        }
        
        var blackHole = Instantiate(blackHolePrefab);
    }
    
    private void EndGame()
    {
        gameStarted = false;
        
        //reload scene
        var scene = EditorSceneManager.GetActiveScene();
        EditorSceneManager.OpenScene(scene.path);
    }
}