using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 2f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= fireRate)
        {
            Shoot();
            timer = 0f;
        }
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.left * 5f;
    }
}