using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool enemyBullet;
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9) { Destroy(gameObject); } //This means bullet hits a wall or something.
        if (other.gameObject.GetComponent<IDamagable>() != null && enemyBullet && !other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<IDamagable>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if(other.gameObject.GetComponent<IDamagable>() != null && !enemyBullet && !other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<IDamagable>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
