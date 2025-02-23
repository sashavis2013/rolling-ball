using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private ColorPaletteSO colorPalette;
    [SerializeField] Transform launchPoint;

    [SerializeField] private int ammoCount = 15;

    private Color color;
    ICannonInput inputSystem;
    CannonAimer aimer;
    ProjectileLauncher launcher;
    IProjectileFactory projectileFactory;
    public static event Action OnPlaying;
    bool _isPlaying;
    private bool _isLevelComplete;
    public static event Action OnPlayerDeath;
    private MeshRenderer _renderer;

    void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        inputSystem = GetComponent<ICannonInput>();
        aimer = GetComponent<CannonAimer>();
        launcher = GetComponent<ProjectileLauncher>();
        projectileFactory = GetComponent<IProjectileFactory>();
        if (projectileFactory == null)
        {
            Debug.LogError("Missing Projectile Factory component!");
        }

        ChangeColor();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (!_isPlaying)
            {
                _isPlaying = true;
                OnPlaying?.Invoke();
            }


            Vector2 aimDirection = inputSystem.GetAimDirection(Input.GetTouch(0).position);
            aimer.UpdateAim(aimDirection);

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                FireProjectile();
                ChangeColor();
            }
        }
    }

    private void ChangeColor()
    {
        color = colorPalette.GetRandomColor();
        _renderer.material.color = color;
    }

    public void FireProjectile()
    {
        if (_isLevelComplete) return;
        ammoCount--;
        Projectile projectile = projectileFactory.GetProjectile();
        if (projectile != null)
        {
            var currentColor = color;
            projectile.transform.position = launchPoint.position;
            projectile.gameObject.SetActive(true);
            launcher.LaunchProjectile(projectile, currentColor);
        }

        if (ammoCount <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        if (_isLevelComplete) return;
        _isLevelComplete = true;

        OnPlayerDeath?.Invoke();

        SoundController.Instance.PlayAudio(AudioType.Die);
    }
}