using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class GameOverDisplay : NetworkBehaviour
{
   [SerializeField] private GameObject CanvasParent;
   [SerializeField] private TMP_Text TextField;
   private void Start()
   {
      GameLoopHandler.GameWonClient += OpenEndGameCanvas;
   }

   private void OnDestroy()
   {
      GameLoopHandler.GameWonClient -= OpenEndGameCanvas;
   }

   private void OpenEndGameCanvas ( string winner)
   {
      CanvasParent.SetActive(true);
      TextField.text = winner + " Has won";

   }

   public void LeaveGame()
   {
      if (NetworkServer.active && NetworkClient.isConnected)
      {
         NetworkManager.singleton.StopHost();
      }
      else
      {
         NetworkManager.singleton.StopClient();
      }
      CanvasParent.SetActive(false);
   }
}
