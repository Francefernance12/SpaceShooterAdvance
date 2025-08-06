using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Image _livesImg;
    [SerializeField] private Sprite[] _livesVisualizer;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;
    [SerializeField] private Text _pauseText;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is null");
        }
        
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _pauseText.gameObject.SetActive(false);
    }

    public void UpdateScore(int UpdatedScore)
    {
        _scoreText.text = "Score: " + UpdatedScore.ToString();
    }

    public void UpdateLives(int CurrentLives)
    {
        // Switching sprites
        _livesImg.sprite = _livesVisualizer[CurrentLives];
        if (CurrentLives <= 0)
        {
            ShowGameOver();
        }
    }

    public void GamePaused()
    {
        _pauseText.gameObject.SetActive(true);
    }
    
    public void GameUnpaused()
    {
        _pauseText.gameObject.SetActive(false);
    }
    
    public void ShowGameOver()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        // Flicker
        StartCoroutine(FlickerText());
    }

    private IEnumerator FlickerText()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);

        }
    }
}
