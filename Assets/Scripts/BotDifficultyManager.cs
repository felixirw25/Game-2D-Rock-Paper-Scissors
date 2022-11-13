using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.RemoteConfig;
using System;

public class BotDifficultyManager : MonoBehaviour
{
    [SerializeField] Bot bot;
    [SerializeField] int selectedDifficulty;
    [SerializeField] BotStats[] botdifficulties;
    [Header("Remote Config Parameters:")]
    [SerializeField] bool enableRemoteConfig = false;
    [SerializeField] string difficultyKey = "Difficulty";
    struct userAttributes{};
    struct appAttributes{};
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitUntil(()=>bot.IsReady);

        // set default difficulty dari inspector
        var newStats = botdifficulties[selectedDifficulty];
        bot.SetStats(newStats, true);

        // ambl difficulty dari remote config kalau enabled
        if(enableRemoteConfig == false)
            yield break;
        
        // tapi tunggu sampai UnityServices siap
        yield return new WaitUntil(()=>UnityServices.State == ServicesInitializationState.Initialized && AuthenticationService.Instance.IsSignedIn);

        // Daftar dulu untuk event fetch completed
        RemoteConfigService.Instance.FetchCompleted += OnRemoteFetched;

        // Lalu fetch di sini cukup awal permainan
        RemoteConfigService.Instance.FetchConfigsAsync(
            new userAttributes(), new appAttributes());
    }

    // Setiap kali data baru didapatkan (melalui fetch), fungsi ini akan dipanggil
    private void OnRemoteFetched(ConfigResponse response)
    {
        if(RemoteConfigService.Instance.appConfig.HasKey(difficultyKey)==false){
            Debug.LogWarning($"Difficulty Key: {difficultyKey} Not Found on Remote Config Server!");
            return;
        }
        
        switch(response.requestOrigin){
            case ConfigOrigin.Default:
            case ConfigOrigin.Cached:       
                break;
            case ConfigOrigin.Remote:
                selectedDifficulty = RemoteConfigService.Instance.appConfig.GetInt(difficultyKey);
                selectedDifficulty = Mathf.Clamp(selectedDifficulty, 0, botdifficulties.Length-1);
                var newStats = botdifficulties[selectedDifficulty];
                bot.SetStats(newStats, true);
                break;
        }
    }
}
