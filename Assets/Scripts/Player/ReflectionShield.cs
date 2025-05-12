using UnityEngine;

public class ReflectionShield : MonoBehaviour
{
    [Header("Shield Visual & Animation")]
    public GameObject shieldVisual;
    public Animator shieldAnim;

    [Header("Shield Settings")]
    [SerializeField] private float duration = 5f;
    private bool isActive = false;

    [Header("Control Settings")]
    public bool keyToggle = false; // Toggle between Z and K

    void Update()
    {
        if ((keyToggle && Input.GetKeyDown(KeyCode.K)) || (!keyToggle && Input.GetKeyDown(KeyCode.Z)))
        {
            ActivateShield();
        }
    }

    void ActivateShield()
    {
        if (isActive) return; // ·ÀÖ¹ÖØ¸´¼¤»î

        isActive = true;

        if (shieldAnim != null)
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

        if (shieldAnim != null)
            shieldAnim.SetBool("Shield", false);

        if (shieldVisual != null)
            shieldVisual.SetActive(false);

        // »Ö¸´Óë¼â´ÌµÄÅö×²
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
