using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] Card chosenCard;
    public HealthBar healthBar;
    public TMP_Text healthText;
    public Transform atkPosRef;
    public float Health;
    public float MaxHealth;
    private Tweener animationTweener;
    public void Start(){
        Health = MaxHealth;
    }
    public Attack? AttackValue{
        get => chosenCard == null ? null : chosenCard.AttackValue;
    }
    
    public void Reset(){
        if(chosenCard != null){
            chosenCard.Reset();
        }

        chosenCard = null;
    }
    public void SetChosenCard(Card newCard){
        if(chosenCard != null){
            chosenCard.Reset();
        }

        chosenCard = newCard;
        chosenCard.transform.DOScale(chosenCard.transform.localScale*1.2f, 0.2f);
    }

    public void ChangeHealth(float amount){
        Health += amount;
        Health = Mathf.Clamp(Health,0,100);

        healthBar.UpdateBar(Health/MaxHealth);
        healthText.text = Health + "/" + MaxHealth;
    }
    
    public void AnimateAttack(){
        Tweener tweener = chosenCard.transform
            .DOMove(atkPosRef.position, 1f)
            .SetEase(Ease.Linear);
    }

    public void AnimateDamage(){
        var image = chosenCard.GetComponent<Image>();
        animationTweener = image
            .DOColor(Color.red, 0.1f)
            .SetLoops(3, LoopType.Yoyo)
            .SetDelay(1f);
    }
    public void AnimateBack(float delay = 2f){
        var image = chosenCard.GetComponent<Image>();
        animationTweener = image.transform
            .DOMove(chosenCard.OriginalPosition, 1f)
            .SetEase(Ease.InBack)
            .SetDelay(delay);
    }
    
    public bool IsAnimating(){
        return animationTweener.IsActive();
    }
    public void isClickable(bool value){
        Card[] cards = GetComponentsInChildren<Card>();
        foreach(var card in cards){
            card.SetClickable(value);
        } 
    }
}
