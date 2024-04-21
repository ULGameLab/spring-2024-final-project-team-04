using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    public int dungeonWidth, dungeonLength;
    public int roomWidthMin, roomLengthMin;
    public int maxIterations;
    public int corridorWidth;
    [Range(0.0f, 0.3f)]
    public float roomBottomCornerModifier;
    [Range(0.7f, 1.0f)]
    public float roomTopCornerMidifier;
    [Range(0, 2)]
    public int roomOffset;
    //types of walls
    public GameObject wallVertical, wallHorizontal, webDoor;
    public GameObject floorPrefab;
    public GameObject roofPrefab;
    public GameObject[] decorations;
    //bugs
    public GameObject[] bugs;
    public static int bugCount = 0;
    List<Vector3Int> possibleDoorVerticalPosition;
    List<Vector3Int> possibleDoorHorizontalPosition;
    List<Vector3Int> possibleWallHorizontalPosition;
    List<Vector3Int> possibleWallVerticalPosition;
    // Start is called before the first frame update
    void Start()
    {
        //reset static variable
        bugCount = 0;
        CreateDungeon();
    }

    // after a floor/ground for the room is made; decorated the room
    void decorateDungeon(Vector3 floorPosition)
    {
        // Pick a random decoration
        int whatDecor = UnityEngine.Random.Range(0, 10);

        // Instantiate the decoration at the calculated position
        Instantiate(decorations[whatDecor], floorPosition, Quaternion.identity);
    }

    //spawn enemies 
    void spawnBugs(Vector3 floorPosition)
    {
        // Pick a random bug
        int whichBug = UnityEngine.Random.Range(0, 2);

        //create bug
        Instantiate(bugs[whichBug], floorPosition, Quaternion.identity);
        bugCount++;
    }


    public void CreateDungeon()
    {
        DestroyAllChildren();
        DugeonGenerator generator = new DugeonGenerator(dungeonWidth, dungeonLength);
        var listOfRooms = generator.CalculateDungeon(maxIterations,
            roomWidthMin,
            roomLengthMin,
            roomBottomCornerModifier,
            roomTopCornerMidifier,
            roomOffset,
            corridorWidth);
        GameObject wallParent = new GameObject("WallParent");
        wallParent.transform.parent = transform;
        possibleDoorVerticalPosition = new List<Vector3Int>();
        possibleDoorHorizontalPosition = new List<Vector3Int>();
        possibleWallHorizontalPosition = new List<Vector3Int>();
        possibleWallVerticalPosition = new List<Vector3Int>();
        for (int i = 0; i < listOfRooms.Count; i++)
        {
            CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner);
        }
        CreateWalls(wallParent);
        for (int i = 0; i < 3; i++)
        {
            int whichwall = UnityEngine.Random.Range(0, possibleWallHorizontalPosition.Count);
            Vector3Int doorlocation = possibleWallHorizontalPosition[whichwall];
            possibleWallVerticalPosition.Remove(doorlocation);

            // Instantiate the door object
            GameObject DoorObject = Instantiate(webDoor, doorlocation + new Vector3(0f, -1.4f, 0.0f), Quaternion.Euler(0f, 90f, 0f));
        }
        foreach (var room in listOfRooms)
        {

            CreateFloor(room.BottomLeftAreaCorner, room.TopRightAreaCorner);
            CreateRoof(room.BottomLeftAreaCorner, room.TopRightAreaCorner);
        }

    }
    void CreateWalls(GameObject wallParent)
    {

        foreach (var wallPosition in possibleWallHorizontalPosition)
        {

            CreateWallHorizontal(wallParent, wallPosition, wallHorizontal);
        }
        foreach (var wallPosition in possibleWallVerticalPosition)
        {

            CreateWallVertical(wallParent, wallPosition, wallVertical);
        }
    }

    void CreateWallVertical(GameObject wallParent, Vector3Int wallPosition, GameObject wallPrefab)
    {
        // Instantiate the wall prefab
        GameObject wallObject = Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);

        // Set the position of the wall

        wallObject.transform.position = wallPosition;
        wallObject.transform.position = new Vector3(wallPosition.x, wallPosition.y, wallPosition.z + 0.5f);

    }

    void CreateWallHorizontal(GameObject wallParent, Vector3Int wallPosition, GameObject wallPrefab)
    {

        // Instantiate the wall prefab
        GameObject wallObject = Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);

        // Set the position of the wall
        wallObject.transform.position = wallPosition;
        wallObject.transform.position = new Vector3(wallPosition.x + 0.5f, wallPosition.y, wallPosition.z);

        wallObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);


    }


    void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0, topRightCorner.y);

        Vector3[] vertices = new Vector3[]
        {
            topLeftV,
            topRightV,
            bottomLeftV,
            bottomRightV
        };

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
        {
            0,
            1,
            2,
            2,
            1,
            3
        };
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        GameObject dungeonFloor = new GameObject("Mesh" + bottomLeftCorner, typeof(MeshFilter));


        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.transform.parent = transform;

        for (int row = (int)bottomLeftV.x; row < (int)bottomRightV.x; row++)
        {
            var wallPosition = new Vector3(row, 0, bottomLeftV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int row = (int)topLeftV.x; row < (int)topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row, 0, topRightV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int col = (int)bottomLeftV.z; col < (int)topLeftV.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
        for (int col = (int)bottomRightV.z; col < (int)topRightV.z; col++)
        {
            var wallPosition = new Vector3(bottomRightV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
    }

    void AddWallPositionToList(Vector3 wallPosition, List<Vector3Int> wallList, List<Vector3Int> doorList)
    {
        Vector3Int point = Vector3Int.CeilToInt(wallPosition);
        if (wallList.Contains(point))
        {
            doorList.Add(point);
            wallList.Remove(point);
        }
        else
        {
            wallList.Add(point);
        }
    }

    void DestroyAllChildren()
    {
        while (transform.childCount != 0)
        {
            foreach (Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }

    void CreateFloor(Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        // Calculate the dimensions of each floor tile
        float tileSizeY = floorPrefab.transform.localScale.y; // Get the X size of the floor prefab
        float tileSizeZ = floorPrefab.transform.localScale.z; // Get the Z size of the floor prefab

        // Calculate the number of tiles in each direction
        int numTilesX = Mathf.FloorToInt((topRightCorner.x - bottomLeftCorner.x) / tileSizeY);
        int numTilesY = Mathf.FloorToInt((topRightCorner.y - bottomLeftCorner.y) / tileSizeZ);

        // Calculate the starting position of the floor tiles
        Vector3 startPos = new Vector3(bottomLeftCorner.x, -1.0f, bottomLeftCorner.y) + new Vector3(tileSizeY / 2f, 0f, tileSizeZ / 2f);

        // Loop through each grid cell and instantiate a floor tile
        for (int x = 0; x < numTilesX; x++)
        {
            for (int y = 0; y < numTilesY; y++)
            {
                // Calculate the position for the current tile
                Vector3 tilePosition = startPos + new Vector3(x * tileSizeY, -0.4f, y * tileSizeZ);

                // Instantiate the floor tile with rotation
                GameObject floorTile = Instantiate(floorPrefab, tilePosition, Quaternion.Euler(0f, 0f, 90f));
                floorTile.transform.parent = transform;
                //randomly add decoration 
                int randomNum = UnityEngine.Random.Range(0, 20);
                if (randomNum == 14)
                {
                    decorateDungeon(floorTile.transform.position);
                }

                //randomly add decoration 
                int randomNumber = UnityEngine.Random.Range(0, 250);
                if (randomNumber == 20)
                {
                    spawnBugs(floorTile.transform.position);
                }
            }
        }

    }

    // same as floor but above the player
    //didn't rename floor varables because i didnt fell like it and the do the same thing just above the player 
    void CreateRoof(Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        // Calculate the dimensions of each floor tile
        float tileSizeY = roofPrefab.transform.localScale.y; // Get the X size of the floor prefab
        float tileSizeZ = roofPrefab.transform.localScale.z; // Get the Z size of the floor prefab

        // Calculate the number of tiles in each direction
        int numTilesX = Mathf.FloorToInt((topRightCorner.x - bottomLeftCorner.x) / tileSizeY);
        int numTilesY = Mathf.FloorToInt((topRightCorner.y - bottomLeftCorner.y) / tileSizeZ);

        // Calculate the starting position of the floor tiles
        Vector3 startPos = new Vector3(bottomLeftCorner.x, -1.0f, bottomLeftCorner.y) + new Vector3(tileSizeY / 2f, 0f, tileSizeZ / 2f);

        // Loop through each grid cell and instantiate a floor tile
        for (int x = 0; x < numTilesX; x++)
        {
            for (int y = 0; y < numTilesY; y++)
            {
                // Calculate the position for the current tile
                Vector3 tilePosition = startPos + new Vector3(x * tileSizeY, 2.4f, y * tileSizeZ);

                // Instantiate the floor tile with rotation
                GameObject floorTile = Instantiate(floorPrefab, tilePosition, Quaternion.Euler(0f, 0f, 90f));
                floorTile.transform.parent = transform;
            }
        }

    }
}



