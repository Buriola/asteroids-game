using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asteroids.Controller
{
    /// <summary>
    /// This is the controller of the player. Its getting updated from the Input Handler.
    /// </summary>
    [RequireComponent(typeof(InputHandler))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class StateManager : MonoBehaviour
    {
        #region Variables
        [Tooltip("Check this you want to control the player without adding physics force")]
        public bool notUseForce;
        public CharacterStats stats;

        [Space]

        [Header("Audio Settings")]
        public AudioClip shotSFX;
        public AudioClip impactSound;
        public AudioClip deathSound;
        public AudioClip levelUpSound;

        [Header("States (Debugging only)")]
        public bool canMove;
        public bool isMoving;
        public bool isShooting;
        public bool tookDamage;
        public bool isDead;
        public bool startBlinking;

        [HideInInspector]
        public float horizontal;
        [HideInInspector]
        public float vertical;
        [HideInInspector]
        public float fire;

        private Rigidbody2D rb;
        private Vector3 moveDir;
        private float moveAmount;
        private float delta;
        private float nextFire;

        private float spriteBlinkingTotalTimer;
        private float spriteBlinkingTotalDuration;
        private float spriteBlinkingTimer;
        private float spriteBlinkingMiniDuration;

        public enum Turrets { Single, Double, Triple }
        public Turrets turretCount;

        #endregion

        #region My Functions
        /// <summary>
        /// This method simulates a Start. Since this script depends of the InputHandler, this will be called in the
        /// Start of the InputHandler.
        /// </summary>
        public void Init()
        {
            canMove = true;
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.angularDrag = 999;
           
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            stats.health = stats.maxHealth;
            stats.level = 1;

            spriteBlinkingMiniDuration = .1f;
            spriteBlinkingTotalDuration = 1.2f;
            spriteBlinkingTimer = 0f;
            spriteBlinkingTotalTimer = 0f;

            turretCount = Turrets.Single;
        }

        /// <summary>
        /// This method simulates a Update. Since this script depends of the InputHandler, this will be called in the
        /// Update of the InputHandler.
        /// </summary>
        /// <param name="delta"></param>
        public void Tick(float delta)
        {
            this.delta = delta;
            if (isDead) return; 

            LevelUp();

            if(stats.health <= 0)
            {
                isDead = true;
                stats.health = 0;

                if(deathSound != null)
                    AudioHandler.PlaySFX(deathSound);
                return;
            }

            if (fire > 0.1f && Time.time > nextFire && !tookDamage && !isDead)
                HandleShooting();
            else
                isShooting = false;
        }

        /// <summary>
        /// This method simulates a FixedUpdate. Since this script depends of the InputHandler, this will be called in the
        /// FixedUpdate of the InputHandler
        /// </summary>
        /// <param name="delta">The current time frame. Ensures that this is happening at the same time.</param>
        public void FixedTick(float delta)
        {
            this.delta = delta;

            if (tookDamage && startBlinking)
            {
                canMove = false;
                HandleDamageTaken();
            }

            if (isDead) return;
            HandleMovement();
            ClampObjectInView();
        }

        /// <summary>
        /// Takes care of the player movement
        /// </summary>
        void HandleMovement()
        {
            if (!canMove)
            {
                isMoving = false;
                return;
            }

            Vector3 v = vertical * Camera.main.transform.up;
            Vector3 h = horizontal * Camera.main.transform.right;
            moveDir = (v + h).normalized;
            float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            moveAmount = Mathf.Clamp01(m);

            float targetSpeed = stats.moveSpeed;

            if(!notUseForce)
            {
                rb.drag = 0;
                rb.AddForce(moveDir * targetSpeed, ForceMode2D.Force);
            }
            else
            {
                rb.drag = 4;
                rb.velocity = moveDir * (targetSpeed * moveAmount);
            }
                

            //Rotates the sprite towards the mouse position
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);

            isMoving = (moveAmount > 0) ? true : false;
        }

        /// <summary>
        /// This will keep the player inside the camera's viewport
        /// </summary>
        void ClampObjectInView()
        {
            Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
            pos.x = Mathf.Clamp01(pos.x);
            pos.y = Mathf.Clamp01(pos.y);
            transform.position = Camera.main.ViewportToWorldPoint(pos);
        }

        /// <summary>
        /// Function to spawn projectiles from the player
        /// Also handling object polling
        /// </summary>
        void HandleShooting()
        {
            isShooting = true;
            nextFire = Time.time + stats.fireRate; // Handles fire rate
            ChooseTurrets(turretCount); //Choose the amount of turrets active at the moment
        }

        /// <summary>
        /// This will handle the knockback when the player takes damage
        /// Also giving him some invicible frames during a small time
        /// </summary>
        void HandleDamageTaken()
        {

            //Starts a blinking effect using timers
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            spriteBlinkingTotalTimer += Time.deltaTime;

            if (spriteBlinkingTotalTimer >= spriteBlinkingTotalDuration)
            {
                startBlinking = false;
                spriteBlinkingTotalTimer = 0.0f;
                sr.enabled = true;

                tookDamage = false;
                canMove = true;
                return;
            }

            spriteBlinkingTimer += Time.deltaTime;
            if (spriteBlinkingTimer >= spriteBlinkingMiniDuration)
            {
                spriteBlinkingTimer = 0.0f;
                if (sr.enabled == true)
                {
                    sr.enabled = false; 
                }
                else
                {
                    sr.enabled = true; 
                }
            }

            CameraShake.ShakeCamera(.1f, .02f);

            //Knockback
            rb.velocity = new Vector2(-transform.up.x * 2f, -transform.up.y * 2f);

        }

        /// <summary>
        /// Public function called by the enemy projectile when it hits the player. 
        /// </summary>
        /// <param name="damage">Damage of the projectile that the player will receive.</param>
        public void TakeDamage(int damage)
        {
            if (isDead || tookDamage) return;

            if (stats.health > 0)
                stats.health -= damage;

            tookDamage = true;
            startBlinking = true;

            if (impactSound != null)
                AudioHandler.PlaySFX(impactSound);
        }

        /// <summary>
        /// Check if the player leveled up
        /// </summary>
        void LevelUp()
        {
            if(stats.xp >= stats.maxXP)
            {
                stats.level++;
                stats.xp = 0;

                if (stats.maxHealth < 5)
                    stats.maxHealth++; //Receive more maxHP

                stats.health = stats.maxHealth;
                stats.maxXP += 50; // increase required xp to level up again

                //Handles the amount of turrets being used
                if ((int)turretCount < 2)
                    turretCount++;
                else turretCount = 0;

                if(levelUpSound != null)
                    AudioHandler.PlaySFX(levelUpSound);
            }
        }

        /// <summary>
        /// This methos will handle the weapon transitions of the player
        /// </summary>
        /// <param name="turrets"></param>
        void ChooseTurrets(Turrets turrets)
        {
            switch (turrets)
            {
                case Turrets.Single:
                    stats.fireRate = .2f;
                    GameObject bullet = ObjectPooler.Instance.GetPlayerProjectileObject();
                    if (bullet != null)
                    {
                        //Getting the child 0 - Turret 0 - from the transform and spawning a bullet from the pool
                        bullet.transform.position = gameObject.transform.GetChild(0).position;
                        bullet.transform.rotation = gameObject.transform.GetChild(0).rotation;
                        bullet.GetComponent<Projectile>().moveDir = gameObject.transform.GetChild(0).up;
                        bullet.GetComponent<Projectile>().damage = stats.damage;
                        bullet.SetActive(true);
                    }
                    break;
                case Turrets.Double:
                    stats.fireRate = .3f;
                    int amount = 2;
                    GameObject[] bullets = new GameObject[2];
                    for (int i = 0; i < amount; i++)
                    {
                        bullets[i] = ObjectPooler.Instance.GetPlayerProjectileObject();
                        if(bullets[i] != null && i == 0)
                        {
                            bullets[i].transform.position = gameObject.transform.GetChild(1).position;
                            bullets[i].transform.rotation = gameObject.transform.GetChild(1).rotation;
                            bullets[i].GetComponent<Projectile>().moveDir = gameObject.transform.GetChild(0).up;
                            bullets[i].GetComponent<Projectile>().damage = stats.damage;
                            bullets[i].SetActive(true);
                        }
                        else if(bullets[i] != null && i == 1)
                        {
                            bullets[i].transform.position = gameObject.transform.GetChild(2).position;
                            bullets[i].transform.rotation = gameObject.transform.GetChild(2).rotation;
                            bullets[i].GetComponent<Projectile>().moveDir = gameObject.transform.GetChild(0).up;
                            bullets[i].GetComponent<Projectile>().damage = stats.damage;
                            bullets[i].SetActive(true);
                        }
                    }
                    break;
                case Turrets.Triple:
                    stats.fireRate = .35f;
                    int amount2 = 3;
                    GameObject[] bullets2 = new GameObject[3];
                    for (int i = 0; i < amount2; i++)
                    {
                        bullets2[i] = ObjectPooler.Instance.GetPlayerProjectileObject();
                        if (bullets2[i] != null && i == 0)
                        {
                            bullets2[i].transform.position = gameObject.transform.GetChild(1).position;
                            bullets2[i].transform.rotation = gameObject.transform.GetChild(1).rotation;
                            bullets2[i].GetComponent<Projectile>().moveDir = gameObject.transform.GetChild(0).up;
                            bullets2[i].GetComponent<Projectile>().damage = stats.damage;
                            bullets2[i].SetActive(true);
                        }
                        else if (bullets2[i] != null && i == 1)
                        {
                            bullets2[i].transform.position = gameObject.transform.GetChild(2).position;
                            bullets2[i].transform.rotation = gameObject.transform.GetChild(2).rotation;
                            bullets2[i].GetComponent<Projectile>().moveDir = gameObject.transform.GetChild(0).up;
                            bullets2[i].GetComponent<Projectile>().damage = stats.damage;
                            bullets2[i].SetActive(true);
                        }
                        else if(bullets2[i] != null && i == 2)
                        {
                            bullets2[i].transform.position = gameObject.transform.GetChild(0).position;
                            bullets2[i].transform.rotation = gameObject.transform.GetChild(0).rotation;
                            bullets2[i].GetComponent<Projectile>().moveDir = gameObject.transform.GetChild(0).up;
                            bullets2[i].GetComponent<Projectile>().damage = stats.damage;
                            bullets2[i].SetActive(true);
                        }
                    }
                    break;
                default:
                    break;
            }

            if(shotSFX != null)
                AudioHandler.PlaySFX(shotSFX);
        }

        #endregion
    }
}
