using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool enemyBullet;
    public int damage;

    private void OnEnable()
    {
        Invoke(nameof(DestroyByTime), 1.5f);
    }

    private void DestroyByTime()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IDamagable>() != null && enemyBullet && !other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<IDamagable>().TakeDamage(damage);
            Destroy(gameObject);
            return;
        }
        else if(other.gameObject.GetComponent<IDamagable>() != null && !enemyBullet && !other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<IDamagable>().TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("Enemy") && !other.gameObject.CompareTag("Weapon"))
        {
            Destroy(gameObject);
        }
    }
}
