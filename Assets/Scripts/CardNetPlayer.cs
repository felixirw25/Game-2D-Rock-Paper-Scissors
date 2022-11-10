using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class CardNetPlayer : MonoBehaviourPun
{
    public static List<CardNetPlayer> NetPlayers = new List<CardNetPlayer>(2);
    // photonView.Owner
    private CardNet[] cards;
    public void Set(CardPlayer player){
        player.Name.text = photonView.Owner.NickName;
        cards = player.GetComponentsInChildren<CardNet>();
        foreach(var card in cards){
            var button = card.GetComponent<Button>();
            button.onClick.AddListener(()=>RemoteClickButton(card.AttackValue));

            if(photonView.IsMine==false)
                button.interactable = false;
        }
    }
    private void OnDestroy(){
        foreach(var card in cards){
            var button = card.GetComponent<Button>();
            button.onClick.RemoveListener(()=>RemoteClickButton(card.AttackValue));
        }
    }
    private void RemoteClickButton(Attack value){
        if(photonView.IsMine){
            photonView.RPC("RemoteClickButtonRPC", RpcTarget.Others, (int)value);
        }
    }
    [PunRPC]
    private void RemoteClickButtonRPC(int value){
        foreach(var card in cards){
            if(card.AttackValue == (Attack) value){
                var button = card.GetComponent<Button>();
                button.onClick.Invoke();
                break;
            }
        }
    }
    private void OnEnable(){
        NetPlayers.Add(this);
    }
    private void OnDisable(){
        NetPlayers.Remove(this);
    }
}
