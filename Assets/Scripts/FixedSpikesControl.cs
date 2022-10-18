using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedSpikesControl : MonoBehaviour
{
    public int playerLayer = 10;
    public bool destroyOnContact = true;

    private Controler controler;

    private void Start()
    {
        controler = GameObject.FindObjectOfType<Controler>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            if(destroyOnContact)
                Destroy(gameObject);
            controler.Death();
        }
    }
}
