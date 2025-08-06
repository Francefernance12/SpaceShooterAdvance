using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private int powerupID; // 0 = Triple Laser, 1 = Speed Boost, 2 = Shield
    [SerializeField] private AudioClip _powerupClip;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5.75f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player_comp = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_powerupClip, transform.position);

            if (player_comp != null)
            {
                switch (powerupID)
                {
                    case 0:
                        player_comp.TripleLaserActive();
                        break;
                    case 1:
                        player_comp.SpeedBoostActive();
                        break;
                    case 2:
                        player_comp.ShieldActive();
                        break;
                    default:
                        Debug.Log("Default Case");
                        break;
                }
            }
            Destroy(gameObject);
        }
    }
}
