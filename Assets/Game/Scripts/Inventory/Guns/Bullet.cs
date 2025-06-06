using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool enemyBullet;
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IDamagable>() != null && enemyBullet && !other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<IDamagable>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if(other.gameObject.GetComponent<IDamagable>() != null && !enemyBullet)
        {
            other.gameObject.GetComponent<IDamagable>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
