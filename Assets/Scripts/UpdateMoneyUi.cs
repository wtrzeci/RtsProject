using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Mirror;

public class UpdateMoneyUi : MonoBehaviour
{
    //this has to be spawned !
    [SerializeField] private Text TextField;
    private MyRtsPlayer player;
    private void Start()
    {
        TextField.text = MyRtsPlayer.StartingMoney.ToString();
    }
    private void Update()
    {
        if (player == null)
        {
            player = NetworkClient.connection.identity.GetComponent<MyRtsPlayer>();
            if (player != null)
            { 
                player.OnMoneyChange += HandleMoneyChange; 
            }
        }
    }
    private void OnDestroy()
    {
        player.OnMoneyChange -= HandleMoneyChange;
    }
    private void HandleMoneyChange(int newMoney,int id)
    {
        TextField.text = newMoney.ToString();
    }
}
