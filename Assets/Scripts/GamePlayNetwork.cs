using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayNetwork : MonoBehaviourPunCallbacks
{
    public void BackToMenu(){
        StartCoroutine(BackMenuCR());
    }
    IEnumerator BackMenuCR(){
        PhotonNetwork.Disconnect();
        while(PhotonNetwork.IsConnected)
            yield return null;

        SceneManager.LoadScene("MainMenu");
    }
    public void BackToLobby(){
        StartCoroutine(BackLobbyCR());
    }
    IEnumerator BackLobbyCR(){
        PhotonNetwork.LeaveRoom();
        while(PhotonNetwork.InRoom || PhotonNetwork.IsConnectedAndReady == false)
            yield return null;

        SceneManager.LoadScene("Lobby");
    }
    public void Replay(){
        if(PhotonNetwork.IsMasterClient){
            var scene = SceneManager.GetActiveScene();
            PhotonNetwork.LoadLevel(scene.name);
        }
    }
    public void Quit(){
        StartCoroutine(QuitCR());
    }
    IEnumerator QuitCR(){
        PhotonNetwork.Disconnect();
        while(PhotonNetwork.IsConnected)
            yield return null;

        Application.Quit();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == 1){
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;
            BackToLobby();
        }
    }
}
