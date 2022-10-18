using UnityEngine;

public class CamMov : MonoBehaviour
{
    public Transform player;
    public float cameraHeightFromPlayer;

    // Update is called once per frame
    void Update ()
    {
        transform.position = new Vector3(player.position.x, player.position.y + cameraHeightFromPlayer, transform.position.z);
    }
}
