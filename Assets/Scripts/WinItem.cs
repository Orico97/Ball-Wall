using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinItem : MonoBehaviour
{
    public int playerLayer = 10;
    public GameObject winScreenUI;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            Destroy(gameObject);
            winScreenUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
