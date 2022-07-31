using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private Transform SpawnPosition;
    
    //we have to make a server command in order to Instantiate object on both clients 
    [Command]
    private void CmdSpawnUnit()
    {
        var unit = Instantiate(unitPrefab, SpawnPosition.position, Quaternion.identity);
        NetworkServer.Spawn(unit,connectionToClient);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("clicked");
        if (eventData.button != PointerEventData.InputButton.Left){return;}
        if (!hasAuthority) {return;}
        CmdSpawnUnit();
    }
}
