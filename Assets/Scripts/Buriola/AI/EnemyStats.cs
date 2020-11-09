using UnityEngine;
using UnityEngine.Serialization;

namespace Buriola.AI
{
    [System.Serializable]
    public class EnemyStats
    {
        public float Health { get; set; }

        [FormerlySerializedAs("xpValue")] [Range(1f, 20f)]
        public float XpValue;

        [FormerlySerializedAs("scoreValue")] [Range(100, 300)]
        public int ScoreValue;

        [FormerlySerializedAs("maxHealth")] [Header("General Stats")] [Range(1, 100)]
        public int MaxHealth;

        [FormerlySerializedAs("damage")] [Range(1, 3)]
        public int Damage;

        [FormerlySerializedAs("moveSpeed")] [Range(0, 10)]
        public float MoveSpeed;

        [FormerlySerializedAs("fireRate")] [Header("Shooting Stats")] [Range(0.15f, 2f)]
        public float FireRate;
    }
}
