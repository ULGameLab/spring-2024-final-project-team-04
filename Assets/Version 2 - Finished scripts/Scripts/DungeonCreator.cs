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
        CreateDungeon();
    }
    
    // after a floor/ground for the room is made; decorated the room
    void decorateDungeon(Vector3 floorPosition, Vector3 floorSize)
    {
        //change to be based off room size??????
        int numberOfDecorations = 10; 

        // place decorations
        for (int i = 0; i < numberOfDecorations; i++)
        {
            // random position
            // The floors (location - half its size) to the floors (location + half its size) in the X/Z axixes
            float xPosition = UnityEngine.Random.Range(floorPosition.x - floorSize.x / 2f, floorPosition.x + floorSize.x / 2f);
            float zPosition = UnityEngine.Random.Range(floorPosition.z - floorSize.z / 2f, floorPosition.z + floorSize.z / 2f);
            Vector3 randomPosition = new Vector3(xPosition, -0.8f, zPosition);

            // Pick a random decoration
            int whatDecor = UnityEngine.Random.Range(0, 10);

            // Instantiate the decoration at the calculated position
            Instantiate(decorations[whatDecor], randomPosition, Quaternion.identity);
        }
        }

    //spawn enemies 
    void spawnBugs(Vector3 floorPosition, Vector3 floorSize)
    {
        int numberOfBugs = UnityEngine.Random.Range(1,1);
        for (int i = 0; i < numberOfBugs; i++)
        {
            // random position
            // The floors (location - half its size) to the floors (location + half its size) in the X/Z axixes
            float xPosition = UnityEngine.Random.Range(floorPosition.x - floorSize.x / 2f, floorPosition.x + floorSize.x / 2f);
            float zPosition = UnityEngine.Random.Range(floorPosition.z - floorSize.z / 2f, floorPosition.z + floorSize.z / 2f);
            Vector3 randomPosition = new Vector3(xPosition, -0.8f, zPosition);

            // Pick a random bug
            int whichBug = UnityEngine.Random.Range(0, 2);

            //create bug
            Instantiate(bugs[whichBug], randomPosition, Quaternion.identity);
            bugCount++;
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
        for (int i = 0; i < 3; i++)
        {
            AddDoors(webDoor);
        }
        foreach (var room in listOfRooms)
        {
            
            CreateFloor(room.BottomLeftAreaCorner, room.TopRightAreaCorner);
            CreateRoof(room.BottomLeftAreaCorner, room.TopRightAreaCorner);
        }
        
    }
void AddDoors(GameObject webDoor)
{
        int whichwall = UnityEngine.Random.Range(0, possibleWallHorizontalPosition.Count);
        Vector3Int doorlocation = possibleWallHorizontalPosition[whichwall];
        possibleWallVerticalPosition.RemoveAt(whichwall);

        // Instantiate the door object
        GameObject DoorObject = Instantiate(webDoor, doorlocation + new Vector3(0f, -0.8f, 0.0f), Quaternion.Euler(0f, 90f, 0f));
    

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
            Vector3 floorSize = floorObject.GetComponent<Renderer>().bounds.size;
            decorateDungeon(floorPosition, floorSize);

        //if the room is a hallway don't spawn bugs
        int Hallway = 15;
        float roomsize = floorObject.GetComponent<Renderer>().bounds.size.x + floorObject.GetComponent<Renderer>().bounds.size.z;
        if (roomsize > Hallway)
        {
            spawnBugs(floorPosition, floorSize);
        }
}

         void CreateRoof(Vector2 bottomLeftCorner, Vector2 topRightCorner)
        {
            // Calculate the position of the roof
            Vector3 roofPosition = new Vector3((bottomLeftCorner.x + topRightCorner.x) / 2f, 2f, (bottomLeftCorner.y + topRightCorner.y) / 2f);

            // Instantiate the roof prefab
            GameObject roofObject = Instantiate(roofPrefab, roofPosition, Quaternion.identity);
            roofObject.transform.localScale = new Vector3(topRightCorner.x - bottomLeftCorner.x, 0.1f, topRightCorner.y - bottomLeftCorner.y); 
        roofObject.transform.parent = transform; 
        }

}



