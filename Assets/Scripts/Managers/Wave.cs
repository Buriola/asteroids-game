using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class generates a Wave asset.
/// </summary>
[CreateAssetMenu(menuName = "Wave", fileName = "New Wave")]
public class Wave : ScriptableObject
{
    [Header("Wave Enemies")]
    [Range(5, 100)]
    public int numberOfEnemies;
    public List<Enemies> waveEnemies = new List<Enemies>();
    [Space]
    [Header("Wave Time Settings")]
    [Range(.5f, 10)]
    public float startTime;
    [Range(1f, 5f)]
    public float spawnRate;
}

[System.Serializable]
public class Enemies
{
    public string enemyTag;
    [Range(1, 100)]
    public float rate; // The weight or chance of this enemy spawning
}
