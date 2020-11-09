using UnityEngine;
using Buriola.Audio;
using Buriola.Controller;
using Buriola.Managers;
using UnityEngine.Serialization;

namespace Buriola.AI
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Meteors : MonoBehaviour
    {
        #region Variables   
        [Header("Meteor Stats")]
        [SerializeField]
        private EnemyStats stats = null;
        
        [FormerlySerializedAs("force")]
        [Range(0, 1f)]
        [SerializeField]
        private float _force = 0f;

        [FormerlySerializedAs("minorMeteors")] 
        [Tooltip("Spawn minor meteors on death")]
        [SerializeField]
        private GameObject[] _minorMeteors = null;
        
        [FormerlySerializedAs("amount")]
        [Range(0, 5)] 
        [SerializeField]
        private int _amount = 1;

        [FormerlySerializedAs("rotationSpeed")]
        [Header("Meteor Settings")]
        [SerializeField]
        private float _rotationSpeed = 10f;
        
        [FormerlySerializedAs("rotationTime")] 
        [SerializeField]
        private float _rotationTime = 10f;

        [FormerlySerializedAs("impactSfx")]
        [Header("Audio Settings")]
        [SerializeField]
        private AudioClip _impactSfx = null;
        
        [FormerlySerializedAs("deathSound")] 
        [SerializeField]
        private AudioClip _deathSound = null;

        private bool _destroyedByPlayer;
        private Rigidbody2D _rigidbody2d;
        private GameObject _player;
        private Vector3 _moveDirection;
        #endregion

        #region Unity Functions
        private void Awake()
        {
            _rigidbody2d = GetComponent<Rigidbody2D>();
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnEnable()
        {
            stats.Health = stats.MaxHealth;

            _destroyedByPlayer = false;
            _rigidbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;

            Invoke(nameof(ChangeRotation), _rotationTime);

            if (_player != null)
            {
                //After its enabled, it will take the last player position and apply a force in that direction
                Vector3 moveDir = _player.transform.position - transform.position;
                _rigidbody2d.AddForce(moveDir * _force, ForceMode2D.Impulse);
            }
        }

        private void FixedUpdate()
        {
            transform.Rotate(new Vector3(0, 0, _rotationSpeed * Time.deltaTime));
        }

        private void OnDisable()
        {
            if (_destroyedByPlayer)
            {
                SpawnMinorMeteors();
                if (_deathSound != null) AudioHandler.PlaySFX(_deathSound);
            }
        }

        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (otherCollider.CompareTag("Player"))
            {
                StateManager st = otherCollider.gameObject.GetComponent<StateManager>();
                st.TakeDamage(stats.Damage);
                if (_impactSfx != null) AudioHandler.PlaySFX(_impactSfx);
                gameObject.SetActive(false);
            }
        }
        #endregion

        #region My Functions
        private void ChangeRotation()
        {
            if (Random.value > 0.5f)
            {
                _rotationSpeed = -_rotationSpeed;
            }

            Invoke(nameof(ChangeRotation), _rotationTime);
        }
        
        private void SpawnMinorMeteors()
        {
            if (_minorMeteors.Length == 0) return;

            for (int i = 0; i < _amount; i++)
            {
                GameObject meteor = ObjectPooler.Instance.GetEnemyObject("M_Meteor");
                
                meteor.transform.position = new Vector3(transform.position.x + (Random.Range(-2, 2)),
                    transform.position.y + (Random.Range(-2, 2)), 0f);
                meteor.SetActive(true);
            }
        }
        
        public void TakeDamage(int damage)
        {
            if (stats.Health <= 1)
            {
                _destroyedByPlayer = true;
                _player.GetComponent<StateManager>().Stats.Xp += stats.XpValue;
                gameObject.SetActive(false);
                LevelController.Instance.PlayerScore += stats.ScoreValue;
                LevelController.Instance.EnemiesKilled++;
            }
            else
            {
                stats.Health -= damage;
            }
        }

        #endregion
    }
}
