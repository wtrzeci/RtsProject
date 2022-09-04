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

   [Command]
   private void CmdResetPosition()
   {
      agent.ResetPath();
   }

   public override void OnStartAuthority()
   {
      selectHandler = GameObject.Find("UnitSelectHandler").GetComponent<SelectHandler>();
      MainCamera = Camera.main;
   }
   private void FixedUpdate()
   {
      if (Mouse.current.rightButton.isPressed)
      {
         thisTargeter.CmdClearTarget();
      }
      if (thisTargeter.GetTarget() != null)
      {
         if (Vector3.Distance(transform.position, thisTargeter.GetTarget().transform.position) > chasingRange)
         {
            agent.SetDestination(thisTargeter.GetTarget().transform.position);
         }
         else
         {
            agent.ResetPath();
         }
         return;
      }
      if (!Mouse.current.rightButton.isPressed){return;}
      if (!hasAuthority) {return;}
      
      thisTargeter.CmdClearTarget();

      if (agent.remainingDistance <= agent.stoppingDistance)
      {
         CmdResetPosition();
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
         {
            Debug.Log("Target chosen");
            thisTargeter.CmdSetTarget(target.gameObject);
         }
      }
   }
   
}
