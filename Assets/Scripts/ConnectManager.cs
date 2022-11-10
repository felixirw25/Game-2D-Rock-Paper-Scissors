using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class ConnectManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text feedbackText;
    [SerializeField] TMP_InputField usernameInput;
    public void Start(){
        usernameInput.text = PlayerPrefs.GetString(PropertyNames.Player.NickName, "");
    }
    public void ClickConnect(){
        feedbackText.text = "";
        if(usernameInput.text.Length<3){
            feedbackText.text = "Username Character Must Greater Than 3 Chars";
            return;
        }
        // simpan username
        PlayerPrefs.SetString(PropertyNames.Player.NickName, usernameInput.text);
        PhotonNetwork.NickName = usernameInput.text;
        PhotonNetwork.AutomaticallySyncScene = true;

        // connect ke server
        PhotonNetwork.ConnectUsingSettings();
        feedbackText.text = "Connecting...";
    }

    // dijalankan ketika sudah connect
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        feedbackText.text = "Connected to Master";
        StartCoroutine(LoadLevelAfterConnectedAndReady());
    }

    IEnumerator LoadLevelAfterConnectedAndReady(){
        while(PhotonNetwork.IsConnectedAndReady==false){
            yield return null;
        }
        SceneManager.LoadScene("Lobby");
    }
}
