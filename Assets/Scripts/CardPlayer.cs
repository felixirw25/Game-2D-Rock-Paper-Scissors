using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Photon.Pun;

public class CardPlayer : MonoBehaviour
{
    [SerializeField] CardNet chosenCard = null;
    public HealthBar healthBar;
    public TMP_Text healthText;
    [SerializeField] private TMP_Text nameText;
    public Transform atkPosRef;
    public PlayerStats stats;
    public float Health;
    public float MaxHealth;
    public TMP_Text Name { get => nameText; }
    public AudioSource audioSource;
    public AudioClip chosenCardClip;
    public AudioClip attackClip;
    public AudioClip damageClip;
    public bool IsReady;
    
    private Tweener animationTweener;
    
    public void Start(){
        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(PropertyNames.Room.MaxHealth, out var maxHealth);
        Health = (float)maxHealth;
        healthText.text = Health.ToString() + " / " + Health.ToString();
    }

    public void SetStats(PlayerStats newStats, bool restoreFullHealth = false){
        this.stats = newStats;
        if(restoreFullHealth)
            Health = stats.MaxHealth;
        
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
    public void SetChosenCard(CardNet newCard){
        if(chosenCard != null){
            chosenCard.transform.DOKill();
            chosenCard.Reset();
        }

        chosenCard = newCard;
        chosenCard.transform.DOScale(chosenCard.transform.localScale*1.2f, 0.2f);
        audioSource.PlayOneShot(chosenCardClip);
    }

    public void ChangeHealth(float amount){
        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(PropertyNames.Room.MaxHealth, out var maxHealth);
        Health += amount;
        Health = Mathf.Clamp(Health,0,(float)maxHealth); //stats.maxhealth

        healthBar.UpdateBar(Health/(float)maxHealth);
        healthText.text = Health + "/" + (float)maxHealth;
    }
    public void UpdateHealthBar(){

    }
    
    // public void AnimateAttack(){
    //     Tweener tweener = chosenCard.transform
    //         .DOMove(atkPosRef.position, 1f)
    //         .SetEase(Ease.Linear);
    // }
    public void AnimateAttack(){
        audioSource.PlayOneShot(attackClip);
        animationTweener = chosenCard.transform
            .DOMove(atkPosRef.position, 0.5f);
    }

    public void AnimateDamage(){
        audioSource.PlayOneShot(damageClip);
        var image = chosenCard.GetComponent<Image>();
        animationTweener = image
            .DOColor(Color.red, 0.1f)
            .SetLoops(3, LoopType.Yoyo)
            .SetDelay(0.2f);
    }

    // public void AnimateDamage(){
    //     audioSource.PlayOneShot(damageClip);
    //     var image = chosenCard.GetComponent<Image>();
    //     animationTweener = image
    //         .DOColor(Color.red, 0.1f)
    //         .SetLoops(3, LoopType.Yoyo)
    //         .SetDelay(1f);
    // }
    // public void AnimateBack(float delay = 2f){
    //     var image = chosenCard.GetComponent<Image>();
    //     animationTweener = image.transform
    //         .DOMove(chosenCard.OriginalPosition, 1f)
    //         .SetEase(Ease.InBack)
    //         .SetDelay(delay);
    // }
    
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
