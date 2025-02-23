using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinsMenu : Menu
{
    [Header("UI References :")]
    [SerializeField] Button _backButton;

    [Space]
    [SerializeField] Button _nextSkinButton;
    [SerializeField] Button _previousSkinButton;
    [SerializeField] Image _playerSkinImage;
    [SerializeField] private List<Material> _playerSkins;
    private GameObject _player;
    
    private bool _sfxState;
    private bool _musicState;
    private bool _vibrationState;
    private int _selectedSkin;

    private void Start()
    {
        _player = FindObjectOfType<BallController>().gameObject;

        _selectedSkin = PlayerPrefs.GetInt("selected_skin");

        OnButtonPressed(_backButton, HandleBackButtonPressed);

        OnButtonPressed(_nextSkinButton, HandleNextSkinButton);
        OnButtonPressed(_previousSkinButton, HandlePreviousSkinButton);
        UpdateSkin();

    }
    

    public override void SetEnable()
    {
        base.SetEnable();
        _backButton.interactable = true;
    }

    private void HandleBackButtonPressed()
    {
        _player.GetComponent<MeshRenderer>().material = _playerSkins[_selectedSkin];
        _backButton.interactable = false;
        PlayerPrefs.SetInt("selected_skin",_selectedSkin);

        //SaveManager.SaveSkin(_selectedSkin);

        MenuManager.Instance.CloseMenu();
    }
    
    

    private void HandleNextSkinButton()
    {
        _selectedSkin++;
        if (_selectedSkin>_playerSkins.Count)
        {
            _selectedSkin = 0;
        }
        UpdateSkin();
    }

    private void UpdateSkin()
    {
        _playerSkinImage.material = _playerSkins[_selectedSkin];
    }

    private void HandlePreviousSkinButton()
    {
            _selectedSkin--;
            if (_selectedSkin<=0)
            {
                _selectedSkin = _playerSkins.Count;
            }
            UpdateSkin();
    }

    private void HandleSaveSkinButton()
    {
        //SaveManager.SaveSkin(_selectedSkin);
    }

   
}