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
        portalAnimation.SetTrigger("Shoot");

        GameObject ball = Instantiate(ballObject, firePoint.position, firePoint.rotation);
        ball.GetComponent<Rigidbody2D>().AddForce(firePoint.up * fireForce, ForceMode2D.Impulse);

        portalAnimation.SetTrigger("NormalPortal");
    }
}
