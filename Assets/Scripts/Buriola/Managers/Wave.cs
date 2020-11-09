using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Buriola.Managers
{
    [CreateAssetMenu(menuName = "Wave", fileName = "New Wave")]
    public class Wave : ScriptableObject
    {
        [FormerlySerializedAs("numberOfEnemies")] 
        [Header("Wave Enemies")] 
        [Range(5, 100)]
        public int NumberOfEnemies;

        [FormerlySerializedAs("waveEnemies")] 
        public List<Enemies> WaveEnemies = new List<Enemies>();

        [FormerlySerializedAs("startTime")]
        [Space] 
        [Header("Wave Time Settings")] 
        [Range(.5f, 10)]
        public float StartTime;

        [FormerlySerializedAs("spawnRate")] 
        [Range(1f, 5f)] 
        public float SpawnRate;
    }

    [System.Serializable]
    public class Enemies
    {
        [FormerlySerializedAs("enemyTag")] 
        public string EnemyTag;
        [FormerlySerializedAs("rate")] [Range(1, 100)] 
        public float Rate;
    }
}
