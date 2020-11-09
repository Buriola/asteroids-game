using UnityEngine;
using Buriola.AI;
using Buriola.Controller;
using UnityEngine.Serialization;

namespace Buriola.Projectiles
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        #region Variables

        private Rigidbody2D _rigidbody2D;

        [FormerlySerializedAs("speed")] 
        [SerializeField, Range(5f, 35f)] 
        private float _speed = 10;
        public int Damage { get; set; }
        public Vector3 MoveDirection { get; set; }

        #endregion

        #region Unity Functions

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rigidbody2D.velocity = MoveDirection * _speed; //Projectile goes wherever his spawner is looking
        }

        private void OnEnable()
        {
            //Init settings
            _rigidbody2D.gravityScale = 0;
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            _rigidbody2D.drag = 4;
            _rigidbody2D.angularDrag = 999;
        }
        
        private void OnTriggerEnter2D(Collider2D otherCollider2D)
        {
            //Enemy Projectile
            if (otherCollider2D.CompareTag("Player") && !gameObject.CompareTag("Projectile_P"))
            {
                StateManager playerState = otherCollider2D.gameObject.GetComponent<StateManager>();
                playerState.TakeDamage(Damage);
            }

            //Meteor/Asteroid - Acts like a projectile
            if (otherCollider2D.CompareTag("Meteor") || otherCollider2D.CompareTag("M_Meteor"))
            {
                Meteors meteor = otherCollider2D.gameObject.GetComponent<Meteors>();
                meteor.TakeDamage(Damage);
                gameObject.SetActive(false);
            }

            //Player Projectile
            if (otherCollider2D.tag.Contains("Enemy"))
            {
                if (otherCollider2D.CompareTag("EnemyBasic") || otherCollider2D.CompareTag("EnemyBasic_1"))
                {
                    EnemyShipBasic ship = otherCollider2D.gameObject.GetComponent<EnemyShipBasic>();
                    ship.TakeDamage(Damage);
                    gameObject.SetActive(false);
                }

                if (otherCollider2D.CompareTag("EnemyUFO"))
                {
                    UFO ufo = otherCollider2D.gameObject.GetComponent<UFO>();
                    ufo.TakeDamage(Damage);
                    gameObject.SetActive(false);
                }
            }

            //Projectile against projectile
            if (otherCollider2D.CompareTag("Projectile"))
            {
                otherCollider2D.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }

        }

        #endregion
    }
}
