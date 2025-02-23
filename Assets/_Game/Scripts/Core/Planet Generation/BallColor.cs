using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallColor : MonoBehaviour
{
    [SerializeField] Renderer rend;
    [SerializeField] Rigidbody rb;
    private PlanetSegment _segment;
    private static readonly int Color1 = Shader.PropertyToID("_BaseColor");
    private SoundController _soundController;

    public void Initialize(Color color, PlanetSegment segment)
    {
        rend.material.SetColor(Color1, color);
        rb.isKinematic = true;
        rb.useGravity = false;

        _segment = segment;
        _soundController = SoundController.Instance;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Projectile>(out Projectile projectile)) return;

        // Cache references first
        Color projectileColor = projectile.Color;
        Color segmentColor = rend.material.GetColor(Color1);
        bool colorsMatch = ColorUtility.CompareColors(projectileColor, segmentColor);

        // Debug with color details
        // Debug.Log($"Collision - Projectile: {ColorUtility.ToHtmlStringRGB(projectileColor)} " +
        //           $"Segment: {ColorUtility.ToHtmlStringRGB(segmentColor)} " +
        //           $"Match: {colorsMatch}");

        if (!colorsMatch)
        {
            projectile.Recycle();
            return;
        }

        // Only process valid matches
        Debug.Log("Valid color match - destroying segment");
        if (_segment != null)
        {
            _soundController.StartVibration();
            _segment.DestroySegment();
            projectile.Recycle();
            rb.AddExplosionForce(5f, other.transform.position, 5f);
        }
    }
}

public static class ColorUtility
{
    public static bool CompareColors(Color a, Color b, float epsilon = 0.01f)
    {
        return Mathf.Abs(a.r - b.r) < epsilon &&
               Mathf.Abs(a.g - b.g) < epsilon &&
               Mathf.Abs(a.b - b.b) < epsilon &&
               Mathf.Abs(a.a - b.a) < epsilon;
    }
}