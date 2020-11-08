using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStats
{
    [HideInInspector]
    public int level;
    [HideInInspector]
    public int health;
    [HideInInspector]
    public float xp;

    [Header("General Stats")]
    [Range(3, 5)]
    public int maxHealth;
    [Range(1, 100)]
    public int maxXP;
    [Range(1, 3)]
    public int damage;
    [Range(2, 10)]
    public float moveSpeed;

    [Header("Shooting Stats")]
    [Range(0.15f, 2f)]
    public float fireRate;
}
