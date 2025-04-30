using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveDistance = 11f;    // How far to move from the start position
    public float moveSpeed = 3f;        // How fast to move
    private Vector3 startPos;
    private bool movingRight = true;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            if (transform.position.x >= startPos.x + moveDistance)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            if (transform.position.x <= startPos.x - moveDistance)
            {
                movingRight = true;
            }
        }
    }
}