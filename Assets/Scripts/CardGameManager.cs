using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class CardGameManager : MonoBehaviour, IOnEventCallback
{
    public GameObject netPlayerPrefab;
    public CardPlayer P1;
    public CardPlayer P2;
    public PlayerStats stats;
    public PlayerStats defaultPlayerStats = new PlayerStats{
        MaxHealth = 100,
        RestoreValue = 5,
        DamageValue = 15,
    };
    public float restoreValue = 10f;
    public float damageValue = 30f;
    public GameState State, NextState = GameState.NetPlayersInizialization;
    public CardPlayer damagedPlayer=null;
    public GameObject gameOverPanel;
    public TMP_Text winnerText;
    public TMP_Text pingText;
    public AudioSource audioSource;
    public AudioClip drawClip;
    private const byte playerChangeState = 1;
    public bool Online = true;
    int streak=0;

    // public List<int> syncReadyPlayers = new List<int>(2);
    HashSet<int> syncReadyPlayers = new HashSet<int>(2); 
    public enum GameState
    {
        SyncState,
        NetPlayersInizialization,
        // Memilih Serangan
        ChooseAttack,
        // Menyerang
        Attacks,
        // Memberi atau menerima damage
        Damages,
        // Kondisi seri
        Draw,
        // Salah1 pihak darah 0
        GameOver,
    }

    // private void Awake(){
    //     PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(PropertyNames.Room.MaxHealth, out var maxHealth);
    //     this.maxHealth = (float)maxHealth;
    //     P1.Health = this.maxHealth;
    //     P1.MaxHealth = this.maxHealth;
    //     P2.Health = this.maxHealth;
    //     P2.MaxHealth = this.maxHealth;
    // }
    private void Start(){
        gameOverPanel.SetActive(false);
        Debug.Log("max health lama: " + stats.MaxHealth);
        Debug.Log("restore lama: " + restoreValue);
        Debug.Log("damage lama: " + damageValue);
        if(Online){
            PhotonNetwork.Instantiate(netPlayerPrefab.name, Vector3.zero, Quaternion.identity);
            StartCoroutine(PingCoroutine());
            State = GameState.NetPlayersInizialization;
            NextState = GameState.NetPlayersInizialization;
            if(PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(PropertyNames.Room.MaxHealth, out var maxHealth)){
                this.stats.MaxHealth = (float)maxHealth;
                Debug.Log("max health baru: " + maxHealth);
                P1.Health = this.stats.MaxHealth;
                P1.MaxHealth = this.stats.MaxHealth;
                P2.Health = this.stats.MaxHealth;
                P2.MaxHealth = this.stats.MaxHealth;
            }
            if(PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(PropertyNames.Room.RestoreValue, out var restoreValue)){
                this.restoreValue = (float)restoreValue;
                Debug.Log("restore health baru: " + restoreValue);

            }
            if(PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(PropertyNames.Room.DamageValue, out var damageValue)){
                this.damageValue = (float)damageValue;
                Debug.Log("damage baru: " + damageValue);
            }
        }
        else{
            State = GameState.ChooseAttack;
        }
        
        // 
        
    }

    // State dan Logika State
    private void Update(){
        switch(State){
            case GameState.SyncState:
                if(syncReadyPlayers.Count==2){
                    syncReadyPlayers.Clear();
                    State = NextState;
                }
                break;
            
            case GameState.NetPlayersInizialization:
                if(CardNetPlayer.NetPlayers.Count == 2){
                    foreach(var netPlayer in CardNetPlayer.NetPlayers){
                        if(netPlayer.photonView.IsMine){
                            netPlayer.Set(P1);
                        }
                        else{
                            netPlayer.Set(P2);
                        }
                    }

                    ChangeState(GameState.ChooseAttack);
                }
                break;

            case GameState.ChooseAttack:
                if(P1.AttackValue != null && P2.AttackValue != null){
                    P1.AnimateAttack();
                    P2.AnimateAttack();
                    P1.isClickable(false);
                    P2.isClickable(false);
                    ChangeState(GameState.Attacks);
                }
                break;
            
            case GameState.Attacks:
                if(P1.IsAnimating() == false && P2.IsAnimating() == false){
                    if(damagedPlayer==GetDamagedPlayer()){
                        streak++;
                        Debug.Log("Sekarang streak: " + streak);
                    }
                    else{
                        streak=0;
                        Debug.Log("streak reset: " + streak);
                    }
                    damagedPlayer = GetDamagedPlayer();
                        if(damagedPlayer != null){
                            damagedPlayer.AnimateDamage();
                            ChangeState(GameState.Damages);
                        }
                        else{
                            ChangeState(GameState.Draw);
                        }
                    
                }
                break;

            case GameState.Damages:
                if(P1.IsAnimating() == false && P2.IsAnimating() == false){
                    if(damagedPlayer == P1){
                        P1.ChangeHealth(-damageValue*(streak+1)); //-P2.stats.DamageValue
                        P2.ChangeHealth(restoreValue*(streak+1)); //P2.stats.RestoreValue
                        Debug.Log("Sekarang vakue damage: " + damageValue*(streak+1));
                    }
                    else{
                        P1.ChangeHealth(restoreValue*(streak+1)); //P1.stats.RestoreValue
                        P2.ChangeHealth(-damageValue*(streak+1)); //-P1.stats.DamageValue
                        Debug.Log("Sekarang vakue damage: " + damageValue*(streak+1));
                    }

                    var winner = GetWinner();
                    if(winner == null){
                        // P1.AnimateBack(1.5f);
                        // P2.AnimateBack(1.5f);
                        ResetPlayers();
                        P1.isClickable(true);
                        P2.isClickable(true);
                        State = GameState.ChooseAttack;
                    }
                    else{
                        gameOverPanel.SetActive(true);
                        winnerText.text = winner == P1 ? $"{P1.Name.text} is the winner!" : $"{P2.Name.text} is the winner!";
                        ResetPlayers();
                        ChangeState(GameState.GameOver);
                    }
                }
            break;

            case GameState.Draw:
                if(P1.IsAnimating() == false && P2.IsAnimating() == false){
                    audioSource.PlayOneShot(drawClip);
                    ResetPlayers();
                    P1.isClickable(true);
                    P2.isClickable(true);
                    ChangeState(GameState.ChooseAttack);
                }
            break;
        }
    }
    private void OnEnable(){
        PhotonNetwork.AddCallbackTarget(this);
    }
    private void OnDisable(){
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void ChangeState(GameState nextState)
    {
        if(Online==false){
            State = nextState;
            return;
        }
        if (this.NextState == nextState)
            return;

        // kirim message bahwa kita sudah siap
        var actorNum = PhotonNetwork.LocalPlayer.ActorNumber;
        var raiseEventOptions = new RaiseEventOptions();
        raiseEventOptions.Receivers = ReceiverGroup.All;
        PhotonNetwork.RaiseEvent(1, actorNum, raiseEventOptions, SendOptions.SendReliable);
        this.State = GameState.SyncState;
        this.NextState = nextState;
    }

    // public void OnEvent(EventData photonEvent)
    // {
    //     if (photonEvent.Code == 1)
    //     {
    //         var actorNum = (int)photonEvent.CustomData;
    //         if (syncReadyPlayers.Contains(actorNum) == false)
    //             syncReadyPlayers.Add(actorNum);
    //     }
    // }

    public void OnEvent(EventData photonEvent)
    {
        switch(photonEvent.Code){
            case playerChangeState:
                var actorNum = (int) photonEvent.CustomData;
                syncReadyPlayers.Add(actorNum);
                break;
            default:
                break;
        }
    }
    IEnumerator PingCoroutine(){
        var wait = new WaitForSeconds(1);
        while(true){
            pingText.text = "ping: " + PhotonNetwork.GetPing() + " ms";
            yield return wait;
        }
    }
    private void ResetPlayers(){
        P1.Reset();
        P2.Reset();
    }
    private CardPlayer GetDamagedPlayer(){
        Attack? PlayerAtk1 = P1.AttackValue;
        Attack? PlayerAtk2 = P2.AttackValue;

        if(PlayerAtk1 == Attack.Rock && PlayerAtk2 == Attack.Paper){
            return P1;
        }
        else if(PlayerAtk1 == Attack.Rock && PlayerAtk2 == Attack.Scissor){
            return P2;
        }
        else if(PlayerAtk1 == Attack.Paper && PlayerAtk2 == Attack.Rock){
            return P2;
        }
        else if(PlayerAtk1 == Attack.Paper && PlayerAtk2 == Attack.Scissor){
            return P1;
        }
        else if(PlayerAtk1 == Attack.Scissor && PlayerAtk2 == Attack.Rock){
            return P1;
        }
        else if(PlayerAtk1 == Attack.Scissor && PlayerAtk2 == Attack.Paper){
            return P2;
        }
        return null;
    }
    private CardPlayer GetWinner(){
        if(P1.Health==0){
            return P2;
        }
        else if(P2.Health==0){
            return P1;
        }
        else{
            return null;
        }
    }
    public void LoadScene(int sceneIndex){
        SceneManager.LoadScene(sceneIndex);
    }
}