using UnityEngine;
using Buriola.Audio;
using Buriola.Controller;
using Buriola.Managers;
using UnityEngine.Serialization;

namespace Buriola.AI
{
    public class UFO : MonoBehaviour
    {
        #region Variables
        [FormerlySerializedAs("stats")] 
        [SerializeField]
        private EnemyStats _stats = null;

        [FormerlySerializedAs("deathSfx")]
        [Header("Audio Settings")]
        [SerializeField]
        private AudioClip _deathSfx = null;

        private StateManager _stateManager;

        private Rigidbody2D _rigidbody2D;
        private bool _tookDamage;
        private float _totalTimer;
        
        private const float KNOCKBACK_DURATION = 1f;
        #endregion

        #region Unity Functions
        private void OnEnable()
        {
            _tookDamage = false;
            _stats.Health = _stats.MaxHealth;

            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.drag = 4;
            _rigidbody2D.angularDrag = 999;
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            _rigidbody2D.gravityScale = 0;

            _stateManager = GameObject.FindGameObjectWithTag("Player").GetComponent<StateManager>();
        }

        private void FixedUpdate()
        {
            if (_tookDamage)
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
            }
        }

        #endregion

        #region My Functions
        private void HandleMovement()
        { 
            transform.position = Vector3.MoveTowards(transform.position, _stateManager.transform.position, _stats.MoveSpeed * Time.deltaTime);
        }
        
        public void TakeDamage(int damage)
        {
            if (_stats.Health <= 1)
            {
                _stateManager.Stats.Xp += _stats.XpValue;
                gameObject.SetActive(false);
                LevelController.Instance.PlayerScore += _stats.ScoreValue;
                LevelController.Instance.EnemiesKilled++;
                if (_deathSfx != null)
                    AudioHandler.PlaySFX(_deathSfx);
                CancelInvoke();
            }
            else
            {
                _stats.Health -= damage;
                _tookDamage = true;
            }
        }
        
        private void Knockback()
        {
            _totalTimer += Time.deltaTime;
            if (_totalTimer >= KNOCKBACK_DURATION)
            {
                _totalTimer = 0;
                _tookDamage = false;
            }

            //Knockback
            _rigidbody2D.velocity = new Vector2(-transform.up.x * 2f, -transform.up.y * 2f);
        }
        #endregion
    }
}