using UnityEngine;

public class ReflectionShield : MonoBehaviour
{
    public GameObject shieldVisual;
    public float duration = 3f;
    private bool isActive = false;

    public Animator shieldAnim;
    
    [Header("Control Settings")]
    public bool keyToggle = false; // Toggle between Z and K

    void Update()
    {
        if ((keyToggle && Input.GetKeyDown(KeyCode.K)) || (!keyToggle && Input.GetKeyDown(KeyCode.Z)))        {
            ActivateShield();
        }
    }

    void ActivateShield()
    {
        isActive = true;

        shieldAnim.SetBool("Shield", true);

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

        shieldAnim.SetBool("Shield", false);

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