using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    private Switch standButton = null;
    [SerializeField]
    private GameObject standTimer = null;
    [SerializeField]
    private Switch boxButton = null;
    [SerializeField]
    private TriggerBox triggerBox = null;
    [SerializeField]
    private GameObject boxIndicator = null;
    [SerializeField]
    private LevelSwitcher levelEnd = null;

    private Stack<Vector3> roomsToDo = new Stack<Vector3>();
    private List<Vector3> placedRooms = new List<Vector3>();
    private List<Vector3> placedWalls = new List<Vector3>();
    private Stack<Vector3> reservedRooms = new Stack<Vector3>();
    private EnemySpawn[] enemySpawns = null;
    private ActivatorSpawn[] activatorSpawns = null;
    private List<Door> currentDoors = new List<Door>();
    private int roomCount = 0;
    private int maxRooms = 5;
    private bool canSpawnEnemies = false;
    private bool hasDoors = false;
    private bool hasEnd = false;
    private int stackRoomCount;

    // Start is called before the first frame update
    void Start()
    {
        stackRoomCount = maxStackPerRoom;

        // Generate a scene start
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
        Vector3 lastRoom = new Vector3();
        stackRoomCount = maxStackPerRoom;

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
            lastRoom = reservedPos;

            stackRoomCount = maxStackPerRoom;
        }

        // If an end point was not generated in a different room, generate in last loaded room
        // Issue with this as standard is that last loaded room will be next to first room
        // Due to nature of stack
        if (!hasEnd) {
            lastRoom.y += 10.0f;
            LevelSwitcher ls = Instantiate(levelEnd, lastRoom, Quaternion.identity);
            ls.TheEnd = true;
        }
    }

    private void GenerateWalls(Transform roomTrans) {
        // Array to hold this room's four wall types
        WallTypes[] walls = new WallTypes[4];
        hasDoors = false;

        // 2. For each wall, determine if [open], [closed], [door]
        for (int index = 0; index < walls.Length; index++) {
            // First wall values correspond to indices in roomWalls. Last is a special number to identify when there is no wall
            walls[index] = (WallTypes)Random.Range(0, 3);

            if (walls[index] == WallTypes.DOOR) {
                if (roomCount >= maxRooms) {
                    walls[index] = WallTypes.BLANK;
                }
                hasDoors = true;
            }
        }

        // 3. Place associated walls (none for [open])
        Vector3 wallPos = roomTrans.position;

        //////////////////////
        // The four compass directions (-X, +X, -Z, +Z) are the same, so only first commented
        //////////////////////

        // Left wall (-X)
        // If target position already filled
        if (placedRooms.Contains(roomTrans.position + (-roomTrans.right * 20.0f))) {
            Debug.Log("Room to -X");
        }
        // If target position reserved for another room
        else if (reservedRooms.Contains(roomTrans.position + (-roomTrans.right * 20.0f))) {
            InstantiateWall(WallTypes.BLANK, roomTrans.position + (-roomTrans.right * 9.5f), Quaternion.identity);
        }
        // If target position available!
        else {
            // Individual wall logic
            switch (walls[0]) {
                // Spawn blank wall
                case WallTypes.BLANK:
                    InstantiateWall(WallTypes.BLANK, roomTrans.position + (-roomTrans.right * 9.5f), Quaternion.identity);
                    break;

                // Spawn a door and reserve the target beyond it for another room
                case WallTypes.DOOR:
                    InstantiateWall(WallTypes.DOOR, roomTrans.position + (-roomTrans.right * 9.5f), Quaternion.identity);
                    reservedRooms.Push(roomTrans.position + (-roomTrans.right * 20.0f));
                    break;

                // Leave a space, this room will be expanded
                case WallTypes.NONE:
                    if (stackRoomCount > 0) {
                        // Add a room to process in -X direction
                        Vector3 pos = roomTrans.position;
                        pos += (-roomTrans.right * 20.0f);

                        // Add room beyond empty wall as next to generate, then decrease available stack
                        roomsToDo.Push(pos);
                        stackRoomCount--;
                    }
                    // Unless there is no space to expand -- close off the room
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
        // If target position already filled
        if (placedRooms.Contains(roomTrans.position + (roomTrans.right * 20.0f))) {
            Debug.Log("Room to +X");
        }
        // If target position reserved for another room
        else if (reservedRooms.Contains(roomTrans.position + (roomTrans.right * 20.0f))) {
            InstantiateWall(WallTypes.BLANK, roomTrans.position + (roomTrans.right * 9.5f), Quaternion.identity);
        }
        // If target position available!
        else {
            // Individual wall logic
            switch (walls[1]) {
                // Spawn blank wall
                case WallTypes.BLANK:
                    InstantiateWall(WallTypes.BLANK, roomTrans.position + (roomTrans.right * 9.5f), Quaternion.identity);
                    break;

                // Spawn a door and reserve the target beyond it for another room
                case WallTypes.DOOR:
                    InstantiateWall(WallTypes.DOOR, roomTrans.position + (roomTrans.right * 9.5f), Quaternion.identity);
                    reservedRooms.Push(roomTrans.position + (roomTrans.right * 20.0f));
                    break;

                // Leave a space, this room will be expanded
                case WallTypes.NONE:
                    if (stackRoomCount > 0) {
                        // Add a room to process in +X direction
                        Vector3 pos = roomTrans.position;
                        pos += (roomTrans.right * 20.0f);

                        // Add room beyond empty wall as next to generate, then decrease available stack
                        roomsToDo.Push(pos);
                        stackRoomCount--;
                    }
                    // Unless there is no space to expand -- close off the room
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
        // If target position already filled
        if (placedRooms.Contains(roomTrans.position + (-roomTrans.forward * 20.0f))) {
            Debug.Log("Room to -Z");
        }
        // If target position reserved for another room
        else if (reservedRooms.Contains(roomTrans.position + (-roomTrans.forward * 20.0f))) {
            InstantiateWall(WallTypes.BLANK, roomTrans.position + (-roomTrans.forward * 9.5f), Quaternion.Euler(0.0f, 90.0f, 0.0f));
        }
        // If target position available!
        else {
            // Individual wall logic
            switch (walls[2]) {
                // Spawn blank wall
                case WallTypes.BLANK:
                    InstantiateWall(WallTypes.BLANK, roomTrans.position + (-roomTrans.forward * 9.5f), Quaternion.Euler(0.0f, 90.0f, 0.0f));
                    break;

                // Spawn a door and reserve the target beyond it for another room
                case WallTypes.DOOR:
                    InstantiateWall(WallTypes.DOOR, roomTrans.position + (-roomTrans.forward * 9.5f), Quaternion.Euler(0.0f, 90.0f, 0.0f));
                    reservedRooms.Push(roomTrans.position + (-roomTrans.forward * 20.0f));
                    break;

                // Leave a space, this room will be expanded
                case WallTypes.NONE:
                    if (stackRoomCount > 0) {
                        // Add a room to process in +X direction
                        Vector3 pos = roomTrans.position;
                        pos += (-roomTrans.forward * 20.0f);

                        // Add room beyond empty wall as next to generate, then decrease available stack
                        roomsToDo.Push(pos);
                        stackRoomCount--;
                    }
                    // Unless there is no space to expand -- close off the room
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
        // If target position already filled
        if (placedRooms.Contains(roomTrans.position + (roomTrans.forward * 20.0f))) {
            Debug.Log("Room to +Z");
        }
        // If target position reserved for another room
        else if (reservedRooms.Contains(roomTrans.position + (-roomTrans.right * 20.0f))) {
            InstantiateWall(WallTypes.BLANK, roomTrans.position + (roomTrans.forward * 9.5f), Quaternion.Euler(0.0f, 90.0f, 0.0f));
        }
        // If target position available!
        else {
            // Individual wall logic
            switch (walls[3]) {
                // Spawn blank wall
                case WallTypes.BLANK:
                    InstantiateWall(WallTypes.BLANK, roomTrans.position + (roomTrans.forward * 9.5f), Quaternion.Euler(0.0f, 90.0f, 0.0f));
                    break;

                // Spawn a door and reserve the target beyond it for another room
                case WallTypes.DOOR:
                    InstantiateWall(WallTypes.DOOR, roomTrans.position + (roomTrans.forward * 9.5f), Quaternion.Euler(0.0f, 90.0f, 0.0f));
                    reservedRooms.Push(roomTrans.position + (roomTrans.forward * 20.0f));
                    break;

                // Leave a space, this room will be expanded
                case WallTypes.NONE:
                    if (stackRoomCount > 0) {
                        // Add a room to process in +X direction
                        Vector3 pos = roomTrans.position;
                        pos += (roomTrans.forward * 20.0f);

                        // Add room beyond empty wall as next to generate, then decrease available stack
                        roomsToDo.Push(pos);
                        stackRoomCount--;
                    }
                    // Unless there is no space to expand -- close off the room
                    else {
                        InstantiateWall(WallTypes.BLANK, roomTrans.position + (roomTrans.forward * 9.5f), Quaternion.Euler(0.0f, 90.0f, 0.0f));
                    }
                    break;

                default:
                    Debug.Log("========NO WALL========");
                    break;
            }
        }

        // Determine if this room is the room with the goal - 80% chance it is not
        if (Random.value > 0.8f && !hasEnd && canSpawnEnemies) {
            Vector3 endRoom = roomTrans.position;
            endRoom.y += 10.0f;
            LevelSwitcher ls = Instantiate(levelEnd, endRoom, Quaternion.identity);
            ls.TheEnd = true;
            hasEnd = true;
        }

        // Add placed room to list of placed room
        placedRooms.Add(roomTrans.position);
    }

    // Spawn the wall
    private void InstantiateWall(WallTypes wallType, Vector3 position, Quaternion rotation) {
        // Check there isn't already a wall there
        if (!placedWalls.Contains(position)) {
            // Instantiate the wall, and add it to the list
            GameObject wall = Instantiate(roomWalls[(int)wallType], position, rotation);
            placedWalls.Add(position);

            // If it is a door wall, isolate the Door object and add it to the list of doors
            if (wallType == WallTypes.DOOR) {
                Door[] doors = wall.GetComponentsInChildren<Door>();
                currentDoors.AddRange(doors.ToList());
            }
        }
    }

    // Use spawn points to fill generated rooms
    private void ClutterRoom() {
        // Get all spawn points
        enemySpawns = FindObjectsOfType<EnemySpawn>();
        activatorSpawns = FindObjectsOfType<ActivatorSpawn>();

        // Loop through all enemy spawn points
        for (int index = 0; index < enemySpawns.Length; index++) {
            // They each have a 50% chance of spawning an enemy - unless first room [canSpawnEnemies] or spawn is already in use
            if (Random.value > 0.5f && 
                canSpawnEnemies && 
                !enemySpawns[index].IsInUse) 
            {
                Instantiate(turret, enemySpawns[index].transform.position, enemySpawns[index].transform.rotation);

                // IsInUse prevents multiple spawn at one point. Destroy() happens a frame to late to be of use here
                enemySpawns[index].IsInUse = true;
                Destroy(enemySpawns[index].gameObject);
            }
            else {
                // IsInUse prevents multiple spawn at one point. Destroy() happens a frame to late to be of use here
                enemySpawns[index].IsInUse = true;
                Destroy(enemySpawns[index].gameObject);
            }
        }

        // If the current room hasDoors, spawn switches to control them
        if (hasDoors) {
            List<Switch> switches = new List<Switch>();
            // Determine if using a boxSwitch (0% - 50%) or a stand switch (50% - 100%)
            bool whichSwich = Random.value > 0.5f;

            // True = stand button, false = box button & box
            if (whichSwich) {

                // Loop through all spawn points
                for (int index = 0; index < activatorSpawns.Length; index++) {

                    // 20% chance of each spawn being used, if not already in use
                    if (Random.value > 0.8f && 
                        !activatorSpawns[index].IsInUse) 
                    {
                        Switch s = Instantiate(
                            standButton, 
                            activatorSpawns[index].transform.position - new Vector3(0.0f, (activatorSpawns[index].transform.localScale.y / 2.0f), 0.0f), 
                            activatorSpawns[index].transform.rotation
                        );

                        // IsInUse prevents multiple spawn at one point. Destroy() happens a frame to late to be of use here
                        activatorSpawns[index].IsInUse = true;

                        // Spawn timer and connect to stand button
                        Quaternion q = activatorSpawns[index].transform.rotation;
                        q.y -= 180.0f;
                        GameObject timer = Instantiate(
                            standTimer, 
                            activatorSpawns[index].transform.position + new Vector3(0.0f, (activatorSpawns[index].transform.localScale.y / 2.0f), 0.0f), 
                            q
                        );
                        StandButton sb = s.GetComponent<StandButton>();
                        sb.OpenTime = Random.Range(8.0f, 15.0f);
                        sb.TimerPanel = timer;

                        // Add to spawned switches for this room
                        switches.Add(s);
                    }
                    else {
                        // Add a stand switch if none was added in previous loop
                        if (switches.Count == 0 && 
                            index == activatorSpawns.Length - 1 && 
                            !activatorSpawns[index].IsInUse) 
                        {
                            Switch s = Instantiate(
                                standButton, 
                                activatorSpawns[index].transform.position - new Vector3(0.0f, (activatorSpawns[index].transform.localScale.y / 2.0f), 0.0f), 
                                activatorSpawns[index].transform.rotation
                            );

                            // IsInUse prevents multiple spawn at one point. Destroy() happens a frame to late to be of use here
                            activatorSpawns[index].IsInUse = true;

                            // Spawn timer and connect to stand button
                            Quaternion q = activatorSpawns[index].transform.rotation;
                            q.y -= 180.0f;
                            GameObject timer = Instantiate(standTimer, activatorSpawns[index].transform.position + new Vector3(0.0f, (activatorSpawns[index].transform.localScale.y / 2.0f), 0.0f), q);
                            StandButton sb = s.GetComponent<StandButton>();
                            sb.OpenTime = Random.Range(8.0f, 15.0f);
                            sb.TimerPanel = timer;

                            // Add to spawned switches for this room
                            switches.Add(s);
                        }
                        // For all un-used spawns
                        else {
                            // IsInUse prevents multiple spawn at one point. Destroy() happens a frame to late to be of use here
                            activatorSpawns[index].IsInUse = true;   
                        }
                    }
                    // Mark for destruction
                    Destroy(activatorSpawns[index].gameObject);
                }
            }
            // Box button
            else {
                if (!activatorSpawns[0].IsInUse) {
                    Switch b = Instantiate(
                        boxButton, 
                        activatorSpawns[0].transform.position - new Vector3(0.0f, (activatorSpawns[0].transform.localScale.y / 2.2f), 0.0f), 
                        activatorSpawns[0].transform.rotation
                    );

                    // IsInUse prevents multiple spawn at one point. Destroy() happens a frame to late to be of use here
                    activatorSpawns[0].IsInUse = true;

                    // Spawn the indicator and connect to the button
                    GameObject indicator = Instantiate(boxIndicator, activatorSpawns[0].transform.position + new Vector3(0.0f, (activatorSpawns[0].transform.localScale.y / 2.0f), 0.0f), activatorSpawns[0].transform.rotation);
                    BoxButton bb = b.GetComponent<BoxButton>();
                    bb.Indicator = indicator;

                    // Add to spawned switches for this room
                    switches.Add(b);
                }

                // Just spawn the triggerbox
                if (!activatorSpawns[1].IsInUse) {
                    Instantiate(triggerBox, activatorSpawns[1].transform.position, activatorSpawns[1].transform.rotation);

                    // IsInUse prevents multiple spawn at one point. Destroy() happens a frame to late to be of use here
                    activatorSpawns[1].IsInUse = true;
                }

                // Mark all spawn points for deletion
                for (int index = 0; index < activatorSpawns.Length; index++) {
                    // IsInUse prevents multiple spawn at one point. Destroy() happens a frame to late to be of use here
                    activatorSpawns[index].IsInUse = true;
                    Destroy(activatorSpawns[index].gameObject);
                }
            }

            // Loop through every door currently created
            foreach (Door door in currentDoors) {
                // Determine which group this switch is in (whole of one group necessary to open door)
                // Decided based on whether this door has previously been connected to switches
                // Allows front-and-back of door switches that work independently
                int group = 1;
                if (door.Switches.Count > 0) {
                    group = 2;
                }

                // If the switch is close enough to the door, connect it
                // A little buggy, sometimes connects to a door in a different room through a wall and can be game breaking
                // But is surprisingly uncommon
                foreach(Switch switcho in switches) {
                    if (Vector3.Distance(door.transform.position, switcho.transform.position) <= 19.0f) {
                        DoorSwitch doorSwitch;
                        doorSwitch.doorOverride = false;
                        doorSwitch.theSwitch = switcho;
                        doorSwitch.group = group;
                        door.Switches.Add(doorSwitch);
                    }
                }
            }
        }

        // Clear spawn points
        enemySpawns = new EnemySpawn[0];
        activatorSpawns = new ActivatorSpawn[0];
    }
}
