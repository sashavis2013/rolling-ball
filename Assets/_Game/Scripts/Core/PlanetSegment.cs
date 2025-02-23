using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlanetSegment : MonoBehaviour
{
    private List<Rigidbody> _segmentBalls = new List<Rigidbody>();
    public static event Action<int> OnSegmentDestroyed;

    public void Initialize(Vector3 corePosition, float radius,
        GameObject ballPrefab, Color color,
        int segmentsPerLayer, float ballDensity)
    {
        // Calculate angular spread per segment
        float maxAngle = Mathf.Acos(1 - 2f / segmentsPerLayer);

        // Calculate ball count for this segment's area
        float segmentArea = 4 * Mathf.PI * radius * radius / segmentsPerLayer;
        int ballCount = Mathf.Max(1, Mathf.RoundToInt(segmentArea * ballDensity));

        GenerateBalls(corePosition, radius, ballPrefab, color, ballCount, maxAngle);

        foreach (Transform child in transform)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb != null) _segmentBalls.Add(rb);
        }
    }

    void GenerateBalls(Vector3 corePosition, float radius,
        GameObject prefab, Color color,
        int count, float maxAngle)
    {
        Vector3 segmentDirection = (transform.position - corePosition).normalized;

        for (int i = 0; i < count; i++)
        {
            Vector3 ballDir = RandomDirectionInCone(segmentDirection, maxAngle);
            Vector3 ballPos = corePosition + ballDir * radius;

            GameObject ball = Instantiate(prefab, ballPos, Quaternion.identity, transform);
            ball.transform.LookAt(corePosition);
            ball.GetComponent<BallColor>().Initialize(color, this);
        }
    }

    public void DestroySegment()
    {
        int score = 0;
        foreach (Rigidbody rb in _segmentBalls)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            score++;
        }

        OnSegmentDestroyed?.Invoke(score);

        // Destroy entire segment after delay
        Destroy(gameObject, 8f);

        // Optional: Trigger particle effects/sound
    }

    Vector3 RandomDirectionInCone(Vector3 direction, float maxAngleRad)
    {
        float cosTheta = Mathf.Cos(maxAngleRad);
        float z = Random.Range(cosTheta, 1f);
        float phi = Random.Range(0f, 2f * Mathf.PI);

        float sinTheta = Mathf.Sqrt(1 - z * z);
        float x = sinTheta * Mathf.Cos(phi);
        float y = sinTheta * Mathf.Sin(phi);

        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, direction);
        return rotation * new Vector3(x, y, z);
    }
}