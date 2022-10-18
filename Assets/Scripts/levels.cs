using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levels : MonoBehaviour
{
    [SerializeField] private Transform FirstWalls;
    [SerializeField] private Transform player;
    public const float wallsMaxSpawnDistanceFromPlayer = 200f;

    private Queue<Object> wallList = new Queue<Object>();
    private int wallListSize = 0;
    private int WallListMaxSize = 4;
    private Vector3 lastEndPosition;

    private void Awake()
    {
        lastEndPosition = FirstWalls.Find("End").position;
    }

    private void Update()
    {
        if (Vector3.Distance(player.position, lastEndPosition) < wallsMaxSpawnDistanceFromPlayer)
            SpawnWallsAuto();
    }

    private void SpawnWallsAuto()
    {
        Transform result = SpawnWalls(lastEndPosition);
        lastEndPosition = result.Find("End").position;
    }

    private Transform SpawnWalls(Vector3 spawnPoint)
    {
        Object result = Instantiate(FirstWalls, spawnPoint, Quaternion.identity);
        wallList.Enqueue(result);
        wallListSize++;

        if (wallListSize > WallListMaxSize)
            Destroy(wallList.Dequeue());

        return (Transform)result;
    }
}
