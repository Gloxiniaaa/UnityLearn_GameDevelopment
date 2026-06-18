using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float _minSize = 0.5f;
    [SerializeField] private float _maxSize = 2.0f;
    [SerializeField] private float _minSpeed = 50f;
    [SerializeField] private float _maxSpeed = 150f;

    Rigidbody2D rb;

    void Start()
    {
        float randomSize = Random.Range(_minSize, _maxSize);
        transform.localScale = new Vector3(randomSize, randomSize, 1);

        float randomSpeed = Random.Range(_minSpeed, _maxSpeed) / randomSize;
        Vector2 randomDirection = Random.insideUnitCircle;

        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(randomDirection * randomSpeed);
    }
}