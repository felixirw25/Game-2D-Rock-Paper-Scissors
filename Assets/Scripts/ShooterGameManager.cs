using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class ShooterGameManager : MonoBehaviour, IOnEventCallback
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TMP_Text winnerText;
    public ShooterPlayer P1;
    public ShooterPlayer P2;
    TMP_Text playerName;
    // private const byte playerChangeState = 1;
    // HashSet<int> syncReadyPlayers = new HashSet<int>(2);
    
    // Start is called before the first frame update
    void Start()
    {
        // playerName.text = photonView.Owner.NickName;
        canvas.SetActive(false);
        gameOverPanel.SetActive(false);

        var randomViewportPos = new Vector2(
            Random.Range(0.3f, 0.8f),
            Random.Range(0.3f, 0.8f)
        );
        var randomWorldpos = Camera.main.ViewportToWorldPoint(randomViewportPos);
        randomWorldpos = new Vector3(randomWorldpos.x, randomWorldpos.y, 0);
        PhotonNetwork.Instantiate(playerPrefab.name, randomWorldpos, Quaternion.identity);
    }
    void Update(){
        if(ShooterPlayer.NetPlayers.Count == 2){
            foreach(var netPlayer in ShooterPlayer.NetPlayers){
                if(netPlayer.photonView.IsMine){
                    netPlayer.Set(P1);
                }
                else{
                    netPlayer.Set(P2);
                }
            }
        }

        // P1 = ShooterPlayer.NetPlayers[0];
        // P2 = ShooterPlayer.NetPlayers[1];

        var winner = GetWinner();
        if(winner == null){
            return;
        }
        else{
            canvas.SetActive(true);
            gameOverPanel.SetActive(true);
            winnerText.text = winner == P1 ? $"{P1.playerName.text} is the winner!" : $"{P2.playerName.text} is the winner!";
        }
    }
    private ShooterPlayer GetWinner(){
        if(P1.health==0){
            return P2;
        }
        else if(P2.health==0){
            return P1;
        }
        else{
            return null;
        }
    }
    private void OnEnable(){
        PhotonNetwork.AddCallbackTarget(this);
    }
    private void OnDisable(){
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    public void OnEvent(EventData photonEvent)
    {
        // switch(photonEvent.Code){
        //     case playerChangeState:
        //         var actorNum = (int) photonEvent.CustomData;
        //         syncReadyPlayers.Add(actorNum);
        //         break;
        //     default:
        //         break;
        // }
    }
}
