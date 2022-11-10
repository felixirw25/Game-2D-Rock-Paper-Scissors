using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Attack AttackValue;
    public PlayerOffline player;
    public Transform atkPosRef;
    public Vector2 OriginalPosition;
    public Vector2 OriginalScale;
    Color originalColor;
    bool isClickable = true;

    private void Start(){
        OriginalPosition = this.transform.position;
        OriginalScale = this.transform.localScale;
        originalColor = GetComponent<Image>().color;

        // transform.DOMove(atkPosRef.position, 5);
        // transform.DOMove(atkPosRef.position, 5).SetLoops(-1, LoopType.Yoyo);

        // var seq = DOTween.Sequence();
        // seq.Append(transform.DOMove(atkPosRef.position, 1));
        // seq.Append(transform.DOMove(startPosition, 1));
    }

    public void onClick()
    {
        if(isClickable){
            OriginalPosition = this.transform.position;
            player.SetChosenCard(this);
        }
    }

    internal void Reset()
    {
        transform.position = OriginalPosition;
        transform.localScale = OriginalScale;
        GetComponent<Image>().color = originalColor;
    }

    public void SetClickable(bool value){
        isClickable = value;
    }

    // float timer=0;
    // private void Update(){
    //     if(timer<=1){
    //         var newX= Linear(startPosition.x, atkPosRef.position.x, timer);
    //         var newY= Linear(startPosition.y, atkPosRef.position.y, timer);
    //         this.transform.position = new Vector2(newX, newY);
    //         timer += Time.deltaTime;
    //     }
    //     else{
    //         // this.transform.position = atkPosRef.position;
    //         timer = 0;
    //     }
    // }
    // private float Linear(float start, float end, float time){
    //     return (end-start)*time + start;
    // }
}
