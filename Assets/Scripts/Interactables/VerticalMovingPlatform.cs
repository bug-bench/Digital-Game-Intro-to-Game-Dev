using UnityEngine;

public class VerticalMovingPlatform : MonoBehaviour
{
    public float moveDistance = 3f;    // How far it moves up and down
    public float moveSpeed = 2f;        // How fast it moves

    private Vector3 startPosition;
    private bool movingUp = true;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        Vector3 targetPosition;

        if (movingUp)
            targetPosition = startPosition + Vector3.up * moveDistance;
        else
            targetPosition = startPosition;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            movingUp = !movingUp;
    }
}
