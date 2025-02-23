using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlanetLayer : MonoBehaviour
{
    private Vector3 _corePosition;
    private float _radius;
    private int _segments;
    private float _missingChance;
    private GameObject _ballPrefab;
    private Color _color;
    private float _ballDensity;
    private ColorPaletteSO _colorPalette;
    private Color _segmentColor;

    public void Initialize(ColorPaletteSO colorPalette,Vector3 corePosition, float radius, int segments,
                          float missingChance, GameObject ballPrefab, float ballDensity)
    {
        _colorPalette = colorPalette;
        _corePosition = corePosition;
        _radius = radius;
        _segments = segments;
        _missingChance = missingChance;
        _ballPrefab = ballPrefab;
        _ballDensity = ballDensity;

        
        _segmentColor = _colorPalette.GetRandomColor();
        GenerateSegments();
    }

    void GenerateSegments()
    {
        List<Vector3> positions = CalculateSegmentPositions();
        
        foreach (Vector3 pos in positions)
        {
            if (Random.value < _missingChance) continue;
            CreateSegment(pos);
        }
    }

    List<Vector3> CalculateSegmentPositions()
    {
        List<Vector3> positions = new List<Vector3>();
        float goldenAngle = Mathf.PI * (3f - Mathf.Sqrt(5f));

        for (int i = 0; i < _segments; i++)
        {
            float y = 1 - (i / (float)(_segments - 1)) * 2;
            float r = Mathf.Sqrt(1 - y * y);
            float theta = goldenAngle * i;

            Vector3 pos = _corePosition + new Vector3(
                Mathf.Cos(theta) * r * _radius,
                y * _radius,
                Mathf.Sin(theta) * r * _radius
            );
            positions.Add(pos);
        }
        return positions;
    }

    void CreateSegment(Vector3 position)
    {
        GameObject segment = new GameObject("Segment");
        segment.transform.SetParent(transform);
        segment.transform.position = position;

        PlanetSegment segmentScript = segment.AddComponent<PlanetSegment>();
        segmentScript.Initialize(
            corePosition: _corePosition,
            radius: _radius,
            ballPrefab: _ballPrefab,
            color: _colorPalette.GetRandomColor(),
            segmentsPerLayer: _segments,
            ballDensity: _ballDensity
        );
    }
}