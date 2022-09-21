using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Mirror.Examples.Chat;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectHandler : NetworkBehaviour
{
    public List<Unit> SelectedUnits = new List<Unit>();

    private Camera mainCamera;

    [SerializeField] private RectTransform SelectionImage;

    [SerializeField] private Vector2 SelectionBoxStartPosition;

    private MyRtsPlayer player;

    private bool GameIsRunning = true;

    private bool IsBuilding = false;
    // Start is called before the first frame update
    void Start()
    {
        player = NetworkClient.connection.identity.GetComponent<MyRtsPlayer>();
        mainCamera = Camera.main;
        GameLoopHandler.GameEnded += OnGameStop;
        BuildingMenuUi.OnBuildingSelectStart += StartBuilding;
        BuildingMenuUi.OnBuildingSelectEnd += StopBuilding;


    }

    private void OnDestroy()
    {
        GameLoopHandler.GameEnded -= OnGameStop;
        BuildingMenuUi.OnBuildingSelectStart -= StartBuilding;
        BuildingMenuUi.OnBuildingSelectEnd -= StopBuilding;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameIsRunning){return;}
        if (IsBuilding) { return; }
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
           StartSelectionArea();
        }
        else if (Mouse.current.leftButton.isPressed)
        {
            UpdatePosition();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }
        
    }

    [Client]
    private void StartSelectionArea()
    {
        if (!Keyboard.current.leftShiftKey.isPressed)
        {
            foreach (var selectedUnit in SelectedUnits)
            {
                selectedUnit.DeSelect();
            }
            SelectedUnits.Clear();
        }
        //Code responsible for the selection box
        SelectionBoxStartPosition = Mouse.current.position.ReadValue();
        SelectionImage.anchoredPosition = new Vector2(SelectionBoxStartPosition.x + SelectionImage.sizeDelta.x/2,
            SelectionBoxStartPosition.y - SelectionImage.sizeDelta.y/2);
        SelectionImage.gameObject.SetActive(true);

    }

    private void UpdatePosition()
    {
        Vector2 MousePosition = Mouse.current.position.ReadValue();
        float DeltaX = -SelectionBoxStartPosition.x + MousePosition.x;
        float DeltaY = MousePosition.y - SelectionBoxStartPosition.y;

        SelectionImage.sizeDelta = new Vector2(Mathf.Abs( SelectionBoxStartPosition.x - Mouse.current.position.ReadValue().x),
            Mathf.Abs( SelectionBoxStartPosition.y - Mouse.current.position.ReadValue().y));
        SelectionImage.anchoredPosition = SelectionBoxStartPosition + new Vector2(DeltaX, DeltaY) / 2;


    }

    [Client]
    private void ClearSelectionArea()
    {
        SelectionImage.gameObject.SetActive(false);
        if (SelectionImage.sizeDelta.magnitude <= 0.01f)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            return;
        }

        if (!hit.collider.TryGetComponent<Unit>(out Unit unit))
        {
            return;
        }

        if (unit.hasAuthority)
            SelectedUnits.Add(unit);
        foreach (var selectedUnit in SelectedUnits)
        {
            selectedUnit.Select();
        }
        }
        else
        {
            var min = SelectionImage.anchoredPosition - SelectionImage.sizeDelta / 2;
            var max = SelectionImage.anchoredPosition + SelectionImage.sizeDelta / 2;
            foreach (var unit in player.GetMyUnits())
            {
                //this solves a problem of unit beeeing in a world space instead of screen space
                var UnitPositionOnScreen = mainCamera.WorldToScreenPoint(unit.transform.position);
                if (UnitPositionOnScreen.x >= min.x
                    && UnitPositionOnScreen.y >= min.y
                    && UnitPositionOnScreen.x <= max.x
                    && UnitPositionOnScreen.y <= max.y
                    && !SelectedUnits.Contains(unit) )
                {
                 unit.Select();
                 SelectedUnits.Add(unit);
                }

            }

        }
    }

    private void OnGameStop()
    {
        GameIsRunning = false;
    }

    private void StartBuilding(int PlayerId)
    {
        IsBuilding = true;


    }
    private void StopBuilding(int PlayerID)
    {
        IsBuilding = false;

    }

}
