using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private Transform SpawnPosition;
    [SerializeField] private Health health;

    [SerializeField] private float spawnCd;
    [SerializeField] private Image UiClock;
    [SerializeField] private MyRtsPlayer player;
    [SerializeField] private TMP_Text  text; 
    private int unitsInQueue = 0;
    private float timer = 0;
    private float CoolDown;
    private bool IsGameRunning = true;

    private void Start()
    {
        text.text = unitsInQueue.ToString();
        CoolDown = spawnCd;
    }
    public override void OnStartServer()
    {
        health.ServerOnDie += HandleDeath;
        GameLoopHandler.GameEnded += HandleGameEnd;
    }

    public override void OnStopServer()
    {
        health.ServerOnDie -= HandleDeath;
        GameLoopHandler.GameEnded -= HandleGameEnd;
    }
    

    private void HandleDeath(Health _health)
    {
        //implement the list of all buildings
        NetworkServer.Destroy(gameObject);
        Destroy(gameObject);
        
    }
    //we have to make a server command in order to Instantiate object on both clients 
    [Command] 
    private void CmdSpawnUnit()
    {
        var unit = Instantiate(unitPrefab, SpawnPosition.position, Quaternion.identity);
        NetworkServer.Spawn(unit,connectionToClient);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsGameRunning){return;}
        Debug.Log("clicked");

        if (eventData.button != PointerEventData.InputButton.Left){return;}
        if (!hasAuthority) {return;}
        unitsInQueue++;
        text.text = unitsInQueue.ToString();
        //Add a functionality to block unit spawn after game over
    }
  
    private void HandleGameEnd()
    {
        IsGameRunning = false;
    }
    private void Update()
    {
        if (player == null)
        {
            player = NetworkClient.connection.identity.GetComponent<MyRtsPlayer>();
        }
        timer += Time.deltaTime;
        if (timer > spawnCd && unitsInQueue>0)
        {
            spawnCd = timer + CoolDown;
            CmdSpawnUnit();
            unitsInQueue--;
            text.text = unitsInQueue.ToString();
        }
        UiClock.fillAmount = Mathf.Min(1, (timer-(spawnCd-CoolDown)) / CoolDown);
        
    }
}
