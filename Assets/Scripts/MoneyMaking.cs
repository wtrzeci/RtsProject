using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyMaking : NetworkBehaviour
{
    [SerializeField] private int MoneyGenerated;
    [SerializeField] private int MoneyGenerationCD;
    public MyRtsPlayer player;

    public override void OnStartServer()
    {
        base.OnStartServer();

    }
    private void Update()
    {
        if (player == null)
        {
            player = connectionToClient.identity.GetComponent<MyRtsPlayer>();
            if (player != null)
                InvokeRepeating("GenerateMoney", 0,MoneyGenerationCD);
        }
        
    }
    private void GenerateMoney()
    {
        player.GenerateMoney(MoneyGenerated);
    }

}
