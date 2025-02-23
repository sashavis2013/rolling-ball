using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    int _currentStage;
    float _fullDistance;
    bool _isRevived = false;
    Vector3 _finishPosition;
    Transform _startTransform;
    MenuManager _menu;
    ScoreHandler _score;

    public ScoreHandler GetScoreHandler => _score;

    public static event Action<float> OnUpdateProgressValue;

    private void Awake()
    {
        _score = GetComponent<ScoreHandler>();
        Application.targetFrameRate = 120;
    }

    private void OnEnable()
    {
        BallController.OnPlayerDeath += ShowEndScreen;
        PlanetCore.OnLevelComplete += HandleOnLevelComplete;
        BallController.OnPlaying += HandleOnPlaying;
    }

    private void OnDisable()
    {
        BallController.OnPlayerDeath -= ShowEndScreen;
        PlanetCore.OnLevelComplete -= HandleOnLevelComplete;
        BallController.OnPlaying -= HandleOnPlaying;
    }

    private void Start()
    {
        _menu = MenuManager.Instance;
        _currentStage = PlayerPrefs.GetInt("stage", 1);
    }

    private void Update()
    {
        UpdateProgressValue();
    }

    public void UpdateProgressValue()
    {
        // float progressValue = Mathf.InverseLerp(_full, 0f, new);
        // OnUpdateProgressValue?.Invoke(progressValue);
    }


    // event handler
    private void HandleOnLevelComplete()
    {
        PlayerPrefs.SetInt("stage", _currentStage + 1);
        _menu.SwitchMenu(MenuType.CompleteStageMenu);
    }


    // event handler
    private void HandleOnPlaying()
    {
        if (_menu.GetCurrentMenu == MenuType.Main)
        {
            _menu.CloseMenu(); // close main menu
        }
    }

    // event handler
    private void ShowEndScreen()
    {
        if (!_isRevived)
        {
            _isRevived = true;
            _menu.OpenMenu(MenuType.ReviveMenu);
        }
        else
        {
            _menu.OpenMenu(MenuType.GameOver);
        }
    }
}