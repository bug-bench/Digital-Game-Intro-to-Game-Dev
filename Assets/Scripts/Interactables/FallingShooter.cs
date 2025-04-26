using UnityEngine;

public class FallingShooter : MonoBehaviour
{
    public GameObject spikePrefab;       
    public Transform firePoint;          
    public float shootInterval = 2f;     

    private float shootTimer;

    void Update()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            ShootSpike();
            shootTimer = shootInterval;
        }
    }

    void ShootSpike()
    {
        Instantiate(spikePrefab, firePoint.position, Quaternion.identity);
    }
}
