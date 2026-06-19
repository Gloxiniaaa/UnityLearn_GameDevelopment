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
    [SerializeField] private UIDocument _uIDocument;
    [SerializeField] private GameObject _explosionVfx;
    [SerializeField] private GameObject _boosterFlame;
    [SerializeField] private AudioClip _thrustSfx;
    private float _maxThrustDuration;
    private Label _scoreText;
    private Button _restartButton;
    private ProgressBar _progressBar;
    private Rigidbody2D _rb;
    private AudioSource _as;
    private float _elapsedTimeSinceStart;
    private float _elapsedTimeSinceThrust;

    void Awake()
    {
        _as = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody2D>();
        _maxThrustDuration = _thrustSfx.length;
        _boosterFlame.SetActive(false);
        _scoreText = _uIDocument.rootVisualElement.Q<Label>("ScoreLabel");
        _restartButton = _uIDocument.rootVisualElement.Q<Button>("RestartButton");
        _progressBar = _uIDocument.rootVisualElement.Q<ProgressBar>("ThrustBar");

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
        if (Mouse.current.leftButton.isPressed && _elapsedTimeSinceThrust < _maxThrustDuration)
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
            _elapsedTimeSinceThrust += Time.deltaTime;
        }
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _elapsedTimeSinceThrust = 0;
            _boosterFlame.SetActive(true);
            _as.PlayOneShot(_thrustSfx);
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _elapsedTimeSinceThrust = 0;
            _boosterFlame.SetActive(false);
            _as.Stop();
        }
        if (_elapsedTimeSinceThrust > _maxThrustDuration)
        {
            _boosterFlame.SetActive(false);
            _as.Stop();
        }
        _progressBar.value = _elapsedTimeSinceThrust / _maxThrustDuration;
    }

    private void UpdateScore()
    {
        _elapsedTimeSinceStart += Time.deltaTime;
        int score = Mathf.RoundToInt(_elapsedTimeSinceStart * _scoreMultiplier);
        _scoreText.text = "Score: " + score.ToString();
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}