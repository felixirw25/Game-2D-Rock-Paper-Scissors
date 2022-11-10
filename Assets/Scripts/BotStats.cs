using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // Dikenalkan ke compiler dapat dikenali
public struct BotStats
{
    public string Name;
    public float MaxHealth;
    public float RestoreValue;
    public float DamageValue;
    public float ChoosingInterval;

}
