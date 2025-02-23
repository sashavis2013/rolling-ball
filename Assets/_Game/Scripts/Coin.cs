using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool _isCollected = false;
    private Vector3 _originalPosition;

    void Start()
    {
        _originalPosition = transform.position;
        transform.DORotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360).SetRelative(true)
            .SetEase(Ease.Linear).SetLoops(-1);
        transform.DOMove(new Vector3(_originalPosition.x, _originalPosition.y+0.25f, _originalPosition.z), 2f)
            .SetEase(Ease.InBounce)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void Collect()
    {
        _isCollected = true;
        transform.DOScale(Vector3.zero, 0.2f)
            .OnComplete(() => gameObject.SetActive(false));
    }

    private void OnDisable()
    {
        DOTween.Kill(transform);
    }

    public void Restore()
    {
        _isCollected = false;
        gameObject.SetActive(true);
        transform.position = _originalPosition;
        transform.DOScale(Vector3.one, 0.2f);
    }
}