using UnityEngine;
using Bolt;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class ArenaMan : ServerManager {

    public Arena[] Arenas { get; private set; }

    private readonly int ArenaSize = 20;
    private readonly int SpaceBetweenArenas = 1;

    public override void OnEvent(GameLoadedEvent evnt) {
        Game = GameMan.Instance as GameMan;
        InitArenas();
    }

    private void InitArenas() {
        Arenas = new Arena[Game.Mode.NumArenas];
        var arenaLayout = Game.Mode.arenaLayout;
        for (int i = 0; i < arenaLayout.Length; i++) {
            for (int j = 0; j < arenaLayout[i].Length; j++) {
                if (!arenaLayout[i, j].active) continue;

                var arenaEntity = BoltNetwork.Instantiate(BoltPrefabs.Arena, GetArenaPosition(i, j), Quaternion.identity);
                Arena arena = arenaEntity.GetComponent<Arena>();
                arena.Shared = arenaLayout[i, j].shared;

                for (int k = 0; k < Arenas.Length; k++) if (Arenas[k] == null) { Arenas[k] = arena; break; }
            }
        }
    }

    private Vector3 GetArenaPosition(int x, int y) {
        int factor = ArenaSize + SpaceBetweenArenas;
        return Vector3.right * x * factor - Vector3.forward * y * factor;
    }

}