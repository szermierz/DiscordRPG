using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Room
{
    public Vector2 position { get; set; }
    public Vector2 size { get; set; }
}

public class LevelGenerator : MonoBehaviourEx
{
    public int levelSize = 100;
    public int minRoomSize = 3;
    public int maxRoomSize = 15;
    public GameObject wallTile;
    public GameObject floorTile;
    public GameObject map;
    public GameObject debugCamera;
    public List<Room> rooms = new List<Room>();

    private void MoveDebugCamera()
    {
        debugCamera.transform.SetPositionAndRotation(new Vector3(levelSize / 2, levelSize / 2, -2 * (levelSize)), Quaternion.identity);
    }

    void Start()
    {
        MoveDebugCamera();
        CreateRooms();
    }

    private void CreateRooms()
    {
        Vector2 currentPosition = Vector2.zero;
        float maxY = 0;
        while(currentPosition.y < levelSize)
        {
            if(currentPosition != Vector2.zero)
                currentPosition = new Vector2(0, maxY);

            while(currentPosition.x < levelSize)
            {
                Vector2 newSize = new Vector2(Random.Range(minRoomSize, maxRoomSize), Random.Range(minRoomSize, maxRoomSize));
                CreateRoom(currentPosition, newSize);

                currentPosition.x += newSize.x;
                maxY = Mathf.Max(maxY, currentPosition.y + newSize.y);
            }
        }
    }

    private void CreateRoom(Vector2 position, Vector2 size)
    {
        for(float x = position.x; x <= position.x + size.x; ++x)
            for(float y = position.y; y <= position.y + size.y; ++y)
            {
                if(x == position.x || y == position.y || x == position.x + size.x || y == position.y + size.y)
                    Instantiate(wallTile, new Vector3(x, y, 0), Quaternion.identity).transform.parent = map.transform;
                else
                    Instantiate(floorTile, new Vector3(x, y, 0), Quaternion.identity).transform.parent = map.transform;
            }
        rooms.Add(new Room { position = position, size = size });
        Debug.Log($"Creating room p:{position} s: {size}");
    }
}
