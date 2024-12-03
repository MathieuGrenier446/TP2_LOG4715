using UnityEngine;

public class BossController : EnemyController
{
    public AudioClip victorySound;
    public override void Die(){
        // Not useful since we win ?
        PlayerStats.Instance.AwardBossKillExperience();
        BossCinematic.Instance.StartCinematic();
        Emote("NOOOOOOOOOOO!", Color.red);
        PlaySound(victorySound);
    }
}