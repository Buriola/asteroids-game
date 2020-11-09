using Buriola.Audio;
using Buriola.Controller;
using Buriola.Managers;
using Buriola.Projectiles;
using UnityEngine;
using UnityEngine.Serialization;

namespace Buriola.AI
{
    public class EnemyShipBasic : MonoBehaviour
    {
        #region Variables
        [FormerlySerializedAs("stats")]
        [SerializeField]
        private EnemyStats _stats = null;

        [FormerlySerializedAs("safeDistance")]
        [SerializeField, Range(3, 10)] 
        private float _safeDistance = 3f;

        [FormerlySerializedAs("shotSfx")]
        [Header("Audio Settings")]
        [SerializeField]
        private AudioClip _shotSfx = null;
        [FormerlySerializedAs("deathSfx")] 
        [SerializeField]
        private AudioClip _deathSfx = null;

        private StateManager _stateManager;
        private Rigidbody2D _rigidbody2D;

        private Vector3 _moveDirection;
        private float _distanceToPlayer;
        private bool _tookDamage;
        private bool _canMove;
        private int _frames;
        private float _totalTimer;
        private const float KNOCKBACK_DURATION = 1f;

        #endregion

        #region Unity Functions
        private void OnEnable()
        {
            _canMove = true;
            _tookDamage = false;
            _stats.Health = _stats.MaxHealth;
            _frames = 0;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.drag = 4;
            _rigidbody2D.angularDrag = 999;
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            _stateManager = GameObject.FindGameObjectWithTag("Player").GetComponent<StateManager>();
            InvokeRepeating("HandleShooting", 3f, _stats.FireRate);
        }
        
        private void FixedUpdate()
        {
            _frames++;

            if(_tookDamage)
            {
                Knockback();
            }
            HandleMovement();
        }

        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (otherCollider.CompareTag("Player"))
            {
                _stateManager.TakeDamage(_stats.Damage);
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
        private void HandleMovement()
        {
            if (!_canMove) return;

            _distanceToPlayer = Vector2.Distance(_stateManager.transform.position, transform.position);
            
            if (_distanceToPlayer > _safeDistance)
            {
                _moveDirection = _stateManager.transform.position - transform.position;
                _rigidbody2D.velocity = _moveDirection * _stats.MoveSpeed;
            }
            else
            {
                if(_frames > 80)
                {
                    _moveDirection = Random.insideUnitCircle * 10f; // Random dash
                    _frames = 0;
                }
                _rigidbody2D.velocity = _moveDirection * (_stats.MoveSpeed + .2f);
            }
            
            transform.rotation = Quaternion.LookRotation(Vector3.forward, _stateManager.transform.position - transform.position);
        }
        
        private void HandleShooting()
        {
            if (!_canMove || _tookDamage) return;

            float distance = Vector2.Distance(transform.position, _stateManager.transform.position);
            
            if (distance < 7)
            {
                GameObject bullet = ObjectPooler.Instance.GetEnemyProjectileObject();
                if (bullet != null)
                {
                    bullet.transform.position = gameObject.transform.GetChild(0).position;
                    bullet.transform.rotation = gameObject.transform.GetChild(0).rotation;
                    bullet.GetComponent<Projectile>().MoveDirection = gameObject.transform.GetChild(0).up;
                    bullet.GetComponent<Projectile>().Damage = _stats.Damage;
                    bullet.SetActive(true);
                    
                    if(_shotSfx != null)
                        AudioHandler.PlaySFX(_shotSfx);
                }
            }
        }
        
        public void TakeDamage(int damage)
        {
            if(_stats.Health <= 1)
            {
                _stateManager.Stats.Xp += _stats.XpValue;
                gameObject.SetActive(false);
                LevelController.Instance.PlayerScore += _stats.ScoreValue;
                LevelController.Instance.EnemiesKilled++;
                if(_deathSfx != null)
                    AudioHandler.PlaySFX(_deathSfx);
                CancelInvoke();
            }
            else
            {
                _stats.Health -= damage;
                _tookDamage = true;
                _canMove = false;
            }
        }
        
        private void Knockback()
        {
            _totalTimer += Time.deltaTime;
            if(_totalTimer >= KNOCKBACK_DURATION)
            {
                _totalTimer = 0;
                _tookDamage = false;
                _canMove = true;
            }
            
            _rigidbody2D.velocity = new Vector2(-transform.up.x * 2f, -transform.up.y * 2f);
        }

        #endregion
    }
}
