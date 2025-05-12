using UnityEngine;

public class GasZone : MonoBehaviour
{
    public float damageInterval = 1.0f;
    public float knockbackForce = 300f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ReflectionShield shield = collision.GetComponent<ReflectionShield>();
            if (shield == null || !shield.IsShieldActive())
            {
                PlayerHealthManager health = collision.GetComponent<PlayerHealthManager>();
                if (health != null)
                {
                    health.TakeGasDamage(transform.position, knockbackForce);
                }
            }
            Debug.Log("Player is in toxic gas without shield!");
        }
        else
        {
            Debug.Log("Player is protected from gas.");
        }
    }
}

