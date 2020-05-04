using UnityEngine;
using UnityEngine.EventSystems;
using System;

[BoltGlobalBehaviour(BoltNetworkModes.Client)]
public class SelectionMan : MonoBehaviour {

    public static SelectionMan Instance { get; private set; }
    private void Awake() { if (Instance == null) Instance = this; }

    private const float DragYOffset = -0.3f;

    private Vector3 selectedPos;
    private Unit selectedUnit = null;

    public Action<Unit> UnitSelectEvent;
    public Action<Unit, Vector3, bool> UnitDeselectEvent;

    private void Update() {
        CheckForInput();
        if (selectedUnit != null) DragSelectedUnit();
    }

    private void CheckForInput() {
        if (Input.GetMouseButtonDown(0)) {
            if (selectedUnit == null) SelectUnit();
            else DeselectUnit();
        }
    }

    private void SelectUnit() {
        if (!Camera.main) return;

        Debug.Log("Marius: unit selection start");

        if (EventSystem.current.IsPointerOverGameObject()) return;

        Debug.Log("Marius: Click registered, starting ray cast");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 25f, LayerMask.GetMask("Units"))) {
            if (hit.transform.GetComponent<Unit>().entity.HasControl) {
                selectedUnit = hit.transform.GetComponent<Unit>();
                selectedPos = selectedUnit.transform.position;
            }
        }

        UnitSelectEvent?.Invoke(selectedUnit);
    }

    private void DeselectUnit() {
        if (!Camera.main) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 25f, LayerMask.GetMask("Board", "Bench"))) {
            bool clickedBoard = hit.transform.gameObject.layer == LayerMask.NameToLayer("Board");
            UnitDeselectEvent?.Invoke(selectedUnit, hit.point, clickedBoard);
        } else selectedUnit.transform.position = selectedPos;

        selectedUnit = null;
        selectedPos = Vector3.zero;
    }

    private void DragSelectedUnit() {
        if (!Camera.main) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 25f, LayerMask.GetMask("Drag Unit"))) {
            selectedUnit.transform.position = hit.point + Vector3.up * DragYOffset;
        }
    }

}