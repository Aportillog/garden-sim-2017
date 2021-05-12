using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    public void PlayGame()
    {
        //On hit Play button
        SceneManager.LoadScene("Main");
    }
    public void QuitGame()
    {
        //On hit Quit Game button
        Application.Quit();
    }
}
