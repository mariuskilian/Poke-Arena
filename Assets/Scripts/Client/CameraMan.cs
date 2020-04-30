using UnityEngine;
using Bolt;

public class CameraMan : GlobalEventListener {

    public static CameraMan Instance { get; private set; }

    public Vector3 CamOffset;
    public Vector3 CamRotationEulers;

    private void Awake() { if (Instance == null) Instance = this; }

    public override void OnEvent(PlayerSpawnedEvent evnt) {
        var playerEntity = BoltNetwork.FindEntity(evnt.PlayerNetID);
        transform.SetParent(playerEntity.transform);
        transform.localPosition = CamOffset;
        transform.localRotation = Quaternion.Euler(CamRotationEulers);
    }

}