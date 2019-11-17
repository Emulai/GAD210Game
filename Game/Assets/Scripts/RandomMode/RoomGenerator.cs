using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    private enum WallTypes {
        BLANK,
        DOOR,
        NONE
    }

    [SerializeField]
    private List<GameObject> rooms = new List<GameObject>();
    [SerializeField]
    private List<GameObject> roomWalls = new List<GameObject>();
    [SerializeField]
    private int maxStackPerRoom = 3;
    [SerializeField]
    private TurretBehaviour turret = null;

    private Stack<Vector3> roomsToDo = new Stack<Vector3>();
    private List<Vector3> placedRooms = new List<Vector3>();
    private List<Vector3> placedWalls = new List<Vector3>();
    private Stack<Vector3> reservedRooms = new Stack<Vector3>();
    private EnemySpawn[] enemySpawns = null;
    private ActivatorSpawn[] activatorSpawns = null;
    private int roomCount = 0;
    private int maxRooms = 5;
    private bool canSpawnEnemies = false;

    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    private void Generate() {
        // 1. Spawn starting room
        GameObject main = Instantiate(rooms[0], new Vector3(), Quaternion.identity);

        // 2 & 3 -->
        GenerateWalls(main.transform);
        // 6. Clutter room
        ClutterRoom();

        // 4. Ignore [closed] walls

        // 5. Process [open] walls, return to 2
        while (roomsToDo.Count != 0) {
            Vector3 pos = roomsToDo.Pop();
            GameObject side = Instantiate(rooms[Random.Range(0, rooms.Count)], pos, Quaternion.identity);

            // 2 & 3 -->
            GenerateWalls(side.transform);

            // 6. Clutter room
            ClutterRoom();
        }

        roomCount++;

        // 7. Process [door] walls, return to 1
        canSpawnEnemies = true;
        while (reservedRooms.Count != 0) {
            Vector3 reservedPos = reservedRooms.Pop();
            // 1. Spawn starting room
            GameObject reservedSide = Instantiate(rooms[Random.Range(0, rooms.Count)], reservedPos, Quaternion.identity);

            // 2 & 3 -->
            GenerateWalls(reservedSide.transform);

            // 6. Clutter room
            ClutterRoom();

            // 4. Ignore [closed] walls

            // 5. Process [open] walls, return to 2
            while (roomsToDo.Count != 0) {
                Vector3 pos = roomsToDo.Pop();
                GameObject side = Instantiate(rooms[Random.Range(0, rooms.Count)], pos, Quaternion.identity);

                // 2 & 3 -->
                GenerateWalls(side.transform);

                // 6. Clutter room
                ClutterRoom();
            }

            roomCount++;
        }
    }

    private void GenerateWalls(Transform roomTrans) {
        WallTypes[] walls = new WallTypes[4];

        // 2. For each wall, determine if [open], [closed], [door]
        for (int index = 0; index < walls.Length; index++) {
            // First wall values correspond to indices in roomWalls. Last is a special number to identify when there is no wall
            walls[index] = (WallTypes)Random.Range(0, 3);

            if (walls[index] == WallTypes.DOOR) {
                if (roomCount >= maxRooms) {
                    walls[index] = WallTypes.BLANK;
                }
            }
        }

        // 3. Place associated walls (none for [open])
        Vector3 wallPos = roomTrans.position;

        // Left wall (-X)
        if (placedRooms.Contains(roomTrans.position + (-roomTrans.right * 20.0f))) {
            Debug.Log("Room to -X");
        }
        else if (reservedRooms.Contains(roomTrans.position + (-roomTrans.right * 20.0f))) {
            InstantiateWall(WallTypes.BLANK, roomTrans.position + (-roomTrans.right * 9.5f), Quaternion.identity);
        }
        else {
            switch (walls[0]) {
                case WallTypes.BLANK:
                    InstantiateWall(WallTypes.BLANK, roomTrans.position + (-roomTrans.right * 9.5f), Quaternion.identity);
                    break;

                case WallTypes.DOOR:
                    InstantiateWall(WallTypes.DOOR, roomTrans.position + (-roomTrans.right * 9.5f), Quaternion.identity);
                    reservedRooms.Push(roomTrans.position + (-roomTrans.right * 20.0f));
                    break;

                case WallTypes.NONE:
                    if (maxStackPerRoom > 0) {
                        // Add a room to process in -X direction
                        Vector3 pos = roomTrans.position;
                        pos += (-roomTrans.right * 20.0f);
                        roomsToDo.Push(pos);
                        maxStackPerRoom--;
                    }
                    else {
                        InstantiateWall(WallTypes.BLANK, roomTrans.position + (-roomTrans.right * 9.5f), Quaternion.identity);
                    }
                    break;

                default:
                    Debug.Log("========NO WALL========");
                    break;
            }
        }

        // Right wall (+X)
        if (placedRooms.Contains(roomTrans.position + (roomTrans.right * 20.0f))) {
            Debug.Log("Room to +X");
        }
        else if (reservedRooms.Contains(roomTrans.position + (roomTrans.right * 20.0f))) {
            InstantiateWall(WallTypes.BLANK, roomTrans.position + (roomTrans.right * 9.5f), Quaternion.identity);
        }
        else {
            switch (walls[1]) {
                case WallTypes.BLANK:
                    InstantiateWall(WallTypes.BLANK, roomTrans.position + (roomTrans.right * 9.5f), Quaternion.identity);
                    break;

                case WallTypes.DOOR:
                    InstantiateWall(WallTypes.DOOR, roomTrans.position + (roomTrans.right * 9.5f), Quaternion.identity);
                    reservedRooms.Push(roomTrans.position + (roomTrans.right * 20.0f));
                    break;

                case WallTypes.NONE:
                    if (maxStackPerRoom > 0) {
                        // Add a room to process in +X direction
                        Vector3 pos = roomTrans.position;
                        pos += (roomTrans.right * 20.0f);
                        roomsToDo.Push(pos);
                        maxStackPerRoom--;
                    }
                    else {
                        InstantiateWall(WallTypes.BLANK, roomTrans.position + (roomTrans.right * 9.5f), Quaternion.identity);
                    }
                    break;

                default:
                    Debug.Log("========NO WALL========");
                    break;
            }
        }

        // Back wall (-Z)
        if (placedRooms.Contains(roomTrans.position + (-roomTrans.forward * 20.0f))) {
            Debug.Log("Room to -Z");
        }
        else if (reservedRooms.Contains(roomTrans.position + (-roomTrans.forward * 20.0f))) {
            InstantiateWall(WallTypes.BLANK, roomTrans.position + (-roomTrans.forward * 9.5f), Quaternion.Euler(0.0f, 90.0f, 0.0f));
        }
        else {
            switch (walls[2]) {
                case WallTypes.BLANK:
                    InstantiateWall(WallTypes.BLANK, roomTrans.position + (-roomTrans.forward * 9.5f), Quaternion.Euler(0.0f, 90.0f, 0.0f));
                    break;

                case WallTypes.DOOR:
                    InstantiateWall(WallTypes.DOOR, roomTrans.position + (-roomTrans.forward * 9.5f), Quaternion.Euler(0.0f, 90.0f, 0.0f));
                    reservedRooms.Push(roomTrans.position + (-roomTrans.forward * 20.0f));
                    break;

                case WallTypes.NONE:
                    if (maxStackPerRoom > 0) {
                        // Add a room to process in +X direction
                        Vector3 pos = roomTrans.position;
                        pos += (-roomTrans.forward * 20.0f);
                        roomsToDo.Push(pos);
                        maxStackPerRoom--;
                    }
                    else {
                        InstantiateWall(WallTypes.BLANK, roomTrans.position + (-roomTrans.forward * 9.5f), Quaternion.Euler(0.0f, 90.0f, 0.0f));
                    }
                    break;

                default:
                    Debug.Log("========NO WALL========");
                    break;
            }
        }

        // Front wall (+Z)
        if (placedRooms.Contains(roomTrans.position + (roomTrans.forward * 20.0f))) {
            Debug.Log("Room to +Z");
        }
        else if (reservedRooms.Contains(roomTrans.position + (-roomTrans.right * 20.0f))) {
            InstantiateWall(WallTypes.BLANK, roomTrans.position + (roomTrans.forward * 9.5f), Quaternion.Euler(0.0f, 90.0f, 0.0f));
        }
        else {
            switch (walls[3]) {
                case WallTypes.BLANK:
                    InstantiateWall(WallTypes.BLANK, roomTrans.position + (roomTrans.forward * 9.5f), Quaternion.Euler(0.0f, 90.0f, 0.0f));
                    break;

                case WallTypes.DOOR:
                    InstantiateWall(WallTypes.DOOR, roomTrans.position + (roomTrans.forward * 9.5f), Quaternion.Euler(0.0f, 90.0f, 0.0f));
                    reservedRooms.Push(roomTrans.position + (roomTrans.forward * 20.0f));
                    break;

                case WallTypes.NONE:
                    if (maxStackPerRoom > 0) {
                        // Add a room to process in +X direction
                        Vector3 pos = roomTrans.position;
                        pos += (roomTrans.forward * 20.0f);
                        roomsToDo.Push(pos);
                        maxStackPerRoom--;
                    }
                    else {
                        InstantiateWall(WallTypes.BLANK, roomTrans.position + (roomTrans.forward * 9.5f), Quaternion.Euler(0.0f, 90.0f, 0.0f));
                    }
                    break;

                default:
                    Debug.Log("========NO WALL========");
                    break;
            }
        }

        placedRooms.Add(roomTrans.position);
    }

    private void InstantiateWall(WallTypes wallType, Vector3 position, Quaternion rotation) {
        if (!placedWalls.Contains(position)) {
            Instantiate(roomWalls[(int)wallType], position, rotation);
            placedWalls.Add(position);
        }
    }

    private void ClutterRoom() {
        enemySpawns = FindObjectsOfType<EnemySpawn>();
        activatorSpawns = FindObjectsOfType<ActivatorSpawn>();

        for (int index = 0; index < enemySpawns.Length; index++) {
            if (Random.value > 0.5f && canSpawnEnemies && !enemySpawns[index].IsInUse) {
                Instantiate(turret, enemySpawns[index].transform.position, enemySpawns[index].transform.rotation);
                enemySpawns[index].IsInUse = true;
                Destroy(enemySpawns[index].gameObject);
            }
            else {
                enemySpawns[index].IsInUse = true;
                Destroy(enemySpawns[index].gameObject);
            }
        }

        enemySpawns = new EnemySpawn[0];
        activatorSpawns = new ActivatorSpawn[0];
    }
}
