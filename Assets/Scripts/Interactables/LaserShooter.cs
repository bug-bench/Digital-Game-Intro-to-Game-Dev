using UnityEngine;
using System.Collections.Generic;

public class LaserShooter : MonoBehaviour
{
    public LineRenderer laserLine;
    public float laserLength = 20f;
    public LayerMask laserHitLayers;
    public float laserDuration = 0.1f;
    public float fireRate = 2f;

    public GameObject laserDoorToOpen;
    public float knockbackForce = 500f;

    private float nextFireTime;

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            FireLaser();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FireLaser()
    {
        Vector2 start = transform.position;
        Vector2 direction = transform.right;
        Vector2 end = start + direction * laserLength;

        RaycastHit2D hit = Physics2D.Raycast(start, direction, laserLength, laserHitLayers);

        if (hit.collider != null)
        {
            end = hit.point;


            if (hit.collider.CompareTag("Player"))
            {
                ReflectionShield shield = hit.collider.GetComponent<ReflectionShield>();
                if (shield != null && shield.IsShieldActive())
                {
                    Debug.Log("Laser hit shielded player → open door");
                    if (laserDoorToOpen != null)
                        Destroy(laserDoorToOpen); //  直接销毁门
                }
                else
                {

                    Rigidbody2D rb = hit.collider.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        Vector2 knockbackDir = (hit.collider.transform.position - transform.position).normalized;
                        rb.AddForce(knockbackDir * knockbackForce);
                        Debug.Log("Laser hit player and knocked back!");
                    }
                }

                StartCoroutine(ShowLaser(start, end));
                return;
            }


            if (hit.collider.CompareTag("LaserDoor"))
            {
                Destroy(hit.collider.gameObject);
                Debug.Log("Laser hit and destroyed door directly!");
            }
        }

        StartCoroutine(ShowLaser(start, end));
    }

    System.Collections.IEnumerator ShowLaser(Vector2 start, Vector2 end)
    {
        laserLine.SetPosition(0, start);
        laserLine.SetPosition(1, end);
        laserLine.enabled = true;
        yield return new WaitForSeconds(laserDuration);
        laserLine.enabled = false;
    }
}
