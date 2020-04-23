using UnityEngine;
using UnityEngine.EventSystems;
using System;

[BoltGlobalBehaviour(BoltNetworkModes.Client)]
public class SelectionMan : Manager {

    #region Singleton
    public static SelectionMan Instance;
    #endregion

    #region Constants
    private const float DRAG_Z_OFFSET = -0.3f;
    #endregion

    #region Variables
    private Vector3 selectedPos;
    private Unit selectedUnit = null;
    #endregion

    #region Events
    public Action<Unit, Vector3> UnitDeselectEvent;
    #endregion

    private void Awake() {
        Instance = this;
    }

    private new void Update() {
        base.Update();
        CheckForInput();
    }

    private void CheckForInput() {
        if (Input.GetMouseButtonDown(0)) {
            if (selectedUnit == null) SelectUnit();
            else DeselectUnit();
        }
    }

    private void SelectUnit() {
        if (!Camera.main) return;
        
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 25f, LayerMask.GetMask("Units"))
            && hit.transform.gameObject.layer == LayerMask.NameToLayer("Units")
            && hit.transform.GetComponent<BoltEntity>().HasControl) {
                selectedUnit = hit.transform.GetComponent<Unit>();
                selectedPos = selectedUnit.transform.localPosition;
        }
    }

    private void DeselectUnit() {
        UnitDeselectEvent?.Invoke(selectedUnit, selectedPos);
        selectedPos = Vector3.zero;
        selectedUnit = null;
    }

    private void DragSelectedUnit() {

    }

}