using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimControl : MonoBehaviour
{
    public GameObject ballObject;
    public Transform firePoint;
    public float fireForce = 20f;
    public Animator portalAnimation;

    public void Fire()
    {
        GameObject ball = Instantiate(ballObject, firePoint.position, firePoint.rotation);
        ball.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);

        FireAnimation();
    }

    void FireAnimation()
    {
        portalAnimation.SetTrigger("Shoot");
        new WaitForSeconds(1);
        portalAnimation.SetTrigger("NormalPortal");
    }
}
