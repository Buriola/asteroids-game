using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyStats
{
    [HideInInspector]
    public float health;
    [Range(1f, 20f)]
    public float xpValue;
    [Range(100, 300)]
    public int scoreValue;

    [Header("General Stats")]
    [Range(1, 100)]
    public int maxHealth;
    [Range(1, 3)]
    public int damage;
    [Range(0, 10)]
    public float moveSpeed;

    [Header("Shooting Stats")]
    [Range(0.15f, 2f)]
    public float fireRate;
}
