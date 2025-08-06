using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Movement Variables
    [SerializeField] private float _speed = 4.0f;
    [SerializeField] private int _incrementScore = 10;
    
    private Player _player;
    private Animator _animator;
    private AudioSource _explosionAudio;
    
    // Start is called before the first frame update
    void Start()
    {
        // Enemy spawn at top
        transform.position = new Vector3(Random.Range(-8.5f, 8.5f), 7.95f, 0);
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is null");
        }
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("Animator is null");
        }

        _explosionAudio = GetComponent<AudioSource>();
        if (_explosionAudio == null)
        {
            Debug.LogError("ExplosionAudio is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Enemy move down
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // Enemy respawn
        if (transform.position.y <= -5.75f)
        {
            // Spawn at top
            transform.position = new Vector3(Random.Range(-8.5f, 8.5f), 10.0f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            if (_player != null)
            {
                _player.AddScore(_incrementScore);
            }
            
            Destroy(other.gameObject);
            // Also destroy enemy
            _animator.SetTrigger("OnEnemyDeath");
            // disable collider
            GetComponent<Collider2D>().enabled = false;
            _explosionAudio.mute = false;
            _explosionAudio.Play();
            Destroy(this.gameObject, 1.2f);
        }

        if (other.tag == "Player")
        {
            if (_player != null)
            {
                _player.DeductLives();
                // Destroy enemy
                _animator.SetTrigger("OnEnemyDeath");
                // disable collider
                GetComponent<Collider2D>().enabled = false;
                _explosionAudio.mute = false;
                _explosionAudio.Play();
                Destroy(this.gameObject, 1.2f);
            }
        }
    }
}
