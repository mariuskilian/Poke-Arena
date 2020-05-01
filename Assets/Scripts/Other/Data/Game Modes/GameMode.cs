using UnityEngine;

[CreateAssetMenu(fileName = "GameMode", menuName = "Poke-Arena/Game Mode")]
public class GameMode : ScriptableObject {

    public int NumPlayers {
        get {
            int numPlayers = 0;
            for (int i = 0; i < arenaLayout.Length; i++) {
                for (int j = 0; j < arenaLayout[0].Length; j++) {
                    if (!arenaLayout[i, j].active) continue;
                    numPlayers++;
                    if (arenaLayout[i, j].shared) numPlayers++;
                }
            }
            return numPlayers;
        }
    }
    public int NumArenas {
        get {
            int numArenas = 0;
            for (int i = 0; i < arenaLayout.Length; i++)
                for (int j = 0; j < arenaLayout[0].Length; j++) {
                    if (arenaLayout[i, j].active) numArenas++;
                }
            return numArenas;
        }
    }

    public Array2DArena arenaLayout = new Array2DArena(1, 1);

}