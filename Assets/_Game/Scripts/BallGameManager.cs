using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGameManager : MonoBehaviour
{
    [System.Serializable]
    public class DifficultySettings
    {
        public Vector2Int gridSize;
        public int coinCount;
        [Range(0, 0.5f)] public float holeProbability;
        public int stageThreshold;
    }

    public List<LevelData> levels;

    [Header("Settings")]
    [SerializeField] private DifficultySettings[] difficultyLevels;

    [Header("References")]
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private BallController ballController;
    // [SerializeField] private UIManager uiManager;

    private int currentStage = 1;
    private int coinsCollected;

    private void Start()
    {
        LoadGame();
        InitializeLevel();
    }

    private void InitializeLevel()
    {
        if (levels.Count<=currentStage)
        {
            currentStage = 0;
        }
        
        levelManager.LoadLevel(levels[currentStage]);
        ballController.Initialize(levels[currentStage].startPosition);
        // levelManager.SpawnCoins(settings.coinCount);
    }
    private DifficultySettings GetCurrentDifficulty()
    {
        foreach (var settings in difficultyLevels)
        {
            if (currentStage <= settings.stageThreshold)
                return settings;
        }
        return difficultyLevels[^1];
    }   
    public void AddCoins(int amount)
    {
        coinsCollected += amount;
        // uiManager.UpdateCoinDisplay(coinsCollected);
        SaveGame();
    }

    public void CompleteLevel()
    {
        currentStage++;
        SaveGame();
        InitializeLevel();
    }
    

    private void LoadGame()
    {
        currentStage = PlayerPrefs.GetInt("stage", 0);
        coinsCollected = PlayerPrefs.GetInt("coins", 0);
    }

    private void SaveGame()
    {
        PlayerPrefs.SetInt("stage", currentStage);
        PlayerPrefs.SetInt("coins", coinsCollected);
        PlayerPrefs.Save();
    }

    public int GetCurrentStage()
    {
        return currentStage;
    }
}