using UnityEngine;

public class projectiles : MonoBehaviour
{
    public float knockbackForce = 400f;
    public float maxDistance = 8f;
    public float speed = 10f;

    private Vector2 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // �ӵ������ƶ�����Ҳ���Ը��ݷ��䷽���޸ģ�
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // ����������������
        if (Vector2.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // �������Ƿ����˻���
            ReflectionShield shield = collision.GetComponent<ReflectionShield>();
            if (shield == null || !shield.IsShieldActive())
            {
                // ����
                Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
                Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.AddForce(knockbackDir * knockbackForce);
                }

                Debug.Log("Player hit by projectile! Knocked back!");
            }
            else
            {
                Debug.Log("Projectile blocked by shield.");
            }

            Destroy(gameObject);
        }

        // �����������壨��ǽ��Ҳ����
        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
