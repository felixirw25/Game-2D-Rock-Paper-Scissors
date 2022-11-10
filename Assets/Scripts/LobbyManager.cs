using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField newRoomInputField;
    [SerializeField] TMP_Text feedbackText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Button rpsStartGameButton;
    [SerializeField] Button gpsStartGameButton;
    [SerializeField] GameObject roomPanel;
    [SerializeField] GameObject RoomListObject;
    [SerializeField] RoomItem roomItemPrefab;
    List<RoomItem> roomItemList = new List<RoomItem>();
    [SerializeField] GameObject playerListObject;
    [SerializeField] PlayerItem playerItemPrefab;
    List<PlayerItem> playerItemList = new List<PlayerItem>();
    Dictionary<string, RoomInfo> roomInfoCache = new Dictionary<string, RoomInfo>();
    private void Start(){
        PhotonNetwork.JoinLobby();
        roomPanel.SetActive(false);
    }
    public void ClickCreateRoom(){
        feedbackText.text = "";
        if(newRoomInputField.text.Length<3){
            feedbackText.text = "Room Name Must Be Longer Than 3 Chars";
            return;
        }
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        // roomOptions.PlayerTtl = 100;
        PhotonNetwork.CreateRoom(newRoomInputField.text, roomOptions);
        // PhotonNetwork.JoinRandomOrCreateRoom(newRoomInputField.text, roomOptions);
    }
    public void ClickStartGame(string levelName){
        if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.CurrentRoom.IsOpen = false;
        
        PhotonNetwork.LoadLevel(levelName);
    }
    public void JoinRoom(string roomName){
        PhotonNetwork.JoinRoom(roomName);
    }
    public override void OnCreatedRoom(){
        Debug.Log("Created Room: " + PhotonNetwork.CurrentRoom.Name);
        feedbackText.text = "Created Room: " + PhotonNetwork.CurrentRoom.Name;
    }
    public override void OnJoinedRoom(){
        Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);
        feedbackText.text = "Joined Room: " + PhotonNetwork.CurrentRoom.Name;
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        roomPanel.SetActive(true);

        // update player list
        UpdatePlayerList();

        // atur start game button
        SetStartGameButton();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        // update player list
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        // update player list
        UpdatePlayerList();
    }
    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        // atur start game button
        SetStartGameButton();
    }
    private void SetStartGameButton(){
        rpsStartGameButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        rpsStartGameButton.interactable = PhotonNetwork.CurrentRoom.PlayerCount > 1;

        gpsStartGameButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        gpsStartGameButton.interactable = PhotonNetwork.CurrentRoom.PlayerCount > 1;
    }
    private void UpdatePlayerList(){
        //destroy dulu semua pemain yang sudah ada
        foreach(var item in playerItemList){
            Destroy(item.gameObject);
        }
        playerItemList.Clear();

        // bikin ulang player list
        // 1. foreach (Photon.Realtime.Player in PhotonNetwork.PlayerList)
        // 2. PhotonNetwork.CurrentRoom.Players (alternatif)
        foreach(var(id,player) in PhotonNetwork.CurrentRoom.Players){
            PlayerItem newPlayerItem = Instantiate(playerItemPrefab, playerListObject.transform);
            newPlayerItem.Set(player);
            playerItemList.Add(newPlayerItem);

            if(player == PhotonNetwork.LocalPlayer){
                newPlayerItem.transform.SetAsFirstSibling(); //SetSiblingIndex(0)
            }
        }

        // start game hanya bisa diklik ketika pemain berjumlah tertentiu
        // jadi atur juga 
        SetStartGameButton();
    }
    // public override void OnCreateRoomFailed(){

    // }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(var roomInfo in roomList){
            roomInfoCache[roomInfo.Name] = roomInfo;
        }
        foreach(var item in this.roomItemList){
            Destroy(item.gameObject);
        }
        this.roomItemList.Clear();

        var roomInfoList = new List<RoomInfo>(roomInfoCache.Count);
        // Sort yang open di bagian atas
        foreach(var roomInfo in roomInfoCache.Values){
            if(roomInfo.IsOpen)
                roomInfoList.Add(roomInfo);
        }
        // kemudian yg close
        foreach(var roomInfo in roomInfoCache.Values){
            if(roomInfo.IsOpen==false)
                roomInfoList.Add(roomInfo);
        }      

        foreach(var roomInfo in roomInfoList){
            if(roomInfo.IsVisible==false || roomInfo.MaxPlayers == 0) // kalo mau full gak keliatan roomInfo.RemovedFromList
                continue;
            
            RoomItem newRoomItem = Instantiate(roomItemPrefab, RoomListObject.transform);
            newRoomItem.Set(this, roomInfo);
            this.roomItemList.Add(newRoomItem);
        }
    }
}
