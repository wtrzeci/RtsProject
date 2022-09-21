using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class BuildingMenuUi : NetworkBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private string PriceText;
    [SerializeField] private GameObject building;
    [SerializeField] private LayerMask FloorLayerMask = new LayerMask();
    [SerializeField] private Image UiImage;
    public static event Action<int> OnBuildingSelectStart;
    public static event Action<int> OnBuildingSelectEnd;
    private Camera mainCamera;
    private MyRtsPlayer player;
    private GameObject BuildingInstance;
    private Vector3 offset;
    private void Start()
    {
        offset = new Vector3(0,
                0, 0);
        mainCamera = Camera.main;
    }
    private void Update()
    {
        //can be changed just one bool 
        if (player == null)
        {
            player = NetworkClient.connection.identity.GetComponent<MyRtsPlayer>(); 
        }
        if ( BuildingInstance != null )
        {
            UpdateBuildingInstancePosition();
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //Disable the Render in the Ui -> Spawn an object 
            UiImage.enabled = false;
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) { return; }
            var _position = hit.point + offset;
            BuildingInstance = Instantiate(building, _position, Quaternion.identity);
            OnBuildingSelectStart?.Invoke(player.connectionToClient.connectionId);
            BuildingInstance.SetActive(true);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Send command to player object 
        player.CmdSpawnBuilding(BuildingInstance.transform.position, BuildingInstance.GetComponent<Building>().GetBuildingId());
        Destroy(BuildingInstance);
        
        //Checking if position is ok is beeeing done in Player script
        BuildingInstance = null;
        UiImage.enabled = true;
        OnBuildingSelectEnd?.Invoke(player.connectionToClient.connectionId);

        //Build this building through command;
    }

    private void UpdateBuildingInstancePosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity,FloorLayerMask)) { return; }
        var _position = hit.point + new Vector3(0,
            building.transform.localScale.y / 2, 0);
        BuildingInstance.transform.position = _position + offset;
        //check the Colliding object ( super zoom bug) !!
    }

}
