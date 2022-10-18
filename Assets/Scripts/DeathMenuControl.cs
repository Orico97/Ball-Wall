using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DeathMenuControl : MonoBehaviour
{
    public GameObject pauseMenuObject;
    public GameObject playerObject;
    public Rigidbody2D playerRigidbody;
    public TMP_Text textScore;
    //public TMP_Text textDeathReason;

    private PauseMenuControl pauseMenuControl;

    void Awake()
    {
        pauseMenuControl = pauseMenuObject.GetComponent<PauseMenuControl>();
        textScore.text = "Score: " + ((int)playerRigidbody.position.y);

        //DeathReasonDecision();
    }

    /*private void DeathReasonDecision()
    {
        Controler controler = playerObject.GetComponent<Controler>();
        if()
        textDeathReason
    }*/

    public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenuTransferXD()
    {
        pauseMenuControl.MainMenuTransfer();
    }

    public void QuitGameXD()
    {
        pauseMenuControl.QuitGame();
    }
}
