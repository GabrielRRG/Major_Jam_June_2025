using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IDamagable>() != null)
        {
            other.gameObject.GetComponent<IDamagable>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
