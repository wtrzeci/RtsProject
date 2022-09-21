using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameLoopHandler : NetworkBehaviour
{
    [SyncVar]
    public List<MainBase> ListOfBasees = new List<MainBase>();

    public static event Action<string> GameWonClient;
    
    public static event Action GameEnded;

    public override void OnStartServer()
    {
        MainBase.OnBaseSpawnServer += AddBase ;
        MainBase.OnBaseDespawnServer += _RemoveBase;
    }

    public override void OnStopServer()
    {
        MainBase.OnBaseSpawnServer -= AddBase;
        MainBase.OnBaseDespawnServer -= _RemoveBase;
    }

    private void AddBase(MainBase mainBase)
    { 
        Debug.Log("Adding base");
        ListOfBasees.Add(mainBase);
        
    }

    private void _RemoveBase(MainBase mainBase)
    {
        StartCoroutine(RemoveBase(mainBase));
    }
    private IEnumerator RemoveBase(MainBase mainBase)
    {
        Debug.Log("Destroying base");
        if (ListOfBasees.Contains(mainBase))
            ListOfBasees.Remove(mainBase);
        if (ListOfBasees.Count == 1)
        {
            GameEnded?.Invoke();
            yield return new WaitForSeconds(1);
            Time.timeScale = 0f;
            var playerId = ListOfBasees[0].gameObject.GetComponent<NetworkIdentity>().connectionToClient.connectionId + 1;
            RpcGameOver($"Player {playerId}");
            Debug.Log("Game Ended");
        }

    }

    [ClientRpc]
    private void RpcGameOver(string WinnerName)
    {
        GameWonClient?.Invoke(WinnerName);
    }
}
