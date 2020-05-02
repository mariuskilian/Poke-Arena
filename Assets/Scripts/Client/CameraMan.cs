using UnityEngine;
using Bolt;

public class CameraMan : GlobalEventListener {

    public static CameraMan Instance { get; private set; }
    private void Awake() { if (Instance == null) Instance = this; }

    public Vector3 CamOffset;
    public Vector3 CamRotationEulers;

    public override void OnEvent(EventManClientInitializedEvent evnt) { if (evnt.RaisedBy == null) SubscribeLocalEventHandlers(); }

    private void AttachCameraToPlayer(Player player) {
        transform.SetParent(player.transform);
        transform.localPosition = CamOffset;
        transform.localRotation = Quaternion.Euler(CamRotationEulers);
    }

    #region Local Event Handlers
    private void SubscribeLocalEventHandlers() {
        ClientGlobalEventMan eventMan = ClientGlobalEventMan.Instance;
        eventMan.PlayerReceivedEvent += HandlePlayerReceivedEvent;
    }

    private void HandlePlayerReceivedEvent(Player player) { AttachCameraToPlayer(player); }
    #endregion

}