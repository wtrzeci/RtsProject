using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
   [SerializeField] private NavMeshAgent agent = null;
   private Camera MainCamera;

   [Command]
   private void CmdMove(Vector3 goal)
   {
      if (!NavMesh.SamplePosition(goal,out NavMeshHit hit, 1f, NavMesh.AllAreas)){return;}

      agent.SetDestination(hit.position);
   }

   public override void OnStartAuthority()
   {
      MainCamera = Camera.main;
   }

   [ClientCallback]
   private void Update()
   {
      if (!hasAuthority) {return;}
      if (!Mouse.current.rightButton.isPressed){return;}

      Ray ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
      if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) {return;}
      CmdMove(hit.point);
   }
}
