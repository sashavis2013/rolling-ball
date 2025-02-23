using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject filledVisual;
    [SerializeField] private GameObject WallVisual;
    private bool isFilled;
    private bool isWall;
    private Vector2Int position;

    public bool IsFilled => isFilled;
    public bool IsWall => isWall;
    public Vector2Int Position => position;

    public void Initialize(Vector2Int gridPosition, bool isWall)
    {
        transform.position = new Vector3(gridPosition.x, 0, gridPosition.y);
        position = gridPosition;
        SetFilled(isWall);
        SetWall(isWall);
    }
    public void SetWall(bool wall)
    {
        WallVisual.SetActive(wall);
        isWall = wall;
        // GetComponent<Collider>().enabled = false;
    }

    public void SetFilled(bool filled)
    {
        if (WallVisual.activeSelf) return;
        isFilled = filled;
        filledVisual.SetActive(filled);
    }
}