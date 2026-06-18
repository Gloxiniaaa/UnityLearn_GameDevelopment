using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    private float _movementX;
    private float _movementY;

    private int _pickupCount = 0;
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private GameObject _winTextObj;
    [SerializeField] private float _speed = 0;

    // Start is called before the first frame update.
    void Start()
    {
        // Get and store the Rigidbody component attached to the player.
        _rb = GetComponent<Rigidbody>();
        _pickupCount = 0;
        SetCountText();
        _winTextObj.SetActive(false);
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

    // FixedUpdate is called once per fixed frame-rate frame.
    void FixedUpdate()
    {
        // Create a 3D movement vector using the X and Y inputs.
        Vector3 movement = new Vector3(_movementX, 0.0f, _movementY);

        // Apply force to the Rigidbody to move the player.
        _rb.AddForce(movement * _speed);
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
                _winTextObj.SetActive(true);
            }
        }
    }

    private void SetCountText()
    {
        _countText.text = "Count: " + _pickupCount;
    }
}