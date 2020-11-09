using UnityEngine;
using UnityEngine.Serialization;

namespace Buriola.Controller
{
    [System.Serializable]
    public class CharacterStats
    {
        public int Level { get; set; }
        public int Health { get; set; }
        public float Xp { get; set; }

        [FormerlySerializedAs("maxHealth")]
        [Header("General Stats")]
        [Range(3, 5)]
        public int MaxHealth;

        [FormerlySerializedAs("maxXP")] 
        [Range(1, 100)] 
        public int MaxXP;
        
        [FormerlySerializedAs("damage")]
        [Range(1, 3)] 
        public int Damage;
        
        [FormerlySerializedAs("moveSpeed")]
        [Range(2, 10)] 
        public float MoveSpeed;

        [FormerlySerializedAs("fireRate")]
        [Header("Shooting Stats")] 
        [Range(0.15f, 2f)]
        public float FireRate;
    }
}
