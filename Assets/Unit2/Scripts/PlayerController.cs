using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private TextMeshProUGUI _winTextObj;
    [SerializeField] private float _speed = 0;
    [SerializeField] private bool _isMoving = false;
    private Rigidbody _rb;
    private float _movementX;
    private float _movementY;
    private int _pickupCount = 0;
    private Vector3 _target;

    // Start is called before the first frame update.
    void Start()
    {
        // Get and store the Rigidbody component attached to the player.
        _rb = GetComponent<Rigidbody>();
        _pickupCount = 0;
        SetCountText();
        _winTextObj.gameObject.SetActive(false);
    }

    // This function is called when a move input is detected.
    void OnMove(InputValue movementValue)
    {
        // Convert the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();

        // Store the X and Y components of the movement.
        _movementX = movementVector.x;
        _movementY = movementVector.y;
    }


    private void Update()
    {
        if (Pointer.current.press.isPressed)
        {
            Vector2 aimPos = Pointer.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(aimPos);
            Debug.DrawRay(ray.origin, ray.direction * 50, Color.yellow);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, 1 << 4))
            {
                _target = hit.point;
                _isMoving = true;
            }
        }
        else
        {
            _isMoving = false;
        }
    }

    void FixedUpdate()
    {
        // Create a 3D movement vector using the X and Y inputs.
        Vector3 movement = new Vector3(_movementX, 0.0f, _movementY);

        // Apply force to the Rigidbody to move the player.
        _rb.AddForce(movement.normalized * _speed);

        if (_isMoving)
        {
            Vector3 direction = _target - transform.position;
            _rb.AddForce(direction.normalized * _speed);

            if (direction.sqrMagnitude < 0.5f)
            {
                _isMoving = false;
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            _pickupCount++;
            SetCountText();
            if (_pickupCount >= 11)
            {
                _winTextObj.gameObject.SetActive(true);
                Destroy(GameObject.FindGameObjectWithTag("Enemy"));
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Destroy the current object
            Destroy(gameObject);
            // Update the winText to display "You Lose!"
            _winTextObj.gameObject.SetActive(true);
            _winTextObj.text = "You Lose!";
        }
    }

    private void SetCountText()
    {
        _countText.text = "Count: " + _pickupCount;
    }
}