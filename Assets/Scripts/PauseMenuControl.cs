using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuControl : MonoBehaviour
{
    public GameObject playerObject;

    private Controler controler;

    void Start()
    {
        controler = playerObject.GetComponent<Controler>();
    }

    public void Resume()
    {
        controler.ResumeGame();
    }

    public void MainMenuTransfer()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
