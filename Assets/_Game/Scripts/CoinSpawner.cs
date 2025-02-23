// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class CoinSpawner : MonoBehaviour
// {
//     [SerializeField] private int _coinsPerLevel = 3;
//     
//     private List<Tile> _availableTiles = new List<Tile>();
//
//     public void SpawnCoins(Vector2Int startPosition)
//     {
//         _availableTiles.Clear();
//         
//         foreach(var tile in FindObjectsOfType<Tile>())
//         {
//             if(!tile.IsFilled && tile.GridPosition != startPosition)
//                 _availableTiles.Add(tile);
//         }
//
//         FisherYatesShuffle(_availableTiles);
//
//         for(int i = 0; i < Mathf.Min(_coinsPerLevel, _availableTiles.Count); i++)
//         {
//             _availableTiles[i].SetCoin(true);
//         }
//     }
//
//     private void FisherYatesShuffle<T>(IList<T> list)
//     {
//         for(int i = list.Count - 1; i > 0; i--)
//         {
//             int j = Random.Range(0, i + 1);
//             (list[i], list[j]) = (list[j], list[i]);
//         }
//     }
// }