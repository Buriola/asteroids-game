using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asteroids.Controller;
using Asteroids.AI;

/// <summary>
/// This class represents the projectiles from both player and enemy
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    #region Variables
    private Rigidbody2D rb;

    [Range(5f, 35f)]
    public float speed;

    [HideInInspector]
    public int damage;

    [HideInInspector]
    public Vector3 moveDir;

    #endregion

    #region Unity Functions
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDir * speed; //Projectile goes wherever his spawner is looking
    }

    private void OnEnable()
    {
        //Init settings
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.drag = 4;
        rb.angularDrag = 999;
    }

    /// <summary>
    /// This unity function is checking every possible tag to collide to and applying damage if there is a match
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Enemy Projectile
        if (collider.tag == "Player" && gameObject.tag != "Projectile_P")
        {
            StateManager playerState = collider.gameObject.GetComponent<StateManager>();
            playerState.TakeDamage(damage);
        }

        //Meteor/Asteroid - Acts like a projectile
        if (collider.tag == "Meteor" || collider.tag == "M_Meteor")
        {
            Meteors meteor = collider.gameObject.GetComponent<Meteors>();
            meteor.TakeDamage(damage);
            gameObject.SetActive(false);
        }

        //Player Projectile
        if(collider.tag.Contains("Enemy"))
        {
            if(collider.tag == "EnemyBasic" || collider.tag == "EnemyBasic_1")
            {
                EnemyShipBasic ship = collider.gameObject.GetComponent<EnemyShipBasic>();
                ship.TakeDamage(damage);
                gameObject.SetActive(false);
            }

            if(collider.tag == "EnemyUFO")
            {
                UFO ufo = collider.gameObject.GetComponent<UFO>();
                ufo.TakeDamage(damage);
                gameObject.SetActive(false);
            }
        }

        //Projectile against projectile
        if(collider.tag == "Projectile")
        {
            collider.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

    }
    #endregion
}
