using UnityEngine;

public class FallingSpikeProjectile : MonoBehaviour
{
    public float lifeTime = 5f;
    public float knockbackForce = 400f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ReflectionShield shield = collision.GetComponent<ReflectionShield>();
            if (shield != null && shield.IsShieldActive())
            {

                Debug.Log("Player passed through falling spike with shield.");
                return;
            }


            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
                rb.AddForce(knockbackDir * knockbackForce);
            }

            Debug.Log("Player hit by spike! Knocked back!");
        }
    }
}
