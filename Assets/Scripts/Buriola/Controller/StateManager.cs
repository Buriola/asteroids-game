using System;
using Buriola.Audio;
using Buriola.CameraUtils;
using Buriola.Managers;
using Buriola.Projectiles;
using UnityEngine;
using UnityEngine.Serialization;

namespace Buriola.Controller
{
    [RequireComponent(typeof(InputHandler))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class StateManager : MonoBehaviour
    {
        #region Variables
        [Tooltip("Check this you want to control the player without adding physics force")]
        [SerializeField]
        private bool _notUseForce = false;

        public CharacterStats Stats;

        [FormerlySerializedAs("shotSFX")]
        [Space]

        [Header("Audio Settings")]
        [SerializeField]
        private AudioClip _shotSFX = null;
        
        [FormerlySerializedAs("impactSound")] 
        [SerializeField]
        private AudioClip _impactSound = null;
        
        [FormerlySerializedAs("deathSound")] 
        [SerializeField]
        private AudioClip _deathSound = null;
        
        [FormerlySerializedAs("levelUpSound")] 
        [SerializeField]
        private AudioClip _levelUpSound = null;

        [FormerlySerializedAs("canMove")]
        [Header("States (Debugging only)")]
        [SerializeField]
        private bool _canMove = false;

        [FormerlySerializedAs("tookDamage")] 
        [SerializeField]
        private bool _tookDamage = false;
        
        public bool IsDead { get; private set; }
        
        [FormerlySerializedAs("startBlinking")]
        [SerializeField]
        private bool _startBlinking = false;
        
        public float Horizontal { get; set; }
        public float Vertical { get; set; }
        public float Fire { get; set; }

        private Rigidbody2D _rigidbody2D;
        private Vector3 _moveDirection;
        private float _moveAmount;
        private float _nextFire;

        private float _spriteBlinkingTotalTimer;
        private float _spriteBlinkingTotalDuration;
        private float _spriteBlinkingTimer;
        private float _spriteBlinkingMiniDuration;

        private enum Turrets { Single, Double, Triple }
        
        [FormerlySerializedAs("turretCount")] 
        [SerializeField]
        private Turrets _turretCount = default;

        #endregion

        #region My Functions
        public void Init()
        {
            _canMove = true;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.gravityScale = 0;
            _rigidbody2D.angularDrag = 999;
           
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;

            Stats.Health = Stats.MaxHealth;
            Stats.Level = 1;

            _spriteBlinkingMiniDuration = .1f;
            _spriteBlinkingTotalDuration = 1.2f;
            _spriteBlinkingTimer = 0f;
            _spriteBlinkingTotalTimer = 0f;

            _turretCount = Turrets.Single;
        }
        
        public void Tick(float delta)
        {
            if (IsDead) return; 

            LevelUp();

            if(Stats.Health <= 0)
            {
                IsDead = true;
                Stats.Health = 0;

                if(_deathSound != null)
                    AudioHandler.PlaySFX(_deathSound);
                return;
            }

            if (Fire > 0.1f && Time.time > _nextFire && !_tookDamage && !IsDead)
                HandleShooting();
            else
            {
            }
        }
        
        public void FixedTick(float delta)
        {
            if (_tookDamage && _startBlinking)
            {
                _canMove = false;
                HandleDamageTaken();
            }

            if (IsDead) return;
            HandleMovement();
            ClampObjectInView();
        }
        
        private void HandleMovement()
        {
            if (!_canMove)
            {
                return;
            }

            Vector3 v = Vertical * Camera.main.transform.up;
            Vector3 h = Horizontal * Camera.main.transform.right;
            _moveDirection = (v + h).normalized;
            float m = Mathf.Abs(Horizontal) + Mathf.Abs(Vertical);
            _moveAmount = Mathf.Clamp01(m);

            float targetSpeed = Stats.MoveSpeed;

            if(!_notUseForce)
            {
                _rigidbody2D.drag = 0;
                _rigidbody2D.AddForce(_moveDirection * targetSpeed, ForceMode2D.Force);
            }
            else
            {
                _rigidbody2D.drag = 4;
                _rigidbody2D.velocity = _moveDirection * (targetSpeed * _moveAmount);
            }

            //Rotates the sprite towards the mouse position
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
        }
        
        private void ClampObjectInView()
        {
            Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
            pos.x = Mathf.Clamp01(pos.x);
            pos.y = Mathf.Clamp01(pos.y);
            transform.position = Camera.main.ViewportToWorldPoint(pos);
        }
        
        private void HandleShooting()
        {
            _nextFire = Time.time + Stats.FireRate;
            ChooseTurrets(_turretCount);
        }
        
        private void HandleDamageTaken()
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            _spriteBlinkingTotalTimer += Time.deltaTime;

            if (_spriteBlinkingTotalTimer >= _spriteBlinkingTotalDuration)
            {
                _startBlinking = false;
                _spriteBlinkingTotalTimer = 0.0f;
                sr.enabled = true;

                _tookDamage = false;
                _canMove = true;
                return;
            }

            _spriteBlinkingTimer += Time.deltaTime;
            if (_spriteBlinkingTimer >= _spriteBlinkingMiniDuration)
            {
                _spriteBlinkingTimer = 0.0f;
                sr.enabled = sr.enabled != true;
            }

            CameraShake.ShakeCamera(.1f, .02f);
            
            _rigidbody2D.velocity = new Vector2(-transform.up.x * 2f, -transform.up.y * 2f);
        }
        
        public void TakeDamage(int damage)
        {
            if (IsDead || _tookDamage) return;

            if (Stats.Health > 0)
                Stats.Health -= damage;

            _tookDamage = true;
            _startBlinking = true;

            if (_impactSound != null)
                AudioHandler.PlaySFX(_impactSound);
        }
        
        private void LevelUp()
        {
            if(Stats.Xp >= Stats.MaxXP)
            {
                Stats.Level++;
                Stats.Xp = 0;

                if (Stats.MaxHealth < 5)
                    Stats.MaxHealth++;

                Stats.Health = Stats.MaxHealth;
                Stats.MaxHealth += 50;
                
                if ((int)_turretCount < 2)
                    _turretCount++;
                else _turretCount = 0;

                if(_levelUpSound != null)
                    AudioHandler.PlaySFX(_levelUpSound);
            }
        }
        
        private void ChooseTurrets(Turrets turrets)
        {
            switch (turrets)
            {
                case Turrets.Single:
                    Stats.FireRate = .2f;
                    GameObject bullet = ObjectPooler.Instance.GetPlayerProjectileObject();
                    if (bullet != null)
                    {
                        //Getting the child 0 - Turret 0 - from the transform and spawning a bullet from the pool
                        bullet.transform.position = gameObject.transform.GetChild(0).position;
                        bullet.transform.rotation = gameObject.transform.GetChild(0).rotation;
                        bullet.GetComponent<Projectile>().MoveDirection = gameObject.transform.GetChild(0).up;
                        bullet.GetComponent<Projectile>().Damage = Stats.Damage;
                        bullet.SetActive(true);
                    }
                    break;
                case Turrets.Double:
                    Stats.FireRate = .3f;
                    int amount = 2;
                    GameObject[] bullets = new GameObject[2];
                    for (int i = 0; i < amount; i++)
                    {
                        bullets[i] = ObjectPooler.Instance.GetPlayerProjectileObject();
                        if(bullets[i] != null && i == 0)
                        {
                            bullets[i].transform.position = gameObject.transform.GetChild(1).position;
                            bullets[i].transform.rotation = gameObject.transform.GetChild(1).rotation;
                            bullets[i].GetComponent<Projectile>().MoveDirection = gameObject.transform.GetChild(0).up;
                            bullets[i].GetComponent<Projectile>().Damage = Stats.Damage;
                            bullets[i].SetActive(true);
                        }
                        else if(bullets[i] != null && i == 1)
                        {
                            bullets[i].transform.position = gameObject.transform.GetChild(2).position;
                            bullets[i].transform.rotation = gameObject.transform.GetChild(2).rotation;
                            bullets[i].GetComponent<Projectile>().MoveDirection = gameObject.transform.GetChild(0).up;
                            bullets[i].GetComponent<Projectile>().Damage = Stats.Damage;
                            bullets[i].SetActive(true);
                        }
                    }
                    break;
                case Turrets.Triple:
                    Stats.FireRate = .35f;
                    int amount2 = 3;
                    GameObject[] bullets2 = new GameObject[3];
                    for (int i = 0; i < amount2; i++)
                    {
                        bullets2[i] = ObjectPooler.Instance.GetPlayerProjectileObject();
                        if (bullets2[i] != null && i == 0)
                        {
                            bullets2[i].transform.position = gameObject.transform.GetChild(1).position;
                            bullets2[i].transform.rotation = gameObject.transform.GetChild(1).rotation;
                            bullets2[i].GetComponent<Projectile>().MoveDirection = gameObject.transform.GetChild(0).up;
                            bullets2[i].GetComponent<Projectile>().Damage = Stats.Damage;
                            bullets2[i].SetActive(true);
                        }
                        else if (bullets2[i] != null && i == 1)
                        {
                            bullets2[i].transform.position = gameObject.transform.GetChild(2).position;
                            bullets2[i].transform.rotation = gameObject.transform.GetChild(2).rotation;
                            bullets2[i].GetComponent<Projectile>().MoveDirection = gameObject.transform.GetChild(0).up;
                            bullets2[i].GetComponent<Projectile>().Damage = Stats.Damage;
                            bullets2[i].SetActive(true);
                        }
                        else if(bullets2[i] != null && i == 2)
                        {
                            bullets2[i].transform.position = gameObject.transform.GetChild(0).position;
                            bullets2[i].transform.rotation = gameObject.transform.GetChild(0).rotation;
                            bullets2[i].GetComponent<Projectile>().MoveDirection = gameObject.transform.GetChild(0).up;
                            bullets2[i].GetComponent<Projectile>().Damage = Stats.Damage;
                            bullets2[i].SetActive(true);
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(turrets), turrets, null);
            }

            if(_shotSFX != null)
                AudioHandler.PlaySFX(_shotSFX);
        }

        #endregion
    }
}
