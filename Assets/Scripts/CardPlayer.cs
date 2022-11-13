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
    public PlayerStats stats = new PlayerStats(){
        MaxHealth = 100,
        RestoreValue = 5,
        DamageValue = 10
    };
    public float Health;
    public TMP_Text Name { get => nameText; }
    public AudioSource audioSource;
    public AudioClip chosenCardClip;
    public AudioClip attackClip;
    public AudioClip damageClip;
    public bool Online;
    public bool IsReady = false;
    
    private Tweener animationTweener;
    
    public void Start(){
        if(Online){
            PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(PropertyNames.Room.MaxHealth, out var maxHealth);
            Health = (float)maxHealth;
            healthText.text = Health.ToString() + " / " + Health.ToString();
        }
        Health = stats.MaxHealth;
    }

    public void SetStats(PlayerStats newStats, bool restoreFullHealth = false){
        this.stats = newStats;
        if(restoreFullHealth)
            Health = stats.MaxHealth;
        
        UpdateHealthBar();
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
        if(Online){
            PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(PropertyNames.Room.MaxHealth, out var maxHealth);
            Health += amount;
            Health = Mathf.Clamp(Health,0,(float)maxHealth); 

            healthBar.UpdateBar(Health/(float)maxHealth);
            healthText.text = Health + "/" + (float)maxHealth;
        }
        Health += amount;
        Health = Mathf.Clamp(Health,0,stats.MaxHealth); 
        UpdateHealthBar();
    }
    public void UpdateHealthBar(){
        healthBar.UpdateBar(Health/stats.MaxHealth);
        healthText.text = Health + "/" + stats.MaxHealth;
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
        CardNet[] cards = GetComponentsInChildren<CardNet>();
        foreach(var card in cards){
            card.SetClickable(value);
        } 
    }
}
