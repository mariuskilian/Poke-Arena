using UnityEngine;
using UnityEngine.EventSystems;
using System;

[BoltGlobalBehaviour(BoltNetworkModes.Client)]
public class SelectionMan : MonoBehaviour {

    public static SelectionMan Instance { get; private set; }
    private void Awake() { if (Instance == null) Instance = this; }

    private const float DragYOffset = -0.3f;

    private Vector3 selectedPos;
    private BoardUnit selectedUnit = null;

    public Action<BoardUnit> UnitSelectEvent;
    public Action<BoardUnit, Vector3, bool> UnitDeselectOnBoardBenchEvent;
    public Action<BoardUnit> UnitDeselectOnVoidEvent;

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

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 25f, LayerMask.GetMask("Units"))) {
            if (hit.transform.GetComponent<BoardUnit>().entity.HasControl) {
                selectedUnit = hit.transform.GetComponent<BoardUnit>();
                selectedPos = selectedUnit.transform.position;
            }
        }

        if (selectedUnit != null) UnitSelectEvent?.Invoke(selectedUnit);
    }

    private void DeselectUnit() {
        if (!Camera.main) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 25f, LayerMask.GetMask("Board", "Bench"))) {
            bool clickedBoard = hit.transform.gameObject.layer == LayerMask.NameToLayer("Board");
            UnitDeselectOnBoardBenchEvent?.Invoke(selectedUnit, hit.point, clickedBoard);
        } else {
            selectedUnit.transform.position = selectedPos;
            UnitDeselectOnVoidEvent?.Invoke(selectedUnit);
        }

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