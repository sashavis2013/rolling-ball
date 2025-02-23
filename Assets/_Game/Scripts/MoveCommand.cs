using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MoveCommand
{
    public Vector2Int OriginalPosition { get; }
    public Vector2Int TargetPosition { get; }
    
    private readonly List<Vector2Int> path;
    private readonly List<Coin> collectedCoins;
    private readonly LevelManager levelManager;
    private readonly BallGameManager gameManager;
    public static event Action<int> OnTileFilled;


    public MoveCommand(Vector2Int from, Vector2Int to,
        List<Vector2Int> path,
        List<Coin> coins,
        LevelManager lm,
        BallGameManager gm)
    {
        OriginalPosition = from;
        TargetPosition = to;
        this.path = path;
        collectedCoins = coins;
        levelManager = lm;
        gameManager = gm;
    }

    public void Execute()
    {
        foreach (Vector2Int pos in path)
        {
            levelManager.SetTileFilled(pos, true);
            OnTileFilled?.Invoke(1);
        }
        
        gameManager.AddCoins(collectedCoins.Count);
    }

    public void Undo()
    {
        foreach (Vector2Int pos in path)
        {
            levelManager.SetTileFilled(pos, false);
            OnTileFilled?.Invoke(-1);
        }
        foreach (Coin coin in collectedCoins)
            coin.Restore();
    }
}