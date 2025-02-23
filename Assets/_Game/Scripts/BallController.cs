using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveDuration = 0.35f;

    [Header("References")]
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private BallGameManager gameManager;

    private Vector2Int currentGridPosition;
    private bool isMoving;
    private Stack<MoveCommand> commandHistory = new Stack<MoveCommand>();
    public static event Action OnPlaying;
    public static event Action OnPlayerDeath;
    private int stepsCount;
    private bool isLevelComplete;

    public void Initialize(Vector2Int startPosition)
    {
        currentGridPosition = startPosition;
        isLevelComplete = false;
        // levelManager.SetTileFilled(currentGridPosition,true);
        transform.position = new Vector3(startPosition.x, 1f, startPosition.y);
        SetStepsCount(gameManager.levels[gameManager.GetCurrentStage()].steps);
    }
    

    public void SetStepsCount(int steps)
    {
        stepsCount = steps;
    }
    
    public void TryMove(Vector2Int direction)
    {
        if(isLevelComplete) return;
        
        OnPlaying?.Invoke();

        if (isMoving || direction == Vector2Int.zero) return;

        List<Vector2Int> path = CalculatePath(direction);
        if (path.Count == 0) return;

        ExecuteMove(direction, path);
    }

    private List<Vector2Int> CalculatePath(Vector2Int direction)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int current = currentGridPosition + direction;

        while (levelManager.IsPositionValid(current))
        {
            Vector2Int next = current + direction;
            if (levelManager.IsTileFilled(current)) break;
            if (levelManager.IsTileWall(current)) break;
            
            path.Add(current);
            current += direction;
        }
        return path;
    }
    
    

    private void ExecuteMove(Vector2Int direction, List<Vector2Int> path)
    {
        isMoving = true;
        Vector2Int newPosition = currentGridPosition + direction * path.Count;
        
        List<Coin> collectedCoins = CollectCoins(path);
        MoveCommand command = new MoveCommand(
            currentGridPosition,
            newPosition,
            path,
            collectedCoins,
            levelManager,
            gameManager
        );
        
        command.Execute();
        commandHistory.Push(command);

        transform.DOMove(new Vector3(newPosition.x, transform.position.y, newPosition.y), moveDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => {
                currentGridPosition = newPosition;
                isMoving = false;
                CheckLevelComplete();
                stepsCount--;
            });
        SoundController.Instance.PlayAudio(AudioType.Button);
    }

    private List<Coin> CollectCoins(List<Vector2Int> path)
    {
        List<Coin> collectedCoins = new List<Coin>();
        foreach (Vector2Int pos in path)
        {
            Collider[] hits = Physics.OverlapSphere(new Vector3(pos.x, 0.5f, pos.y), 0.3f);
            foreach (Collider hit in hits)
            {
                if (hit.TryGetComponent<Coin>(out Coin coin))
                {
                    collectedCoins.Add(coin);
                    coin.Collect();
                    SoundController.Instance.PlayAudio(AudioType.BallBounce);
                }
            }
        }
        return collectedCoins;
    }

    public void UndoLastMove()
    {
        if (isLevelComplete) return;
        if (commandHistory.Count == 0) return;
        if (isMoving) return;
        
        MoveCommand command = commandHistory.Pop();
        command.Undo();
        currentGridPosition = command.OriginalPosition;
        transform.DOMove(new Vector3(command.OriginalPosition.x, transform.position.y, command.OriginalPosition.y), moveDuration).OnComplete(() => {
            isMoving = false;
            CheckLevelComplete();
        });
        SoundController.Instance.PlayAudio(AudioType.ObstacleDestroy);
        isMoving = true;
    }

    private void CheckLevelComplete()
    {
        // Debug.Log(levelManager.GetAllTiles().Count);
        foreach (Tile tile in levelManager.GetAllTiles())
        {
            if (!tile.IsFilled && stepsCount <= 0)
            {
                Death();
                return;

            }

            if (!tile.IsFilled)
            {
                return;

            }
        }
        isLevelComplete = true;
        gameManager.CompleteLevel();
        Debug.Log("Won");

        
    }
    
    private void Death()
    {
        Debug.Log("Died");
        isLevelComplete = true;

        OnPlayerDeath?.Invoke();

        SoundController.Instance.PlayAudio(AudioType.Die);
    }

}