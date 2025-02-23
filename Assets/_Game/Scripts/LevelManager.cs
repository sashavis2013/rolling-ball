using System.Collections;
using System.Collections.Generic;
using UnityEngine;








public class LevelManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform gridParent;
    
    [Header("Level Data")]
    [SerializeField] private LevelData currentLevel;
    
    private Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();
    
    public void LoadLevel(LevelData levelData)
    {
        ClearGrid();
        currentLevel = levelData;
        
        // Create all tiles
        foreach (var tileData in levelData.tiles)
        {
            GameObject tileObj = Instantiate(tilePrefab, 
                new Vector3(tileData.position.x, 0, tileData.position.y), 
                Quaternion.identity, 
                gridParent);
            
            Tile tile = tileObj.GetComponent<Tile>();
            tile.Initialize(tileData.position, tileData.isWall);
            tile.SetFilled(tileData.startsFilled);
            tiles[tileData.position] = tile;
        }
        tiles[levelData.startPosition].SetFilled(true);
        
        
        SpawnRandomCoins();
    }
    
    private void SpawnRandomCoins()
    {
        List<Vector2Int> validPositions = new List<Vector2Int>();
        
        // Collect all valid positions (non-wall tiles that aren't the start position)
        foreach (var tile in tiles.Values)
        {
            if (!tile.IsWall && tile.Position != currentLevel.startPosition)
            {
                validPositions.Add(tile.Position);
            }
        }
        
        // Shuffle positions
        for (int i = validPositions.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (validPositions[i], validPositions[randomIndex]) = (validPositions[randomIndex], validPositions[i]);
        }
        
        // Spawn coins on the first N valid positions
        int coinsToSpawn = Mathf.Min(currentLevel.coinsToSpawn, validPositions.Count);
        for (int i = 0; i < coinsToSpawn; i++)
        {
            Vector2Int pos = validPositions[i];
            Instantiate(coinPrefab, 
                new Vector3(pos.x, 0.5f, pos.y), 
                Quaternion.identity,
                gridParent);
        }
    }
    
    public List<Tile> GetAllTiles()
    {
        return new List<Tile>(tiles.Values);
    }
    
    public bool IsPositionValid(Vector2Int position) => tiles.ContainsKey(position);
    public bool IsTileFilled(Vector2Int position) => tiles[position].IsFilled;
    public bool IsTileWall(Vector2Int position) => tiles[position].IsWall;
    public void SetTileFilled(Vector2Int position, bool filled) => tiles[position].SetFilled(filled);
    
    private void ClearGrid()
    {
        foreach (Transform child in gridParent)
            Destroy(child.gameObject);
        tiles.Clear();
    }
}













