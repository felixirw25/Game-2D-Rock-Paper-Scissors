using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotDifficultyManager : MonoBehaviour
{
    [SerializeField] Bot bot;
    [SerializeField] int selectedDifficulty;
    [SerializeField] BotStats[] botdifficulties;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitUntil(()=>bot.IsReady);
        var newStats = botdifficulties[selectedDifficulty];
        bot.SetStats(newStats, true);
    }
}
