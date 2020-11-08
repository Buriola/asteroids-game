using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a helper class to disable the objects that are off screen.
/// </summary>
public class Boundary : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag.Contains("Projectile"))
            collider.gameObject.SetActive(false);
        else if (collider.tag.Contains("Enemy"))
            collider.gameObject.SetActive(false);
        else if (collider.tag.Contains ("Meteor"))
            collider.gameObject.SetActive(false);
        else
            Destroy(gameObject);
    }

}
