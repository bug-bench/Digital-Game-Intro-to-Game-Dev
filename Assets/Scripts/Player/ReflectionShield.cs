using UnityEngine;

public class ReflectionShield : MonoBehaviour
{
    public GameObject shieldVisual;
    public float duration = 3f;
    private bool isActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isActive)
        {
            ActivateShield();
        }
    }

    void ActivateShield()
    {
        isActive = true;

        if (shieldVisual != null)
            shieldVisual.SetActive(true);

        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Spike"),
            true
        );

        Invoke(nameof(DeactivateShield), duration);
    }

    void DeactivateShield()
    {
        isActive = false;

        if (shieldVisual != null)
            shieldVisual.SetActive(false);


        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Player"),
            LayerMask.NameToLayer("Spike"),
            false
        );
    }

    public bool IsShieldActive()
    {
        return isActive;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive && other.CompareTag("Projectile"))
        {
            Destroy(other.gameObject);
        }
    }
}