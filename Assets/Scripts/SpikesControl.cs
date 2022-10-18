using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesControl : MonoBehaviour
{
    public int floorLayer = 6;
    public int playerLayer = 10;
    public Rigidbody2D spikesRigidbody;
    public float maxFallDistance = 50f;

    private Vector2 originalPlace;
    private Controler controler;

    private void Start()
    {
        originalPlace = spikesRigidbody.position;
        controler = GameObject.FindObjectOfType<Controler>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == floorLayer || collision.gameObject.layer == playerLayer)
        {
            Destroy(gameObject);
            if (collision.gameObject.layer == playerLayer)
                controler.Death();
        }
    }

    void FixedUpdate()
    {
        if (originalPlace.y - spikesRigidbody.position.y > maxFallDistance)
            Destroy(gameObject);
    }
}
