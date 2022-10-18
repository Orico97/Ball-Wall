using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneHitBoostWall : MonoBehaviour
{
    public int defaultLayer = 0;
    public int noBoostballLayer = 3;
    public int ballObjectLayer = 7;

    private bool touched = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.layer == noBoostballLayer || collision.gameObject.layer == ballObjectLayer) && !touched)
        {
            gameObject.layer = defaultLayer;
            gameObject.GetComponent<SpriteRenderer>().color = Color.black;
            touched = true;
        }
    }
}
