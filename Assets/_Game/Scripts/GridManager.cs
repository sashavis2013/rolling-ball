// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class GridManager : MonoBehaviour
// {
//     public static GridManager Instance { get; private set; }
//
//     private Dictionary<Vector2Int, Tile> _tiles = new Dictionary<Vector2Int, Tile>();
//
//     void Awake() => Instance = this;
//
//     public void InitializeGrid()
//     {
//         _tiles.Clear();
//         
//         foreach(Transform child in transform)
//         {
//             if(child.TryGetComponent<Tile>(out Tile tile))
//             {
//                 Vector2Int gridPos = new Vector2Int(
//                     Mathf.RoundToInt(child.position.x),
//                     Mathf.RoundToInt(child.position.z)
//                 );
//                 tile.Initialize(gridPos);
//                 _tiles.Add(gridPos, tile);
//             }
//         }
//     }
//
//     public Tile GetTile(Vector2Int position) => 
//         _tiles.TryGetValue(position, out Tile tile) ? tile : null;
//
//     public bool IsPositionValid(Vector2Int position) => _tiles.ContainsKey(position);
// }