using UnityEngine;

namespace Buriola.Managers
{
    public class Boundary : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D otherCollider)
        {
            if (otherCollider.tag.Contains("Projectile"))
                otherCollider.gameObject.SetActive(false);
            else if (otherCollider.tag.Contains("Enemy"))
                otherCollider.gameObject.SetActive(false);
            else if (otherCollider.tag.Contains("Meteor"))
                otherCollider.gameObject.SetActive(false);
            else
                Destroy(gameObject);
        }
    }
}
