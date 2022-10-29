using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimControl : MonoBehaviour
{
    public GameObject ballObject;
    public Transform firePoint;
    public float maxFireForce = 20f;
    public bool useMouseDistanceFromPlayer = false;
    public float maxMousePowerDistance = 3f;
    public Animator portalAnimation;

    private Vector2 aimDirection;
    private Controler controler;

    private void Start()
    {
        controler = GameObject.FindObjectOfType<Controler>();
    }

    public void Fire()
    {
        aimDirection = controler.getAimDirection();

        portalAnimation.SetTrigger("Shoot");

        float currentFireForce = maxFireForce;
        if (aimDirection.magnitude < maxMousePowerDistance && useMouseDistanceFromPlayer)
            currentFireForce = maxFireForce * (aimDirection.magnitude / maxMousePowerDistance);

        GameObject ball = Instantiate(ballObject, firePoint.position, firePoint.rotation);
        ball.GetComponent<Rigidbody2D>().AddForce(aimDirection.normalized * currentFireForce, ForceMode2D.Impulse);

        portalAnimation.SetTrigger("NormalPortal");
    }
}
