using UnityEngine;

public class UnitSelection : UnitBehaviour {

    private void Start() {
        if (BoltNetwork.IsClient) {
            InitEventSubscribers();
        }
    }

    private void InitEventSubscribers() {
        SelectionMan selection = SelectionMan.Instance;
        selection.UnitDeselectEvent += HandleUnitDeselectEvent;
    }

    private void HandleUnitDeselectEvent(Unit unit, Vector3 oldPos) {
        if (!IsThisUnit(unit)) return;

        var input = MoveUnitCommand.Create();

        input.FromPosition = oldPos;
        input.ToPosition = unit.transform.localPosition;

        entity.QueueInput(input);
    }

    public override void ExecuteCommand(Bolt.Command command, bool resetState) {
        MoveUnitCommand cmd = (MoveUnitCommand) command;

        if (resetState) {

        } else {
            
        }
    }

}