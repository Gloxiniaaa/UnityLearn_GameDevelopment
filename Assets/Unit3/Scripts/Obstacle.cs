using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private static WaitForSeconds _waitForSeconds0_5 = new WaitForSeconds(0.5f);
    [SerializeField] private float _minSize = 0.5f;
    [SerializeField] private float _maxSize = 2.0f;
    [SerializeField] private float _minSpeed = 50f;
    [SerializeField] private float _maxSpeed = 150f;
    [SerializeField] private AudioClip _impactSfx;
    [SerializeField] private GameObject _impactVfx;

    Rigidbody2D _rb;
    AudioSource _as;

    void Start()
    {
        float randomSize = Random.Range(_minSize, _maxSize);
        transform.localScale = new Vector3(randomSize, randomSize, 1);

        float randomSpeed = Random.Range(_minSpeed, _maxSpeed) / randomSize;
        Vector2 randomDirection = Random.insideUnitCircle;

        _as = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.AddForce(randomDirection * randomSpeed);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (transform.localScale.x < collision.transform.localScale.x)
            return;
        _as.PlayOneShot(_impactSfx);
        Vector3 connectVector = collision.transform.position - transform.position;
        Vector3 impactPoint = transform.position + connectVector.normalized * transform.localScale.x;
        GameObject vfx = Instantiate(_impactVfx, impactPoint, Quaternion.identity);
        StartCoroutine(DelayDestroyObject(vfx));
    }

    IEnumerator DelayDestroyObject(GameObject obj)
    {
        yield return _waitForSeconds0_5;
        Destroy(obj);
    }
}