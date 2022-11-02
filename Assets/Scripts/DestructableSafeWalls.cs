using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableSafeWalls : MonoBehaviour
{
    public int noBoostballLayer = 3;
    public int ballObjectLayer = 7;
    public int playerLayer = 10;

    private Controler controler;

    private void Start()
    {
        controler = GameObject.FindObjectOfType<Controler>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            Destroy(gameObject);
            controler.setYVelocity(0.0f);
        }
        else
            Destroy(gameObject);
    }
}
