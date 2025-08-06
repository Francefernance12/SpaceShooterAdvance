using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   
    // Properties
    [Header("Movement")]
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _run_speed = 8.5f;
    [SerializeField] private float _speed_boost = 2.0f;

    [Header("Combat")]
    [SerializeField] private float _fireRate = 0.25f;
    private float _canFire = -1.0f;
    [SerializeField] private int _lives = 3;

    [Header("Power Ups")]
    [SerializeField] private bool _tripleLaserActive = false;
    [SerializeField] private bool _speedBoostActive = false;
    [SerializeField] private bool _shieldActive = false;

    [Header("UI")]
    [SerializeField] private int _score;

    [Header("References")]
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleLaserPrefab;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private GameObject[] _damagedPlayerVisualizer;
    [SerializeField] private GameObject _explosionPrefab;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private AudioSource _laserAudio;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, -2.5f, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager is null");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UIManager is null");
        }

        _laserAudio = GetComponent<AudioSource>();
        if (_laserAudio == null)
        {
            Debug.LogError("LaserAudio is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        MovementPosition();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            Fire();
        }
    }

    // Movement
    void MovementPosition()
    {
        // Key Controls
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        // Store the base speed
        float currentSpeed = _speed;
        
        // Apply speed boost if shift is held down
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = _run_speed;
        }
        
        // Apply additional speed boost if powerup is active
        if (_speedBoostActive)
        {
            currentSpeed *= _speed_boost;
        }
        
        // Apply movement with the calculated speed
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * currentSpeed * Time.deltaTime);

        // X wrapping
        if (transform.position.x >= 9.17f)
        {
            transform.position = new Vector3(-9.16f, transform.position.y, 0);

        } else if (transform.position.x <= -9.17f)
        {
            transform.position = new Vector3(9.16f, transform.position.y, 0);
        }

        // Y Boundaries
        if (transform.position.y >= -0.5f)
        {
            transform.position = new Vector3(transform.position.x, -0.5f, 0);
        } else if (transform.position.y <= -3.50f)
        {
            transform.position = new Vector3(transform.position.x, -3.50f, 0);
        }
    }

    // Fire
    void Fire()
    {
        // Fire Rate. Future time set. Current game time + fire rate.
        _canFire = Time.time + _fireRate;

        if (_tripleLaserActive)
        {
            GameObject newLaser = Instantiate(_tripleLaserPrefab, transform.position + Vector3.up * 1.05f, Quaternion.identity);

        }
        else
        {
            GameObject newLaser = Instantiate(_laserPrefab, transform.position + Vector3.up * 1.05f, Quaternion.identity);   
        }

        _laserAudio.mute = false;
        _laserAudio.Play();
    }

    // Lives
    public void DeductLives()
    {
        if (_shieldActive)
        {
            _shieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;
        if (_lives >= 1)
        {
            DamagePlayerVisuals(_lives - 1);
        }
        _uiManager.UpdateLives(_lives);
        
        // Game Over
        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _uiManager.ShowGameOver();
            Destroy(gameObject);
        }
    }

    public void instakilled()
    {
        _lives = 1;
        DeductLives();
    }

    // Power Ups toggle
    public void TripleLaserActive()
    {
        _tripleLaserActive = true;
        StartCoroutine(TripleLaserPowerUp());
    }

    IEnumerator TripleLaserPowerUp()
    {
        while (_tripleLaserActive)
        {
            yield return new WaitForSeconds(5.0f);
            _tripleLaserActive = false;
        }
    }

    public void SpeedBoostActive()
    {
        _speedBoostActive = true;
        StartCoroutine(SpeedBoostPowerUp());
    }

    IEnumerator SpeedBoostPowerUp()
    {
        while (_speedBoostActive)
        {
            yield return new WaitForSeconds(5.0f);
            _speedBoostActive = false;
        }
    }

    public void ShieldActive()
    {
        _shieldActive = true;
        if (_shieldActive)
        {
            _shieldVisualizer.SetActive(true);
        }
    }

    // Score
    public void AddScore(int score)
    {
        _score += score;
        _uiManager.UpdateScore(_score);
    }

    // Damage Player Visuals
    public void DamagePlayerVisuals(int index)
    {
        _damagedPlayerVisualizer[index].SetActive(true);
    }
}
