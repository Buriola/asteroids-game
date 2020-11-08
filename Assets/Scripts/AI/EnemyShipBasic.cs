using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asteroids.Controller;

namespace Asteroids.AI
{
    /// <summary>
    /// This class contains the behaviors of an enemy space ship
    /// </summary>
    public class EnemyShipBasic : MonoBehaviour
    {
        #region Variables
        public EnemyStats stats;

        [Range(3, 10)]  public float safeDistance;

        [Header("Audio Settings")]
        public AudioClip shotSfx;
        public AudioClip deathSfx;

        private StateManager st;
        private Rigidbody2D rb;

        private Vector3 moveDir;
        private float distanceToPlayer;
        private bool tookDamage;
        private bool canMove;
        private int frames;
        private float totalTimer = 0;
        private float knockbackDuration = 1f;

        #endregion

        #region Unity Functions
        private void OnEnable()
        {
            canMove = true;
            tookDamage = false;
            stats.health = stats.maxHealth;
            frames = 0;
            rb = GetComponent<Rigidbody2D>();
            rb.drag = 4;
            rb.angularDrag = 999;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            st = GameObject.FindGameObjectWithTag("Player").GetComponent<StateManager>();
            InvokeRepeating("HandleShooting", 3f, stats.fireRate);
        }
        
        private void FixedUpdate()
        {
            frames++;

            if(tookDamage)
            {
                Knockback();
            }
            HandleMovement();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            //Handles collision with player and apply  damage
            if (collider.tag == "Player")
            {
                st.TakeDamage(stats.damage);
                gameObject.SetActive(false);
                CancelInvoke(); // Cancels the InvokeRepeating that runs OnEnable
            }
        }

        private void OnDisable()
        {
            CancelInvoke();
        }
        #endregion

        #region My Functions
        /// <summary>
        /// This method handles the enemy ship movement and dashing
        /// </summary>
        private void HandleMovement()
        {
            if (!canMove) return;

            distanceToPlayer = Vector2.Distance(st.transform.position, transform.position);

            //Will move to a safe distance from the player
            if (distanceToPlayer > safeDistance)
            {
                moveDir = st.transform.position - transform.position;
                rb.velocity = moveDir * stats.moveSpeed;
            }
            else
            {
                //Once it found a safe position it will start dashing randomly every 80 frames
                if(frames > 80)
                {
                    moveDir = Random.insideUnitCircle * 10f; // Random dash
                    frames = 0;
                }
                rb.velocity = moveDir * (stats.moveSpeed + .2f);
            }

            //Look to player
            transform.rotation = Quaternion.LookRotation(Vector3.forward, st.transform.position - transform.position);

        }

        /// <summary>
        /// Handles the enemy shooting
        /// </summary>
        private void HandleShooting()
        {
            if (!canMove || tookDamage) return;

            float distance = Vector2.Distance(transform.position, st.transform.position);

            //Only start shooting after a certain distance so the player can know where the enemy is coming from
            if(distance < 7)
            {
                GameObject bullet = ObjectPooler.Instance.GetEnemyProjectileObject(); // Get from the pool
                if (bullet != null)
                {
                    //Change this to turrets positions
                    bullet.transform.position = gameObject.transform.GetChild(0).position; // Get the turret position on the object
                    bullet.transform.rotation = gameObject.transform.GetChild(0).rotation;
                    bullet.GetComponent<Projectile>().moveDir = gameObject.transform.GetChild(0).up;
                    bullet.GetComponent<Projectile>().damage = stats.damage; //Passes the desired damage to the projectile
                    bullet.SetActive(true);
                    if(shotSfx != null)
                        AudioHandler.PlaySFX(shotSfx);
                }
            }
            
        }

        /// <summary>
        /// Called once this enemy takes damage
        /// </summary>
        /// <param name="damage">Damage to be applied on enemy</param>
        public void TakeDamage(int damage)
        {
            if(stats.health <= 1)
            {
                st.stats.xp += stats.xpValue;
                gameObject.SetActive(false);
                LevelController.instance.playerScore += stats.scoreValue;
                LevelController.instance.enemiesKilled++;
                if(deathSfx != null)
                    AudioHandler.PlaySFX(deathSfx);
                CancelInvoke();
            }
            else
            {
                stats.health -= damage;
                tookDamage = true;
                canMove = false;
            }
        }

        /// <summary>
        /// Handles the enemy knockback after being hit by the player
        /// </summary>
        private void Knockback()
        { 
            //Using a timer so the enemy cant move or shoot until the knockback is over
            totalTimer += Time.deltaTime;
            if(totalTimer >= knockbackDuration)
            {
                totalTimer = 0;
                tookDamage = false;
                canMove = true;
            }

            //Knockback
            rb.velocity = new Vector2(-transform.up.x * 2f, -transform.up.y * 2f);
        }

        #endregion
    }
}