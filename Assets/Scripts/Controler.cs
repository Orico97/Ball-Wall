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
    public bool enableScriptGravity = true;
    public float gravityScale = 1f;
    public bool fixedJumpForce = false;
    public float maxJumpForce = 20f;
    public float maxYVelocity = 20f;
    public float lowestJumpForcePercentage = 0.6f;
    public float lowestJumpForceTimeLimit = 0.33f;
    public float maxJumpTimeLimit = 1f;
    public float spikesDistance = 25f;
    public float fireDistance = 1f;
    public int noBoostBallLayer = 3;
    public int ballObjectLayer = 7;
    public int spikesObjectLayer = 9;
    public float fireRateInSeconds = 0.5f;
    public Animator player_animation;
    public GameObject aimObject;
    public GameObject ui;
    public GameObject pauseMenuUI;
    public GameObject deathMenuUI;
    public bool stuckAtFirst = true;
    public float centerYCorrectionForPlayer = 0f;
    public float portalRotationAngleCorrection = 0f;
    public float deathDelayTime = 0.2f;

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
    private Vector2 aimDirection;

    private void Start()
    {
        player = GetComponent<Rigidbody2D>();

        if (enableScriptGravity == false)
            gravityScale = 0f;

        if (stuckAtFirst)
            player.constraints = RigidbodyConstraints2D.FreezePositionY;

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
        if ( (isGrounded() && YVelocity.y < 0) || stuckAtFirst || enableScriptGravity == false)
            YVelocity.y = 0;

        //movement:
        Vector2 userMovement = moveInput * moveSpeed * Time.fixedDeltaTime;
        if (!stuckAtFirst)
            player.MovePosition(player.position + userMovement + YVelocity * Time.fixedDeltaTime);

        //mouse stuff:
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 playerActualCenter = new Vector2(player.position.x, player.position.y + centerYCorrectionForPlayer);
        aimDirection = mousePosition - playerActualCenter;
        bool flipSpriteToRight = aimDirection.x > 0;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg + portalRotationAngleCorrection;

        //aimRigidbody.rotation = aimAngle
        aimObject.transform.rotation = Quaternion.Euler(new Vector3(0f, flipSpriteToRight ? 180f : 0f, flipSpriteToRight ? -aimAngle : aimAngle));
        aimRigidbody.position = playerActualCenter + aimDirection.normalized * fireDistance;

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
        if (collision.gameObject.layer != ballObjectLayer && collision.gameObject.layer != noBoostBallLayer && collision.gameObject.layer != 0)
        {
            YVelocity.y = 0;
        }
        if (collision.gameObject.layer == spikesObjectLayer){
            player_animation.SetTrigger("isDead");
            Invoke("Death", deathDelayTime);
        }
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
        new WaitForSeconds(1f);
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
        float precentPower = 1 - (ballRigidBody.velocity.x / aim.maxFireForce);

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
        if(stuckAtFirst)
        {
            player.constraints = RigidbodyConstraints2D.None;
            player.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        stuckAtFirst = false;
        float power = 0f;

        if(fixedJumpForce || ts.Seconds > maxJumpTimeLimit)
            power = maxJumpForce;
        else
        {
            float jumpPercentage;
            if (maxJumpTimeLimit - lowestJumpForceTimeLimit <= 0)
                jumpPercentage = 0;
            else
                jumpPercentage = ((ts.Seconds - lowestJumpForceTimeLimit) / (maxJumpTimeLimit - lowestJumpForceTimeLimit)) * (1 - lowestJumpForcePercentage) + lowestJumpForcePercentage;

            if (ts.Seconds < lowestJumpForceTimeLimit)
                power = maxJumpForce * lowestJumpForcePercentage;
            else
                power = maxJumpForce * jumpPercentage;
        }

        if (enableScriptGravity)
            YVelocity.y = power;
        else
            player.AddForce(power * Vector2.up , ForceMode2D.Impulse);

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

    public Vector2 getAimDirection()
    {
        return aimDirection;
    }

    public float getLowestJumpForceTimeLimit()
    {
        return lowestJumpForceTimeLimit;
    }

    public float getMaxJumpTimeLimit()
    {
        return maxJumpTimeLimit;
    }
}