// public class LevelManager : MonoBehaviour
// {
//     [Header("Prefabs")]
//     [SerializeField] private GameObject tilePrefab;
//     [SerializeField] private GameObject coinPrefab;
//     [SerializeField] private Transform gridParent;
//     private Vector2Int currentGridSize;
//     [SerializeField] private bool drawDebugPath = true;
//
//     private Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();
//     private List<Vector2Int> validCoinPositions = new List<Vector2Int>();
//     private List<Vector2Int> solutionPath  = new List<Vector2Int>();
//     public Tile[,] grid;
//     private Stack<Vector2Int> dfsStack = new Stack<Vector2Int>();
//     private Vector2Int gridSize;
//     
//     public List<Tile> GetAllTiles()
//     {
//         var allTiles = new List<Tile>();
//         foreach (var tile in tiles.Values)
//         {
//             allTiles.Add(tile);
//         }
//
//         return allTiles;
//     }
//     
//
//     public void SpawnCoins(int coinCount)
//     {
//         ShufflePositions(validCoinPositions);
//
//         for (int i = 0; i < Mathf.Min(coinCount, validCoinPositions.Count); i++)
//         {
//             Vector2Int pos = validCoinPositions[i];
//             Instantiate(coinPrefab, new Vector3(pos.x, 0.5f, pos.y), Quaternion.identity);
//         }
//     }
//     
//    public void GenerateMaze(Vector2Int gridSize)
//     {
//         this.gridSize = gridSize;
//         InitializeGrid();
//         GenerateDFSPath();
//         DrawDebugPath();
//     }
//
//     private void InitializeGrid()
//     {
//         grid = new Tile[gridSize.x, gridSize.y];
//         
//         for (int x = 0; x < gridSize.x; x++)
//         {
//             for (int y = 0; y < gridSize.y; y++)
//             {
//                 Vector2Int pos = new Vector2Int(x, y);
//                 GameObject tileObj = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity, gridParent);
//                 
//                 grid[x, y] = tileObj.GetComponent<Tile>();
//                 grid[x, y].Initialize(pos, true); // All tiles start as walls
//                 tiles[pos] = grid[x, y];
//             }
//         }
//     }
//
//     
//     
//     
//     private void GenerateDFSPath()
//     {
//         Vector2Int start = Vector2Int.zero;
//         dfsStack.Push(start);
//         grid[start.x, start.y].SetWall(false);
//         grid[start.x, start.y].SetFilled(true);
//
//         // Track visited cells to avoid infinite loops
//         HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
//
//         while (dfsStack.Count > 0)
//         {
//             Vector2Int current = dfsStack.Pop();
//             visited.Add(current);
//
//             List<Vector2Int> neighbors = GetUnvisitedNeighbors(current, visited);
//             ShuffleNeighbors(neighbors);
//
//             foreach (Vector2Int neighbor in neighbors)
//             {
//                 if (!visited.Contains(neighbor))
//                 {
//                     RemoveWallBetween(current, neighbor);
//                     grid[neighbor.x, neighbor.y].SetWall(false);
//                     validCoinPositions.Add(new Vector2Int(neighbor.x, neighbor.y));
//                     dfsStack.Push(neighbor);
//                 }
//             }
//         }
//     }
//
//     private List<Vector2Int> GetUnvisitedNeighbors(Vector2Int pos, HashSet<Vector2Int> visited)
//     {
//         List<Vector2Int> neighbors = new List<Vector2Int>();
//         Vector2Int[] directions = { Vector2Int.up * 2, Vector2Int.right * 2, Vector2Int.down * 2, Vector2Int.left * 2 };
//
//         foreach (Vector2Int dir in directions)
//         {
//             Vector2Int neighbor = pos + dir;
//             if (IsValidPosition(neighbor) && !visited.Contains(neighbor))
//             {
//                 neighbors.Add(neighbor);
//             }
//         }
//         return neighbors;
//     }
//
//     private void ShuffleNeighbors(List<Vector2Int> neighbors)
//     {
//         for (int i = 0; i < neighbors.Count; i++)
//         {
//             int randomIndex = Random.Range(i, neighbors.Count);
//             (neighbors[i], neighbors[randomIndex]) = (neighbors[randomIndex], neighbors[i]);
//         }
//     }
//
//     private void RemoveWallBetween(Vector2Int a, Vector2Int b)
//     {
//         Vector2Int wallPosition = a + (b - a) / 2;
//         grid[wallPosition.x, wallPosition.y].SetWall(false); // Remove the wall
//     }
//
//     private bool IsValidPosition(Vector2Int pos)
//     {
//         return pos.x >= 0 && pos.x < gridSize.x && pos.y >= 0 && pos.y < gridSize.y;
//     }
//
//     private void DrawDebugPath()
//     {
//         if (!drawDebugPath || solutionPath.Count < 2) return;
//         
//         for (int i = 1; i < solutionPath.Count; i++)
//         {
//             Debug.DrawLine(
//                 new Vector3(solutionPath[i-1].x, 0.5f, solutionPath[i-1].y),
//                 new Vector3(solutionPath[i].x, 0.5f, solutionPath[i].y),
//                 Color.red, 10f
//             );
//         }
//     }
//
//     private void ClearGrid()
//     {
//         foreach (Transform child in gridParent)
//             Destroy(child.gameObject);
//         tiles.Clear();
//     }
//
//     public bool IsPositionValid(Vector2Int position) => tiles.ContainsKey(position);
//     public bool IsTileFilled(Vector2Int position) => tiles[position].IsFilled;
//     public bool IsTileWall(Vector2Int position) => tiles[position].IsWall;
//     public void SetTileFilled(Vector2Int position, bool filled) => tiles[position].SetFilled(filled);
//
//     private void ShufflePositions<T>(IList<T> list)
//     {
//         for (int i = 0; i < list.Count; i++)
//         {
//             int randomIndex = Random.Range(i, list.Count);
//             (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
//         }
//     }
//     
//     
// }