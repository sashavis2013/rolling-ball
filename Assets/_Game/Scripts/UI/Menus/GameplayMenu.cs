using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayMenu : Menu
{
    [Header("UI References :")]
    [SerializeField] private Image _fillImage;
    [SerializeField] private TMP_Text _currentStageText;
    [SerializeField] private TMP_Text _nextStageText;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Button _undoButton;

    public override void SetEnable()
    {
        base.SetEnable();

        UpdateProgressText();

        GameManager.OnUpdateProgressValue += UpdateProgressFill;
        ScoreHandler.OnScoreUpdated += UpdateScoreText;
    }

    private void Start()
    {
        var ballController= FindObjectOfType<BallController>();
        OnButtonPressed(_undoButton, () =>
        {
            ballController.UndoLastMove();
        });
    }

    private void OnDisable()
    {
        GameManager.OnUpdateProgressValue -= UpdateProgressFill;
        ScoreHandler.OnScoreUpdated -= UpdateScoreText;
    }

    private void UpdateScoreText(int score)
    {
        _scoreText.text = score.ToString();
    }

    private void UpdateProgressText() {
        int currentStage = PlayerPrefs.GetInt("stage", 1);
        _currentStageText.text = currentStage.ToString();
        _nextStageText.text = (currentStage + 1).ToString();
    }

    private void UpdateProgressFill(float value) {
        _fillImage.fillAmount = value;
    }
}