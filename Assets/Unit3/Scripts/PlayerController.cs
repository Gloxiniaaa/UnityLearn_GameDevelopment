using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _thrustForce;
    [SerializeField] private float _maxSpeed = 100;
    [SerializeField] private float _scoreMultiplier;
    [SerializeField] private GameObject _boosterFlame;
    [SerializeField] private UIDocument _uIDocument;
    [SerializeField] private GameObject _explosionVfx;
    private Label _scoreText;
    private Button _restartButton;
    private Rigidbody2D _rb;
    private float _elapsedTime;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _boosterFlame.SetActive(false);
        _scoreText = _uIDocument.rootVisualElement.Q<Label>("ScoreLabel");
        _restartButton = _uIDocument.rootVisualElement.Q<Button>("RestartButton");
        _restartButton.style.display = DisplayStyle.None;
        _restartButton.clicked += ReloadScene;
    }

    void Update()
    {
        UpdateScore();
        MovePlayer();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(_explosionVfx, transform.position, transform.rotation);
        _restartButton.style.display = DisplayStyle.Flex;
        Destroy(gameObject);
    }

    private void MovePlayer()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Vector3 direction = mousePos - transform.position;
            direction.z = 0;
            direction.Normalize();
            transform.up = direction;
            _rb.AddForce(_thrustForce * direction);
            if (_rb.linearVelocity.magnitude > _maxSpeed)
            {
                _rb.linearVelocity = _rb.linearVelocity.normalized * _maxSpeed;
            }
        }
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _boosterFlame.SetActive(true);
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _boosterFlame.SetActive(false);

        }
    }

    private void UpdateScore()
    {
        _elapsedTime += Time.deltaTime;
        int score = Mathf.RoundToInt(_elapsedTime * _scoreMultiplier);
        _scoreText.text = "Score: " + score.ToString();
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}