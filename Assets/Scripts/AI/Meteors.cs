using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asteroids.Controller;

namespace Asteroids.AI
{
    /// <summary>
    /// This class contains the behavior of an asteoid
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Meteors : MonoBehaviour
    {
        #region Variables   
        [Header("Meteor Stats")]
        public EnemyStats stats;
        [Range(0, 1f)]
        public float force;

        [Tooltip("Spawn minor meteors on death")]
        public GameObject[] minorMeteors;
        [Range(0, 5)] public int amount;

        [Header("Meteor Settings")]
        public float rotationSpeed;
        public float rotationTime;

        [Header("Audio Settings")]
        public AudioClip impactSfx;
        public AudioClip deathSound;

        private bool destroyedByPlayer;
        private Rigidbody2D rb;
        private GameObject player;
        private Vector3 moveDir;
        #endregion

        #region Unity Functions
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnEnable()
        {
            stats.health = stats.maxHealth;

            destroyedByPlayer = false;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            Invoke("ChangeRotation", rotationTime);

            if (player != null)
            {
                //After its enabled, it will take the last player position and apply a force in that direction
                Vector3 moveDir = player.transform.position - transform.position;
                rb.AddForce(moveDir * force, ForceMode2D.Impulse);
            }
        }

        private void FixedUpdate()
        {
            transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
        }

        private void OnDisable()
        {
            //If the object was disable or "destroyed" by the player it will call the method to spawn more meteors
            if (destroyedByPlayer)
            {
                SpawnMinorMeteors();
                if (deathSound != null) AudioHandler.PlaySFX(deathSound);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            //Handles collision with the player
            if (collider.tag == "Player")
            {
                StateManager st = collider.gameObject.GetComponent<StateManager>();
                st.TakeDamage(stats.damage);
                if (impactSfx != null) AudioHandler.PlaySFX(impactSfx);
                gameObject.SetActive(false);
            }
        }
        #endregion

        #region My Functions
        /// <summary>
        /// A helper method to make the meteors rotate randomly
        /// </summary>
        void ChangeRotation()
        {
            if (Random.value > 0.5f)
            {
                rotationSpeed = -rotationSpeed;
            }

            Invoke("ChangeRotation", rotationTime);
        }

        /// <summary>
        /// This will spawn minor meteors after a big one is destroyed
        /// </summary>
        void SpawnMinorMeteors()
        {
            if (minorMeteors.Length == 0) return;

            for (int i = 0; i < amount; i++)
            {
                //Searching in the pool with a tag
                GameObject meteor = ObjectPooler.Instance.GetEnemyObject("M_Meteor");
                //Giving it a random position 
                meteor.transform.position = new Vector3(transform.position.x + (Random.Range(-2, 2)),
                    transform.position.y + (Random.Range(-2, 2)), 0f);
                meteor.SetActive(true);
            }
        }

        /// <summary>
        /// Called once this enemy takes damage
        /// </summary>
        /// <param name="damage">Damage to be applied on enemy</param>
        public void TakeDamage(int damage)
        {
            if (stats.health <= 1)
            {
                destroyedByPlayer = true;
                player.GetComponent<StateManager>().stats.xp += stats.xpValue;
                gameObject.SetActive(false);
                LevelController.instance.playerScore += stats.scoreValue;
                LevelController.instance.enemiesKilled++;
            }
            else
            {
                stats.health -= damage;
            }
        }

        #endregion
    }
}
