using UnityEngine;
using TMPro;
using Bolt;

[BoltGlobalBehaviour(BoltNetworkModes.Client)]
public class PlayerInfoUpdater : GlobalEventListener {

    private Player player;
    private FinanceMan finance;
    private LevelMan level;

    [SerializeField] private TextMeshProUGUI
        coinText = null,
        levelText = null,
        expText = null
        ;

    public override void OnEvent(GameNewPlayer evnt) {
        player = evnt.Player.GetComponent<Player>();
        finance = player.GetManager<FinanceMan>() as FinanceMan;
        level = player.GetManager<LevelMan>() as LevelMan;
    }

    private void Update() {
        if (player == null) return;
        coinText.text = finance.Coins.ToString();
        int lvl = level.Level;
        levelText.text = "Level: " + (lvl + 1);
        expText.text = "EXP: " + level.Exp + "/" + level.MAX_EXP[lvl];
    }
}
