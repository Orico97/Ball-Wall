using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombGen : MonoBehaviour
{
    public float bombHeight = 100f;
    public float risingHeight = 3f;
    public float bombPlayerHeightDiff = 30f;
    public int maxBombs = 10;
    public Transform playerTransform;
    public Transform rightWall;
    public Transform leftWall;
    public GameObject bombObject;
    public GameObject obstacleObject;
    public GameObject destructableWallObject;


    private int numOfBombsCreated = 1;
    private float wallDistance;
    private Vector3 bombPosition;
    private Queue<Object> ObstaclesList = new Queue<Object>();
    private int ObstaclesListSize = 0;

    void Start()
    {
        wallDistance = rightWall.position.x - leftWall.position.x - 1;
    }

    void FixedUpdate()
    {
        if(playerTransform.position.y > bombHeight)
        {
            bombHeight += risingHeight;
            bombPosition.y = playerTransform.position.y + bombPlayerHeightDiff;

            for (int i = 0; i < numOfBombsCreated; i++)
            {
                bombPosition.x = leftWall.position.x + Random.Range(1, wallDistance);
                Instantiate(bombObject, bombPosition, Quaternion.identity);

                bombPosition.x = leftWall.position.x + Random.Range(1, wallDistance);
                ObstaclesList.Enqueue( Instantiate(obstacleObject, bombPosition, Quaternion.identity) );
                ObstaclesListSize++;

                bombPosition.x = leftWall.position.x + Random.Range(1, wallDistance);
                ObstaclesList.Enqueue(Instantiate(destructableWallObject, bombPosition, Quaternion.identity));
                ObstaclesListSize++;
            }

            if (risingHeight > 1)
                risingHeight -= 0.03f;
            if(ObstaclesListSize > 50)
            {
                for (int i = 0; i < numOfBombsCreated * 2; i++)
                {
                    Destroy(ObstaclesList.Dequeue());
                }
            }
        }
    }
}
