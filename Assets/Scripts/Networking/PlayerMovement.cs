using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Networking.Types;

public class PlayerMovement : NetworkBehaviour
{
   [SerializeField] private NavMeshAgent agent = null;
   private Camera MainCamera;
   [SerializeField] private SelectHandler selectHandler = null;
   [SerializeField] Unit thisUnit;
   [SerializeField] private Targeter thisTargeter;
   [SerializeField] private float chasingRange;//this should be grater than shooting distance
   [Command]
   private void CmdMove(Vector3 goal)
   {
      if (!NavMesh.SamplePosition(goal,out NavMeshHit hit, 1f, NavMesh.AllAreas)){return;}

      agent.SetDestination(hit.position);
   }

   public override void OnStartAuthority()
   {
      selectHandler = GameObject.Find("UnitSelectHandler").GetComponent<SelectHandler>();
      MainCamera = Camera.main;
   }

   [ClientCallback]
   private void Update()
   {
      if (!hasAuthority) {return;}

      if (Mouse.current.rightButton.isPressed)
      {
         thisTargeter.CmdClearTarget();
      }
      if (thisTargeter.target != null)
      {
         if (Vector3.Distance(transform.position, thisTargeter.target.gameObject.transform.position) > chasingRange)
         {
            CmdMove(thisTargeter.target.transform.position);
         }
         else
         {
            agent.ResetPath();
         }
         return;
      }
      if (!Mouse.current.rightButton.isPressed){return;}
      thisTargeter.CmdClearTarget();

      if (agent.remainingDistance <= agent.stoppingDistance)
      {
         agent.ResetPath();
      }



      Ray ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
      if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) {return;}
      if (!selectHandler.SelectedUnits.Contains(thisUnit)) {return;}
      if (!hit.collider.TryGetComponent<Targetable>(out Targetable target))
      CmdMove(hit.point);
      else
      {
         //this block of code is responsible or attacking
         if (!target.gameObject.GetComponent<NetworkIdentity>().hasAuthority)
            Debug.Log("Target chosen");
            thisTargeter.CmdSetTarget(target.gameObject);
      }
   }
   
}
