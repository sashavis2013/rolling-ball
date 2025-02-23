using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum MenuType
{
    None,
    Main,
    Credit,
    Gameplay,
    Exit,
    CompleteStageMenu,
    GameOver,
    Pause,
    Setting,
    Skins,
    ReviveMenu
}

[RequireComponent(typeof(MenuAnimation))]
public abstract class Menu : MonoBehaviour
{
    [SerializeField] MenuType _type;
    [SerializeField] bool _useAnimation = false;

    private MenuAnimation _menuAnimation;
    private Canvas _canvas;

    public MenuType Type => _type;

    protected virtual void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _menuAnimation = GetComponent<MenuAnimation>();
    }

    public virtual void SetEnable()
    {
        _canvas.enabled = true;
        if (!_useAnimation) return;

        _menuAnimation.PlayShowUI();
    }

    public virtual void SetDisable()
    {
        if (!_useAnimation)
        {
            DisableCanvas();
            return;
        }

        _menuAnimation.SetDisable(DisableCanvas);
    }

    public void DisableCanvas()
    {
        //if (_menuManager.GetCurrentMenu == _type) return;
        _canvas.enabled = false;
    }

    protected void OnButtonPressed(Button button, UnityAction buttonListener)
    {
        if (!button)
        {
            Debug.LogWarning($"There is a 'Button' that is not attached to the '{gameObject.name}' script,  but a script is trying to access it.");
            return;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(buttonListener);
    }
}
