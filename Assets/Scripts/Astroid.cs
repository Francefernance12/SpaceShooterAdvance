using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private float _rotateSpeed = 30.0f;
    [SerializeField] private int _lives = 2;
    [SerializeField] private int _incrementScore = 30;
    [SerializeField] private GameObject _explosionPrefab;
    private Player _player;

    // Update is called once per frame
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is null");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);

        if (transform.position.y <= -5.75f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser" && _lives == 0)
        {
            if (_player != null)
            {
                _player.AddScore(_incrementScore);
            }

            Destroy(other.gameObject);

            // Astroid/Explosion
            GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        else if (other.tag == "Laser" && _lives > 0)
        {
            _lives--;
            Destroy(other.gameObject);
        }

        if (other.tag == "Player")
        {
            _player.instakilled();
        }
    }


}
