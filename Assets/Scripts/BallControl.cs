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
    public Animator ballAnimation;
    public int maxCollisonNum = 20;
    public bool affectedByPlayerYVelocity = true;
    public SpriteRenderer ballRender;

    private Vector2 YVelocity;
    private Vector2 originalPlace;
    private Controler controler;
    private System.DateTime startTime;
    private float nextColorChangeTime = 0f;
    private int collisonCount = 0;
    private int countColorChange = 0;
    private Vector4 addColor;

    private void Awake()
    {
        originalPlace = ballRigidbody.position;
        startTime = System.DateTime.UtcNow;

        controler = GameObject.FindObjectOfType<Controler>();
        YVelocity = controler.getYVelocity();

        if (!affectedByPlayerYVelocity)
            velocityToForceScale = 0f;
        ballRigidbody.AddForce(YVelocity * velocityToForceScale, ForceMode2D.Impulse);

        addColor = new Vector4(1, 0, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisonCount++;

        if ( (collision.gameObject.layer == wallLayer || collision.gameObject.layer == spikesLayer) && (gameObject.layer != ballObjectLayer) )
        {
            nextColorChangeTime = Time.time + 0.1f;
            gameObject.layer = ballObjectLayer;
            ballAnimation.SetTrigger("WallHit");
            ballAnimation.SetTrigger("ConstantFlame");
        }

        if (collision.gameObject.layer == floorLayer || collisonCount > maxCollisonNum)
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

        ballRigidbody.rotation = Mathf.Atan2(ballRigidbody.velocity.y, ballRigidbody.velocity.x) * Mathf.Rad2Deg + 90f;
        
        /*if(gameObject.layer == ballObjectLayer && Time.time > nextColorChangeTime && countColorChange < 10)
            changeColor();*/
    }

    private void changeColor()
    {
        nextColorChangeTime = Time.time + 0.1f;
        countColorChange++;
        Vector4 currentColor = ballRender.material.color;
        currentColor += addColor;
        ballRender.material.color = currentColor;
    }
}
