using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour {
    
    private bool isPaused;

    private GameController m_GameController;

    public GameObject pauseMenuCanvas;

    private void Awake()
    {
        m_GameController = FindObjectOfType<GameController>();
        isPaused = false;
    }
    // Update is called once per frame
    void Update () {

        if (isPaused)
        {
            pauseMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pauseMenuCanvas.SetActive(false);
            Time.timeScale = 1f;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            m_GameController.isPaused = isPaused;
        }
	}

    public void Resume()
    {
        isPaused = false;
        m_GameController.isPaused = false;
    }

    public void QuitGame()
    {
        //On hit Quit Game button
        Application.Quit();
    }
}
