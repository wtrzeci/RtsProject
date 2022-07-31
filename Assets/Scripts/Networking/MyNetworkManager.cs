using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MyNetworkManager : NetworkManager
{
    [SerializeField] private GameObject unitSpawner;
    public  override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        var objectSpawnerInsance = Instantiate(unitSpawner, conn.identity.transform.position, conn.identity.transform.rotation);
        NetworkServer.Spawn(objectSpawnerInsance,conn);
        Debug.Log("Spawned Spawner" );

    }
}
