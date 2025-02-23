using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Ball Game/Level Data")]
public class LevelData : ScriptableObject
{
    [Serializable]
    public class TileData
    {
        public Vector2Int position;
        public bool isWall;
        public bool startsFilled;
    }

    public Vector2Int gridSize = new Vector2Int(5, 5);
    public Vector2Int startPosition = Vector2Int.zero;
    public int coinsToSpawn = 3;
    public int steps = 3;
    public TileData[] tiles;
    
#if UNITY_EDITOR
    public void InitializeDefaultGrid()
    {
        tiles = new TileData[gridSize.x * gridSize.y];
        int index = 0;
        
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                tiles[index] = new TileData
                {
                    position = new Vector2Int(x, y),
                    isWall = false,
                    startsFilled = false
                };
                index++;
            }
        }
    }
    
    public TileData GetTileAt(Vector2Int position)
    {
        return tiles.Length > 0 ? 
            System.Array.Find(tiles, t => t.position == position) : null;
    }
    
    public void SetWallAt(Vector2Int position, bool isWall)
    {
        TileData tile = GetTileAt(position);
        if (tile != null)
        {
            tile.isWall = isWall;
        }
    }
#endif
}