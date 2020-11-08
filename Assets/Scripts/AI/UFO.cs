using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asteroids.Controller;

namespace Asteroids.AI
{
    /// <summary>
    /// This class contains the behavior of an UFO enemy.
    /// </summary>
    public class UFO : MonoBehaviour
    {
        #region Variables
        public EnemyStats stats;

        [Header("Audio Settings")]
        public AudioClip deathSfx;

        private StateManager st;

        private Rigidbody2D rb;
        private bool tookDamage;
        private float totalTimer = 0;
        private float knockbackDuration = 1f;
        #endregion

        #region Unity Functions
        private void OnEnable()
        {
            tookDamage = false;
            stats.health = stats.maxHealth;

            rb = GetComponent<Rigidbody2D>();
            rb.drag = 4;
            rb.angularDrag = 999;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 0;

            st = GameObject.FindGameObjectWithTag("Player").GetComponent<StateManager>();
        }

        private void FixedUpdate()
        {
            if (tookDamage)
            {
                Knockback();
            }
            HandleMovement();
        }

        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (otherCollider.CompareTag("Player"))
            {
                st.TakeDamage(stats.damage);
                gameObject.SetActive(false);
            }
        }

        #endregion

        #region My Functions
        private void HandleMovement()
        { 
            transform.position = Vector3.MoveTowards(transform.position, st.transform.position, stats.moveSpeed * Time.deltaTime);
        }

        /// <summary>
        /// Called once this enemy takes damage
        /// </summary>
        /// <param name="damage">Damage to be applied on enemy</param>
        public void TakeDamage(int damage)
        {
            if (stats.health <= 1)
            {
                st.stats.xp += stats.xpValue;
                gameObject.SetActive(false);
                LevelController.instance.playerScore += stats.scoreValue;
                LevelController.instance.enemiesKilled++;
                if (deathSfx != null)
                    AudioHandler.PlaySFX(deathSfx);
                CancelInvoke();
            }
            else
            {
                stats.health -= damage;
                tookDamage = true;
            }
        }

        /// <summary>
        /// Handles the enemy knockback after being hit by the player
        /// </summary>
        private void Knockback()
        {
            //Using a timer so the enemy cant move or shoot until the knockback is over
            totalTimer += Time.deltaTime;
            if (totalTimer >= knockbackDuration)
            {
                totalTimer = 0;
                tookDamage = false;
            }

            //Knockback
            rb.velocity = new Vector2(-transform.up.x * 2f, -transform.up.y * 2f);
        }
        #endregion
    }
}