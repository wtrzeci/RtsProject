using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectHandler : NetworkBehaviour
{
    public List<Unit> SelectedUnits = new List<Unit>();

    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            foreach (var selectedUnit in SelectedUnits)
            {
                selectedUnit.DeSelect();
            }
            SelectedUnits.Clear();
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }
        
    }
[Client]
private void ClearSelectionArea()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) {return;}
        if (!hit.collider.TryGetComponent<Unit>(out Unit unit)){return;}
        if(unit.hasAuthority)
        SelectedUnits.Add(unit);
        foreach (var selectedUnit in SelectedUnits)
        {
            selectedUnit.Select();
        }
    }
}
