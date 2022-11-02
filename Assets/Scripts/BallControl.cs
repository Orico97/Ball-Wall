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
    public float explodeAfterXSeconds = 10f;
    public bool affectedByPlayerYVelocity = true;
    public Renderer ballRender;
    public Color startColor;
    public Color endColor;
    public float colorChangeSpeed = 1f;

    private Vector2 YVelocity;
    private Vector2 originalPlace;
    private Controler controler;
    private System.DateTime startTime;
    private System.DateTime wallHitTime;
    private float startTimeFloat;
    private float wallHitTimeFloat;
    private int collisonCount = 0;

    private void Awake()
    {
        originalPlace = ballRigidbody.position;
        startTime = System.DateTime.UtcNow;
        startTimeFloat = Time.time;

        controler = GameObject.FindObjectOfType<Controler>();
        YVelocity = controler.getYVelocity();
        Vector2 aimDirection = controler.getAimDirection();
        if(aimDirection.x > 0)
            gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0));

        if (!affectedByPlayerYVelocity)
            velocityToForceScale = 0f;
        ballRigidbody.AddForce(YVelocity * velocityToForceScale, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisonCount++;

        if ( (collision.gameObject.layer == wallLayer || collision.gameObject.layer == spikesLayer) && (gameObject.layer != ballObjectLayer) )
        {
            wallHitTime = System.DateTime.UtcNow;
            wallHitTimeFloat = Time.time;
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
                System.TimeSpan ts = System.DateTime.UtcNow - wallHitTime;
                controler.JumpingWithTimeDiff(ts);
            }
        }
    }

    void FixedUpdate()
    {
        if(originalPlace.y - ballRigidbody.position.y > maxFallDistance || explodeAfterXSeconds + startTimeFloat < Time.time)
            Destroy(gameObject);

        ballRigidbody.rotation = Mathf.Atan2(ballRigidbody.velocity.y, ballRigidbody.velocity.x) * Mathf.Rad2Deg + 90f;
        
        if(gameObject.layer == ballObjectLayer)
            changeColor();
    }

    private void changeColor()
    {
        float changeAmount = colorChangeSpeed * (Time.time - wallHitTimeFloat);
        ballRender.material.color = Color.Lerp(startColor, endColor, changeAmount);
    }
}
