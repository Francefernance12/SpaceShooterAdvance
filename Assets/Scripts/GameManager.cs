using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _isGameOver;
    [SerializeField] private bool _isGamePaused = false;

    private UIManager _uiManager;

    private void Start()
    {
        // Find UImanager
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UIManager is null");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            SceneManager.LoadScene(1); // Current Game Scene
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_isGamePaused)
            {
                GameUnpaused();
            }
            else
            {
                GamePaused();
            }
        }
    }

    public void GameOver()
    {
        Debug.Log("GameManager::GameOver() Called");
        _isGameOver = true;
    }
    
    public void GamePaused()
    {
        Debug.Log("GameManager::GamePaused() Called");
        _uiManager.GamePaused();
        Time.timeScale = 0f;
        _isGamePaused = true;
    }

    public void GameUnpaused()
    {
        Debug.Log("GameManager::GameUnpaused() Called");
        _uiManager.GameUnpaused();
        Time.timeScale = 1f;
        _isGamePaused = false;
    }



}
