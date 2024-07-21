using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainManager : Singleton<TerrainManager> {
    public List<GameObject> terrains;

    public TerrainChecker terrainChecker;
    public float terrainSize = 20f; // Tamaño del terreno (asumimos cuadrado)

    public void OnDoorClicked(Door clickedDoor) {
        terrainChecker.transform.position = clickedDoor.terrain.transform.position;
        Vector3 targetPosition = CalculatePosition(clickedDoor, terrainChecker.gameObject);
        List<CollisionInfo> collisions = terrainChecker.Check(targetPosition);
        //// Instancia el nuevo terreno
        var terrainPrefab = GetTerrain(collisions);
        if(terrainPrefab == null ) {
            return;
        }
        GameObject newTerrain = Instantiate(terrainPrefab, clickedDoor.terrain.transform.position, clickedDoor.transform.rotation, transform);
        //// Ajusta la posición del nuevo terreno para que se alinee con la puerta seleccionada
        targetPosition = CalculatePosition(clickedDoor, newTerrain);
        newTerrain.transform.position = targetPosition;
        int doorCount = 0;
        foreach (CollisionInfo collision in collisions) {
            if (collision.objectType == Door.DoorType.Entrance || collision.objectType == Door.DoorType.Exit) {
                doorCount++;
            }
        }
        //// Calcula la rotación necesaria para alinear las puertas
        if (false && newTerrain.GetComponent<BlockTerrain>().GetActiveDoorsCount() == 1) {
            StartCoroutine(CalculateRotationCoroutine(clickedDoor, newTerrain, doorCount));
        } else {
            CalculateRotation(clickedDoor, newTerrain, doorCount);
            var newExitDoor = GetExitDoor(clickedDoor, newTerrain);

            newExitDoor.point.points.Add(clickedDoor.point);
            //Debug.Log("Entrando a DeactiveOcupedDoors.---------------------", newTerrain);

            StartCoroutine(DeactiveDoors(newTerrain.GetComponent<BlockTerrain>(), clickedDoor.terrain));
        }

    }
    public IEnumerator DeactiveDoors(BlockTerrain newTerrain, BlockTerrain lastTerrain) {
        yield return new WaitForSeconds(.05f);
        //lastTerrain.DeactiveOcupedDoors();
        newTerrain.DeactiveOcupedDoors();
        newTerrain.DeactiveTouchedOcupedDoors();
        var spawners = FindObjectsByType<Spawner>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID);
        foreach (var spawner in spawners) {
            spawner.Spawn();
        }
    }
    private GameObject GetTerrain(List<CollisionInfo> collisions) {
        // Lógica para decidir qué tipo de terreno instanciar
        int doorCount = 0;
        int wallCount = 0;
        List<Vector3> doorDirections = new List<Vector3>();
        List<Vector3> wallDirections = new List<Vector3>();

        foreach (CollisionInfo collision in collisions) {
            if (collision.objectType == Door.DoorType.Entrance || collision.objectType == Door.DoorType.Exit) {
                doorCount++;
                doorDirections.Add(collision.direction);
            } else if (collision.objectType == Door.DoorType.Wall) {
                wallCount++;
                wallDirections.Add(collision.direction);
            }
                Debug.Log(collision + " " + collision.objectType + " " + collision.point);
        }
        Debug.Log("doorCount = " + doorCount + ", wallCount = " + wallCount);
        // Basado en doorCount y wallCount, decide qué tipo de terreno instanciar
        List<GameObject> posiblesTerrains;
        if (wallCount == 3) {
            return terrains.Find(t => t.GetComponent<BlockTerrain>().GetDoorsCount() == 1);
        } else if (doorCount == 4) {
            return terrains.Find(t => t.GetComponent<BlockTerrain>().GetDoorsCount() == 4);
        } else if (doorCount == 1 && wallCount == 0) {
            posiblesTerrains = terrains.FindAll(t => (t.GetComponent<BlockTerrain>().GetDoorsCount() == 2 || t.GetComponent<BlockTerrain>().GetDoorsCount() == 3 || t.GetComponent<BlockTerrain>().GetDoorsCount() == 4));
        } else if (doorCount == 1 && wallCount == 1) {
            posiblesTerrains = terrains.FindAll(t => {
                var terrain = t.GetComponent<BlockTerrain>();
                return (terrain.GetDoorsCount() == 2 || terrain.GetDoorsCount() == 3) && (CollisionInfo.AreDirectionsOpposite(wallDirections[0], doorDirections[0]) != terrain.HasOppositeDoors());
            }
           );
        } else if (doorCount == 1 && wallCount == 2) {
            posiblesTerrains = terrains.FindAll(t => {
                var terrain = t.GetComponent<BlockTerrain>();
                if (CollisionInfo.AreDirectionsOpposite(wallDirections[0], wallDirections[1])) {
                    return (terrain.GetDoorsCount() == 2 && terrain.HasOppositeDoors()) || terrain.GetDoorsCount() == 3;
                } else {
                    return terrain.GetDoorsCount() == 2 && !terrain.HasOppositeDoors() || terrain.GetDoorsCount() == 3 ;
                }
            }
            );
        } else if (doorCount == 2 && wallCount == 2) {
            posiblesTerrains = terrains.FindAll(t => {
                var terrain = t.GetComponent<BlockTerrain>();
                if (CollisionInfo.AreDirectionsOpposite(doorDirections[0], doorDirections[1])) {
                    return (terrain.GetDoorsCount() == 2 && terrain.HasOppositeDoors());
                } else {
                    return terrain.GetDoorsCount() == 2 && !terrain.HasOppositeDoors();
                }
            });

        } else if (doorCount == 2 && wallCount == 0) {
            posiblesTerrains = terrains.FindAll(t => {
                var terrain = t.GetComponent<BlockTerrain>();
                return (terrain.GetDoorsCount() == 3);
            }
            );
        } else if (wallCount == 2) {
            posiblesTerrains = terrains.FindAll(t => {
                var terrain = t.GetComponent<BlockTerrain>();
                return (terrain.GetDoorsCount() == 2 || terrain.GetDoorsCount() == 3 || terrain.GetDoorsCount() == 4) &&
                    (CollisionInfo.AreDirectionsOpposite(wallDirections[0], wallDirections[1]) == terrain.HasOppositeDoors());
            });
        } else if (doorCount == 3 && wallCount == 0) {
            posiblesTerrains = terrains.FindAll(t => (t.GetComponent<BlockTerrain>().GetDoorsCount() == 4));

        } else if (doorCount == 3 && wallCount == 1) {
            posiblesTerrains = terrains.FindAll(t => (t.GetComponent<BlockTerrain>().GetDoorsCount() == 3));

        } else if (doorCount == 2 && wallCount == 1) {
            posiblesTerrains = terrains.FindAll(t => {
                var terrain = t.GetComponent<BlockTerrain>();
                if (CollisionInfo.AreDirectionsOpposite(doorDirections[0], doorDirections[1])) {
                    return (terrain.GetDoorsCount() == 2 && terrain.HasOppositeDoors()) || terrain.GetDoorsCount() == 3;
                } else {
                    return terrain.GetDoorsCount() == 2 && !terrain.HasOppositeDoors() || terrain.GetDoorsCount() == 3;
                }
            });
        } else {
            posiblesTerrains = null;
        }
        if (posiblesTerrains == null)
            return null;
        return posiblesTerrains[Random.Range(0, posiblesTerrains.Count - 1)];


    }
    private Door GetExitDoor(Door clickedDoor, GameObject terrain) {
        var exitDoor = terrain.GetComponent<BlockTerrain>().TouchDoor(clickedDoor);
        terrain.GetComponent<BlockTerrain>().ResetDoors();
        exitDoor.doorType = Door.DoorType.Exit;
        terrain.GetComponent<BlockTerrain>().RePath();
        return exitDoor;
    }

    private IEnumerator CalculateRotationCoroutine(Door clickedDoor, GameObject newTerrain, int touchDoors) {
        var terrain = newTerrain.GetComponent<BlockTerrain>();
        var doorsCount = 4;
        var tmpTouchCount = touchDoors;
        var checkDoor = terrain.CheckDoors(tmpTouchCount);
        var touchDoor = terrain.TouchDoor(clickedDoor);
        Debug.Log("ANTES Volteando " + doorsCount, newTerrain);
        Debug.Log("ANTES terrain.CheckDoors() " + checkDoor, newTerrain);
        Debug.Log("ANTES terrain.TouchDoor(clickedDoor) " + touchDoor, newTerrain);
        Debug.Log("ANTES  ((terrain.CheckDoors(terrain.GetActiveDoors().Count) && terrain.TouchDoor(clickedDoor) != null)) = " + ((checkDoor && touchDoor != null)).ToString(), newTerrain);
        while ((checkDoor && touchDoor != null) == false) {
            doorsCount--;
            if (doorsCount < 0) {
                break;
            }
            terrain.transform.Rotate(0, 90, 0);
            tmpTouchCount = touchDoors;
            checkDoor = terrain.CheckDoors(tmpTouchCount);
            touchDoor = terrain.TouchDoor(clickedDoor);
            Debug.Log("DENTRO Volteando = " + doorsCount, newTerrain);
            Debug.Log("DENTRO terrain.CheckDoors() = " + checkDoor, newTerrain);
            Debug.Log("DENTRO terrain.TouchDoor(clickedDoor) = " + touchDoor, newTerrain);
            Debug.Log("DENTRO  ((terrain.CheckDoors(terrain.GetActiveDoors().Count) && terrain.TouchDoor(clickedDoor) != null)) = " + (checkDoor && touchDoor != null).ToString(), newTerrain);
            yield return new WaitForSeconds(5);
        }
        Debug.Log("DESPUES Volteando = " + doorsCount, newTerrain);
        Debug.Log("DESPUES terrain.CheckDoors() = " + checkDoor, newTerrain);
        Debug.Log("DESPUES terrain.TouchDoor(clickedDoor) = " + touchDoor, newTerrain);
        Debug.Log("DESPUES ((terrain.CheckDoors(terrain.GetActiveDoors().Count) && terrain.TouchDoor(clickedDoor) != null)) = " + (checkDoor && touchDoor != null).ToString(), newTerrain);
        var newExitDoor = GetExitDoor(clickedDoor, newTerrain);
        newExitDoor.point.points.Add(clickedDoor.point);
        //Debug.Log("Entrando a DeactiveOcupedDoors.---------------------", newTerrain);

        StartCoroutine(DeactiveDoors(newTerrain.GetComponent<BlockTerrain>(), clickedDoor.terrain));
    }

    private void CalculateRotation(Door clickedDoor, GameObject newTerrain, int touchDoors) {
        var terrain = newTerrain.GetComponent<BlockTerrain>();
        var doorsCount = 4;
        var tmpTouchCount = touchDoors;
        var checkDoor = terrain.CheckDoors(tmpTouchCount);
        var touchDoor = terrain.TouchDoor(clickedDoor);
        //Debug.Log("ANTES Volteando " + doorsCount, newTerrain);
        //Debug.Log("ANTES terrain.CheckDoors() " + checkDoor, newTerrain);
        //Debug.Log("ANTES terrain.TouchDoor(clickedDoor) " + touchDoor, newTerrain);
        //Debug.Log("ANTES  ((terrain.CheckDoors(terrain.GetActiveDoors().Count) && terrain.TouchDoor(clickedDoor) != null)) = " + ((checkDoor && touchDoor != null)).ToString(), newTerrain);
        while ((checkDoor && touchDoor != null) == false) {
            doorsCount--;
            if (doorsCount < 0) {
                break;
            }
            terrain.transform.Rotate(0, 90, 0);
            tmpTouchCount = touchDoors;
            checkDoor = terrain.CheckDoors(tmpTouchCount);
            touchDoor = terrain.TouchDoor(clickedDoor);
            //Debug.Log("DENTRO Volteando = " + doorsCount, newTerrain);
            //Debug.Log("DENTRO terrain.CheckDoors() = " + checkDoor, newTerrain);
            //Debug.Log("DENTRO terrain.TouchDoor(clickedDoor) = " + touchDoor, newTerrain);
            //Debug.Log("DENTRO  ((terrain.CheckDoors(terrain.GetActiveDoors().Count) && terrain.TouchDoor(clickedDoor) != null)) = " + (checkDoor && touchDoor != null).ToString(), newTerrain);
        }
        //Debug.Log("DESPUES Volteando = " + doorsCount, newTerrain);
        //Debug.Log("DESPUES terrain.CheckDoors() = " + checkDoor, newTerrain);
        //Debug.Log("DESPUES terrain.TouchDoor(clickedDoor) = " + touchDoor, newTerrain);
        //Debug.Log("DESPUES ((terrain.CheckDoors(terrain.GetActiveDoors().Count) && terrain.TouchDoor(clickedDoor) != null)) = " + (checkDoor && touchDoor != null).ToString(), newTerrain);
    }

    private Vector3 CalculatePosition(Door clickedDoor, GameObject newTerrain) {
        Vector3 direction = clickedDoor.transform.position - newTerrain.transform.position;
        direction.Normalize(); // Normaliza la dirección para obtener un vector unitario

        // Mueve el objeto en la dirección calculada
        return newTerrain.transform.position + direction * GridManager.Instance.cellSize;
    }
    private void Update() {
        if (Input.GetKeyUp(KeyCode.Space)) {
            var terrains = new List<BlockTerrain>(FindObjectsByType<BlockTerrain>(FindObjectsSortMode.None));
            terrains = terrains.FindAll(t => t.GetActiveDoorsCount() > 0);
            if (terrains.Count > 0) {

                var terrain = terrains[Random.Range(0, terrains.Count - 1)];
                Door door = terrain.GetActiveDoors()[Random.Range(0, terrain.GetActiveDoorsCount() - 1)];
                terrain.OnDoorClicked(door);
            }

        }
    }
}
