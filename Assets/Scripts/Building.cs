using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Building : NetworkBehaviour
{
    [SerializeField] private Sprite Icon;

    [SerializeField] private int Id;

    [SerializeField] private int price;

    public static event Action<Building> OnBuildingSpawnServer;

    public static event Action<Building> OnBuildingDespawnServer;

    public static event Action<Building> OnBuildingSpawnClient;

    public static event Action<Building> OnBuildingDespawnClient;
    public override void OnStartServer()
    {
        base.OnStartServer();
        OnBuildingSpawnServer?.Invoke(this);
    }
    public override void OnStopServer()
    {
        base.OnStopServer();
        OnBuildingDespawnServer?.Invoke(this);
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        OnBuildingSpawnClient?.Invoke(this);
    }
    public override void OnStopClient()
    {
        base.OnStopClient();
        OnBuildingDespawnClient?.Invoke(this);
    }

    public int GetBuildingId()
    {
        return Id;
    }
    public int GetPrice()
    {
        return price;
    }


}
