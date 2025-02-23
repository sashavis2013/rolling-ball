using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCore : MonoBehaviour
{
    public static event Action OnLevelComplete;
    bool _levelComplete = false;
    private SoundController _soundController;

    private void Start()
    {
        _soundController = SoundController.Instance;
    }

    void OnTriggerEnter(Collider other)
    {
        if (_levelComplete) return;
        if (!other.TryGetComponent<Projectile>(out Projectile projectile)) return;

        _soundController.StartVibration();
        OnLevelComplete?.Invoke();
        _levelComplete = true;
        _soundController.PlayAudio(AudioType.Win);
    }
}