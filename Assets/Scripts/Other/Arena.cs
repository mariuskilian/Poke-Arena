using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class Arena : EntityBehaviour<IArenaState> {

    public GameObject Player1Handle, Player2Handle;
    [HideInInspector] public Player[] Players;

    public bool TryAddPlayer(Player player) {
        for (int i = 0; i < Players.Length; i++) {
            if (Players[i] == null) {
                var playerHandle = (i == 0) ? Player1Handle : Player2Handle;
                player.InitPlayer(playerHandle.transform);
                Players[i] = player;
                return true;
            }    
        }
        return false;
    }

}
