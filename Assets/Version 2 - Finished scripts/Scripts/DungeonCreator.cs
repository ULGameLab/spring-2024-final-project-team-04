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
    public GameObject wallVertical, wallHorizontal;
    public GameObject floorPrefab;
    public GameObject roofPrefab;
    public GameObject[] decorations;
    List<Vector3Int> possibleDoorVerticalPosition;
    List<Vector3Int> possibleDoorHorizontalPosition;
    List<Vector3Int> possibleWallHorizontalPosition;
    List<Vector3Int> possibleWallVerticalPosition;
    // Start is called before the first frame update
    void Start()
    {
        CreateDungeon();
    }

    //get list of rooms
    void decorateDungeon(List<Node> listOfRooms)
    {
        //destory old game objects 
        //This wont destroy objects unless game is running
        foreach (var gameObj in GameObject.FindGameObjectsWithTag("Decor"))
        {
            Destroy(gameObj);
        }

        // Iterate through each room
        foreach (var room in listOfRooms)//only fills some rooms!!!!!!!!!
        {
            for (int i = 0; i < 1; i++)
            {
                //get outline of room
                float roomLength = Mathf.Abs(room.TopRightAreaCorner.x - room.BottomLeftAreaCorner.x);
                float roomWidth = Mathf.Abs(room.TopRightAreaCorner.x - room.BottomLeftAreaCorner.x);

                // Get size of the decoration 
                int whatDEcor = UnityEngine.Random.Range(0, 10);
                Vector3 decorationSize = decorations[whatDEcor].GetComponent<Renderer>().bounds.size;

                // Calculate random position within the room
                float xPosition = UnityEngine.Random.Range(room.BottomLeftAreaCorner.x, room.TopRightAreaCorner.x);
                float zPosition = UnityEngine.Random.Range(room.BottomLeftAreaCorner.x, room.TopRightAreaCorner.x);
                Vector3 randomPosition = new Vector3(xPosition, -0.886f, zPosition);

                // Offset position by half the decoration size so it's centered
                randomPosition += new Vector3(-decorationSize.x / 2, 0, -decorationSize.z / 2);
                // Debug.Log(roomLength);
                // Debug.Log(roomWidth);
                
                Instantiate(decorations[whatDEcor], randomPosition, Quaternion.identity);
            }
        }
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
        foreach (var room in listOfRooms)
        {
            
            CreateFloor(room.BottomLeftAreaCorner, room.TopRightAreaCorner);
            CreateRoof(room.BottomLeftAreaCorner, room.TopRightAreaCorner);
        }
        decorateDungeon(listOfRooms);
    }

         void CreateWalls(GameObject wallParent)
        {
            foreach (var wallPosition in possibleWallHorizontalPosition)
            {
                CreateWall(wallParent, wallPosition, wallHorizontal);
        }
            foreach (var wallPosition in possibleWallVerticalPosition)
            {
                CreateWall(wallParent, wallPosition, wallVertical);
        }
        }

    void CreateWall(GameObject wallParent, Vector3Int wallPosition, GameObject wallPrefab)
    {
        // Instantiate the wall prefab
        GameObject wallObject = Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);

        // Set the position of the wall
        wallObject.transform.position = wallPosition;

        // Apply fixed rotation based on wall type//change with prefabs as needed!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        if (wallPrefab == wallHorizontal)
        {
            wallObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else if (wallPrefab == wallVertical)
        {
            wallObject.transform.rotation = Quaternion.identity; // No rotation needed for vertical walls
        }
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
            if (wallList.Contains(point)) {
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
        Vector3 floorPosition = new Vector3((bottomLeftCorner.x + topRightCorner.x) / 2f, -0.9f, (bottomLeftCorner.y + topRightCorner.y) / 2f);



        // Instantiate the floor prefab
        GameObject floorObject = Instantiate(floorPrefab, floorPosition, Quaternion.identity);
            floorObject.transform.localScale = new Vector3(topRightCorner.x - bottomLeftCorner.x, 0.1f, topRightCorner.y - bottomLeftCorner.y);
            floorObject.transform.parent = transform; 
        }

         void CreateRoof(Vector2 bottomLeftCorner, Vector2 topRightCorner)
        {
            // Calculate the position of the roof
            Vector3 roofPosition = new Vector3((bottomLeftCorner.x + topRightCorner.x) / 2f, 0.9f, (bottomLeftCorner.y + topRightCorner.y) / 2f);

            // Instantiate the roof prefab
            GameObject roofObject = Instantiate(roofPrefab, roofPosition, Quaternion.identity);
            roofObject.transform.localScale = new Vector3(topRightCorner.x - bottomLeftCorner.x, 0.1f, topRightCorner.y - bottomLeftCorner.y); 
        roofObject.transform.parent = transform; 
        }


    }



