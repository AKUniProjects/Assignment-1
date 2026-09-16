using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float minSize = 0.5f;
    public float maxSize = 2.0f;

    public float minSpeed = 50f;
    public float maxSpeed = 150f;

    public float maxSpinSpeed = 10f;

    Rigidbody2D rb;

    void Start()
    {
        // Randomize obstacle size
        float randomSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randomSize, randomSize,1);

        // Get Rigidbody2D
        rb = GetComponent<Rigidbody2D>();

        // Randomize speed
        // Larger obstacles move slower
        float randomSpeed =
            Random.Range(minSpeed, maxSpeed) / randomSize;

        // Randomize movement direction
        Vector2 randomDirection = Random.insideUnitCircle;

        // Apply movement force
        rb.AddForce(randomDirection * randomSpeed);

        // Randomize spin
        float randomTorque =
            Random.Range(-maxSpinSpeed, maxSpinSpeed);

        rb.AddTorque(randomTorque);
    }
}