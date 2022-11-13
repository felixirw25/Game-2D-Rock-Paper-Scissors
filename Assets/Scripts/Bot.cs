using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public CardPlayer player;
    public CardGameManager gameManager;
    public BotStats stats;
    private float timer;

    int lastSelected = 0;
    CardNet[] cards;
    public bool IsReady = false;

    public void SetStats(BotStats newStats, bool restoreFullHealth = false){
        this.stats = newStats;
        var newPlayerStats = new PlayerStats{
            MaxHealth = this.stats.MaxHealth,
            RestoreValue = this.stats.RestoreValue,
            DamageValue = this.stats.DamageValue,
        };

        player.SetStats(newPlayerStats, restoreFullHealth);
    }

    IEnumerator Start(){
        cards = GetComponentsInChildren<CardNet>();

        // while(player.IsReady==false)
        //     yield return null;

        yield return new WaitUntil(()=>player.IsReady);
        SetStats(this.stats);
        this.IsReady = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(gameManager.State != CardGameManager.GameState.ChooseAttack){
            timer = 0;
            return;
        }
        if(timer<stats.ChoosingInterval){
            timer += 2*Time.deltaTime;
            return;
        }
        timer = 0;
        ChooseAttack();
    }

    public void ChooseAttack(){
        var random = Random.Range(1, cards.Length);
        var selection = (lastSelected + random) % cards.Length;
        player.SetChosenCard(cards[selection]);
        lastSelected = selection;
    }
}
