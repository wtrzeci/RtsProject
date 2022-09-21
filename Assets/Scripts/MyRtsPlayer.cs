using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MyRtsPlayer : NetworkBehaviour
{
    [SerializeField] List<GameObject> PossibleeBuilds = new List<GameObject>();
   [SerializeField] List<Unit> myUnitList = new List<Unit>();
    [SerializeField] List<Building> myBuildingList = new List<Building>();
    [SerializeField] private float PylonRange = 10;
    [SyncVar(hook = nameof(HandleMoneyChange))]
    public int Money;
    
    [SerializeField] public static int StartingMoney=100;
    public event Action<int,int> OnMoneyChange;
    private void Start()
    {
        Money = StartingMoney;
    }
    public override void OnStartServer()
   {
      base.OnStartServer();
      Unit.OnUnitSpawn += ServerHandleUnitSpawner;
      Unit.OnUnitDeSpawn += ServerHandleUnitDespawn;
      Building.OnBuildingSpawnServer += ServerHandleBuildingSpawn;
      Building.OnBuildingDespawnServer += ServerHandleBuildingDespawn;

   }

    public List<Building> GetMyBuildings()
    {
        return myBuildingList;
    }

   public List<Unit> GetMyUnits()
   {
      return myUnitList;
   }

   public override void OnStopServer()
   {
      base.OnStopServer();
      Unit.OnUnitSpawn -= ServerHandleUnitSpawner;
      Unit.OnUnitDeSpawn -= ServerHandleUnitDespawn;
      Building.OnBuildingSpawnServer -= ServerHandleBuildingSpawn;
      Building.OnBuildingDespawnServer -= ServerHandleBuildingDespawn;

    }

    public override void OnStartClient()
   {
      base.OnStartClient();
      if (!hasAuthority) {return;}
      Unit.OnUnitSpawnClient += ClientHandleUnitSpawn;
      Unit.OnUnitDespawnClient += ClientHandleUnitDespawn;
      Building.OnBuildingSpawnClient += ClientHandleBuildingSpawn;
      Building.OnBuildingDespawnClient += ClientHandleBuildingDespawn;
   }

   
    public override void OnStopClient()
   {
      base.OnStopClient();
      if (!hasAuthority) {return;}
      Unit.OnUnitSpawnClient -= ClientHandleUnitSpawn;
      Unit.OnUnitDespawnClient -= ClientHandleUnitDespawn;
      Building.OnBuildingSpawnClient -= ClientHandleBuildingSpawn;
      Building.OnBuildingDespawnClient -= ClientHandleBuildingDespawn;
    }
    [Server]
   private void ServerHandleUnitSpawner(Unit unit)
   {
      if (unit.connectionToClient.connectionId != connectionToClient.connectionId) {return;}
      myUnitList.Add(unit);
      
   }
    [Server]
   private void ServerHandleUnitDespawn(Unit unit)
   {
      if (unit.connectionToClient.connectionId != connectionToClient.connectionId) {return;}
      myUnitList.Remove(unit);
   }
    [Server]
    private void ServerHandleBuildingDespawn(Building building)
    {
        if (building.connectionToClient.connectionId != connectionToClient.connectionId) { return; }
        myBuildingList.Remove(building);
    }
    [Server]
    private void ServerHandleBuildingSpawn(Building building)
    {
        if (building.connectionToClient.connectionId != connectionToClient.connectionId) { return; }
        myBuildingList.Add(building);
    }

    [ClientCallback]
    private void ClientHandleUnitSpawn(Unit unit)
   {
      if (hasAuthority)
         myUnitList.Add(unit);
   }
    [ClientCallback]
    private void ClientHandleUnitDespawn(Unit unit)
   {
      if (hasAuthority)
         myUnitList.Remove(unit);

   }
    [ClientCallback]
    private void ClientHandleBuildingDespawn(Building building)
    {
        if (hasAuthority)
            myBuildingList.Remove(building);
    }
    [ClientCallback]
    private void ClientHandleBuildingSpawn(Building building)
    {
        if (hasAuthority)
            myBuildingList.Add(building);
    }
    [Command]
    public void CmdSpawnBuilding (Vector3 position, int BuildingID)
    {
        GameObject BuildingToSpawn = null;
        foreach (GameObject building in PossibleeBuilds)
        {
            if (building.GetComponent<Building>().GetBuildingId() == BuildingID)
            {
                BuildingToSpawn = building;
                break;
            }
        }
        if (BuildingToSpawn == null) { return; }
        if (BuildingToSpawn.GetComponent<Building>().GetPrice() > Money) { return; }
        //Checking if pylon is close enough
        bool PylonInProximity = false;
        foreach(Building buildidng in myBuildingList )
        {
            if (buildidng.GetBuildingId() == 3 & Vector3.Distance(buildidng.gameObject.transform.position, position) < PylonRange)
            {
                PylonInProximity = true;
                Debug.Log("Building in proximity");
                Debug.Log(Vector3.Distance(buildidng.gameObject.transform.position, position));

                break;
            }
        }
        if (!PylonInProximity) { return; }
        Money -= BuildingToSpawn.GetComponent<Building>().GetPrice();
        var SpawnedObject = Instantiate(BuildingToSpawn, position, Quaternion.identity);
        NetworkServer.Spawn(SpawnedObject, connectionToClient);
    }
    private void HandleMoneyChange(int NewMoney, int OldMoney)
    {
        OnMoneyChange?.Invoke(NewMoney,NetworkClient.connection.connectionId);
    }
    //Money Generation
    public void GenerateMoney(int MoneyGenerated)
    {
        Money += MoneyGenerated;
    }

}
