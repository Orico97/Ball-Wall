using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Controler : MonoBehaviour
{
    public Transform floorCheck;
    public Transform transformPlayer;
    public Transform ballCheck;
    public Transform transformSpikes;
    public LayerMask floorLayer;
    public LayerMask ballLayer;
    public LayerMask spikesLayer;
    public Rigidbody2D player;
    public Rigidbody2D rightWall;
    public Rigidbody2D leftWall;
    public Rigidbody2D spikes;
    public Rigidbody2D aimRigidbody;
    public AimControl aim;
    public float moveSpeed = 1f;
    public float gravityScale = 1f;
    public bool fixedJumpForce = false;
    public float maxJumpForce = 20f;
    public float maxYVelocity = 20f;
    public float lowestJumpForcePercentage = 0.6f;
    public float middleJumpForcePercentage = 0.8f;
    public float lowestJumpForceTimeLimit = 0.33f;
    public float middleJumpForceTimeLimit = 0.66f;
    public float maxJumpingDelay = 1f;
    public float spikesDistance = 25f;
    public float fireDistance = 1f;
    public int ballObjectLayer = 7;
    public int spikesObjectLayer = 9;
    public float fireRateInSeconds = 0.5f;
    public Animator player_animation;
    public GameObject ui;
    public GameObject pauseMenuUI;
    public GameObject deathMenuUI;
    public bool stuckAtFirst = true;

    private float horizontal;
    private float nextShotTime = 0.0f;
    private Vector2 moveInput;
    private Vector2 YVelocity;
    private Vector2 gravity;
    private Vector2 jumpVec;
    private Vector2 mousePosition;
    private ScoreManager scoreManager;
    private bool isPaused = false;
    private bool isDead = false;

    private void Start()
    {
        player = GetComponent<Rigidbody2D>();

        YVelocity.x = 0;
        YVelocity.y = 0;
        gravity.x = 0;
        gravity.y = -1 * gravityScale;
        jumpVec.x = 0;
        jumpVec.y = maxJumpForce;

        scoreManager = ui.GetComponent<ScoreManager>();
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(floorCheck.position, 0.5f, floorLayer);
    }

    private bool isBallTouching()
    {
        return Physics2D.OverlapCircle(transformPlayer.position, 0.6f, ballLayer);
    }

    private bool isSpikesTouching()
    {
        return Physics2D.OverlapCircle(floorCheck.position, 3f, spikesLayer);
    }

    void FixedUpdate()
    {
        //gravity:
        if (YVelocity.y * (-1) < maxYVelocity)
            YVelocity += gravity;
        if ( (isGrounded() && YVelocity.y < 0) || stuckAtFirst)
            YVelocity.y = 0;

        //movement:
        Vector2 userMovement = moveInput * moveSpeed * Time.fixedDeltaTime;
        if (!stuckAtFirst)
            player.MovePosition(player.position + userMovement + YVelocity * Time.fixedDeltaTime);

        //mouse stuff:
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 aimDirection = mousePosition - player.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        aimRigidbody.rotation = aimAngle;
        aimRigidbody.position = player.position + aimDirection.normalized * fireDistance;

        //jumping:
        //if (isBallTouching())
        //    Jumping();

        /*//Bottom spikes movment:
        Vector2 distance = new Vector2((-1) * player.position.x, (-1) * spikesDistance);
        if (transformPlayer.position.y - transformSpikes.position.y > spikesDistance)
            spikes.MovePosition(player.position + distance);*/

        //score:
        if (scoreManager.score < player.position.y)
            scoreManager.score = player.position.y;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != ballObjectLayer)
        {
            YVelocity.y = 0;
        }
        if (collision.gameObject.layer == spikesObjectLayer)
            Death();
    }

    void BoostAnimation()
    {
        player_animation.SetTrigger("Boost");
        new WaitForSeconds(1);
        player_animation.SetTrigger("Normal");
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        moveInput.y = 0;
    }

    void OnFire()
    {
        if (isPaused)
            return;
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + fireRateInSeconds;
            aim.Fire();
        }
    }

    void OnPause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        if (isDead)
            return;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (isDead)
            return;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Death()
    {
        isDead = true;
        deathMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    void OnStop()
    {
        if (YVelocity.y > 0)
            YVelocity.y = 0;
    }

    //--------------------- Not in use ------------------------------
    void Jumping()
    {
        float xDistance = rightWall.position.x - player.position.x;
        float maxDistance = rightWall.position.x - leftWall.position.x;
        float precentDistance = xDistance / maxDistance;

        if (xDistance < 1 || precentDistance < lowestJumpForcePercentage)
            YVelocity.y = maxJumpForce * lowestJumpForcePercentage;
        else
        {
            if (xDistance > maxDistance)
                YVelocity.y = maxJumpForce;
            else
                YVelocity.y = maxJumpForce * precentDistance;
        }
        BoostAnimation();
    }

    //--------------------- Not in use ------------------------------
    void JumpingWithVelocity(GameObject ball)
    {
        Rigidbody2D ballRigidBody = ball.GetComponent<Rigidbody2D>();
        float precentPower = 1 - (ballRigidBody.velocity.x / aim.fireForce);

        if (precentPower < lowestJumpForcePercentage)
            YVelocity.y = maxJumpForce * lowestJumpForcePercentage;
        else
        {
            if (precentPower > 1)
                YVelocity.y = maxJumpForce;
            else
                YVelocity.y = maxJumpForce * precentPower;
        }
        BoostAnimation();
    }

    public void JumpingWithTimeDiff(System.TimeSpan ts)
    {
        stuckAtFirst = false;

        if(ts.Seconds > middleJumpForceTimeLimit || fixedJumpForce)
            YVelocity.y = maxJumpForce;
        else
        {
            if(ts.Seconds < lowestJumpForceTimeLimit)
                YVelocity.y = maxJumpForce * lowestJumpForcePercentage;
            else
                YVelocity.y = maxJumpForce * middleJumpForcePercentage;
        }
        BoostAnimation();
    }



    public Vector2 getYVelocity()
    {
        return YVelocity;
    }

    public float getMaxVelocity()
    {
        return maxYVelocity;
    }

    public float getSpikesDistance()
    {
        return spikesDistance;
    }

    public float getGravity()
    {
        return gravityScale;
    }

    public void setYVelocity(float newYVelocity)
    {
        YVelocity.y = newYVelocity;
    }
}
