using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MyRtsPlayer : NetworkBehaviour
{
   [SerializeField] List<Unit> myUnitList = new List<Unit>();
   public override void OnStartServer()
   {
      base.OnStartServer();
      Unit.OnUnitSpawn += ServerHandleUnitSpawner;
      Unit.OnUnitDeSpawn += ServerHandleUnitDespawn;

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
   }

   public override void OnStartClient()
   {
      base.OnStartClient();
      if (!hasAuthority) {return;}
      Unit.OnUnitSpawnClient += ClientHandleUnitSpawn;
      Unit.OnUnitDespawnClient += ClientHandleUnitDespawn;
   }

   public override void OnStopClient()
   {
      base.OnStopClient();
      if (!hasAuthority) {return;}
      Unit.OnUnitSpawnClient -= ClientHandleUnitSpawn;
      Unit.OnUnitDespawnClient -= ClientHandleUnitDespawn;
   }

   private void ServerHandleUnitSpawner(Unit unit)
   {
      if (unit.connectionToClient.connectionId != connectionToClient.connectionId) {return;}
      myUnitList.Add(unit);
      
   }

   private void ServerHandleUnitDespawn(Unit unit)
   {
      if (unit.connectionToClient.connectionId != connectionToClient.connectionId) {return;}
      myUnitList.Remove(unit);
   }

   private void ClientHandleUnitSpawn(Unit unit)
   {
      if (hasAuthority)
         myUnitList.Add(unit);
   }

   private void ClientHandleUnitDespawn(Unit unit)
   {
      if (hasAuthority)
         myUnitList.Remove(unit);

   }
}
