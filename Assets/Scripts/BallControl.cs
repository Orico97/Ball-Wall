using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    public int ballObjectLayer = 7;
    public int floorLayer = 6;
    public int wallLayer = 8;
    public int spikesLayer = 9;
    public int playerLayer = 10;
    public float maxFallDistance = 300f;
    public float velocityToForceScale = 1;
    public Rigidbody2D ballRigidbody;

    private Vector2 YVelocity;
    private Vector2 originalPlace;
    private Controler controler;
    private System.DateTime startTime;

    private void Awake()
    {
        originalPlace = ballRigidbody.position;
        startTime = System.DateTime.UtcNow;

        controler = GameObject.FindObjectOfType<Controler>();
        YVelocity = controler.getYVelocity();

        ballRigidbody.AddForce(YVelocity * velocityToForceScale, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == wallLayer || collision.gameObject.layer == spikesLayer)
            gameObject.layer = ballObjectLayer;

        if (collision.gameObject.layer == floorLayer)
            Destroy(gameObject);

        if (collision.gameObject.layer == playerLayer)
        {
            Destroy(gameObject);
            if(gameObject.layer == ballObjectLayer)
            {
                System.TimeSpan ts = System.DateTime.UtcNow - startTime;
                controler.JumpingWithTimeDiff(ts);
            }
        }
    }

    void FixedUpdate()
    {
        if(originalPlace.y - ballRigidbody.position.y > maxFallDistance)
            Destroy(gameObject);
    }
}
