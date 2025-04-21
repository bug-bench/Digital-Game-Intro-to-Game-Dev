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
        // 子弹向右移动（你也可以根据发射方向修改）
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // 超出最大距离则销毁
        if (Vector2.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 检查玩家是否开启了护盾
            ReflectionShield shield = collision.GetComponent<ReflectionShield>();
            if (shield == null || !shield.IsShieldActive())
            {
                // 击退
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

        // 碰到其他物体（如墙）也销毁
        if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
