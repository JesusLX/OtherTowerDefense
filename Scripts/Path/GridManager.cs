using UnityEngine;

public class GridManager : Singleton<GridManager> {

    public int width = 10;
    public int height = 10;
    public float cellSize = 10f;

    public Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x * cellSize, 0, y * cellSize);
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition) {
        int x = Mathf.FloorToInt(worldPosition.x / cellSize);
        int y = Mathf.FloorToInt(worldPosition.z / cellSize);
        return new Vector2Int(x, y);
    }
}
