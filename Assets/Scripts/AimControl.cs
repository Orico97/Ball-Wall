using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimControl : MonoBehaviour
{
    public GameObject ballObject;
    public Transform firePoint;
    public float maxFireForce = 20f;
    public float maxMousePowerDistance = 3f;
    public Animator portalAnimation;

    private Vector2 aimDirection;
    private Controler controler;

    private void Start()
    {
        controler = GameObject.FindObjectOfType<Controler>();
        
        /*float correctedAimAngle = Mathf.Atan2(firePoint.up.y, firePoint.up.x) * Mathf.Rad2Deg - portalRotationAngleCorrection;
        float firePointUpX = Mathf.Sin(correctedAimAngle);
        float firePointUpY = Mathf.Cos(correctedAimAngle);
        firePointUpCorrected = new Vector2(firePointUpX, firePointUpY);*/
    }

    public void Fire()
    {
        aimDirection = controler.getAimDirection();

        portalAnimation.SetTrigger("Shoot");

        float currentFireForce = maxFireForce;
        if (aimDirection.magnitude < maxMousePowerDistance)
            currentFireForce = maxFireForce * (aimDirection.magnitude / maxMousePowerDistance);
        GameObject ball = Instantiate(ballObject, firePoint.position, firePoint.rotation);
        ball.GetComponent<Rigidbody2D>().AddForce(aimDirection.normalized * currentFireForce, ForceMode2D.Impulse);

        portalAnimation.SetTrigger("NormalPortal");
    }
}
