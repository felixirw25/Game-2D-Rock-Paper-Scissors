using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player P1;
    public Player P2;
    public GameState State = GameState.ChooseAttack;
    private Player damagedPlayer;
    public GameObject gameOverPanel;
    public TMP_Text winnerText;
    public AudioSource audioSource;
    public AudioClip drawClip;
    public enum GameState
    {
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

    private void Start(){
        gameOverPanel.SetActive(false);
    }

    // Testing Start Debug
    // private void Start(){
    //     // Debug.Log(state);

    //     // Mengecek sedang berada di stage apa
    //     Debug.Log(state == State.ChooseAttack);

    //     state = State.Damages;
    //     Debug.Log(state);

    //     Debug.Log(state == State.ChooseAttack);
    // }

    // State dan Logika State
    private void Update(){
        switch(State){
            case GameState.ChooseAttack:
                if(P1.AttackValue != null && P2.AttackValue != null){
                    P1.AnimateAttack();
                    P2.AnimateAttack();
                    P1.isClickable(false);
                    P2.isClickable(false);
                    State = GameState.Attacks;
                }
                break;
            
            case GameState.Attacks:
                if(P1.IsAnimating() == false && P2.IsAnimating() == false){
                    damagedPlayer = GetDamagedPlayer();
                    if(damagedPlayer != null){
                        damagedPlayer.AnimateDamage();
                        State = GameState.Damages;
                    }
                    else{
                        // P1.AnimateBack();
                        // P2.AnimateBack();
                        State = GameState.Draw;
                    }
                }
                break;

            case GameState.Damages:
                if(P1.IsAnimating() == false && P2.IsAnimating() == false){
                    if(damagedPlayer == P1){
                        P1.ChangeHealth(-30);
                        P2.ChangeHealth(5);
                    }
                    else{
                        P1.ChangeHealth(5);
                        P2.ChangeHealth(-30);
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
                        Debug.Log("Sudah Menang");
                        gameOverPanel.SetActive(true);
                        winnerText.text = winner == P1 ? "P1 is the winner!" : "P2 is the winner!";
                        ResetPlayers();
                        State = GameState.GameOver;
                    }
                }

            break;

            case GameState.Draw:
                if(P1.IsAnimating() == false && P2.IsAnimating() == false){
                    audioSource.PlayOneShot(drawClip);
                    ResetPlayers();
                    P1.isClickable(true);
                    P2.isClickable(true);
                    State = GameState.ChooseAttack;
                }
            break;
            
        }
    }
    private void ResetPlayers(){
        damagedPlayer = null;
        P1.Reset();
        P2.Reset();
    }
    private Player GetDamagedPlayer(){
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
    private Player GetWinner(){
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
