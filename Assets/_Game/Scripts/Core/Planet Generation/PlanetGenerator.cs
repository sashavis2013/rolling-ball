using UnityEngine;
using System.Collections.Generic;
public class PlanetGenerator : MonoBehaviour
{
    [Header("Core Settings")]
    public float coreRadius = 5f;
    public int layers = 3;
    public float layerThickness = 1.5f;

    [Header("Layer Settings")]
    public int segmentsPerLayer = 12;
    [Range(0, 1)] public float missingChance = 0f;
    public float ballDensity = 0.5f;

    [Header("References")]
    public GameObject ballPrefab;
    [SerializeField] private ColorPaletteSO colorPalette;

    void Start() => GeneratePlanet();

    public void GeneratePlanet()
    {
        ClearChildren();
        CalculateDifficulty();
        CreateLayers();
    }

    void ClearChildren()
    {
        foreach (Transform child in transform) 
            Destroy(child.gameObject);
    }

    void CreateLayers()
    {
        for (int layer = 1; layer <= layers; layer++)
        {
            CreateLayer(
                layerNumber: layer,
                radius: coreRadius + (layer-1)*layerThickness
            );
        }
    }

    void CreateLayer(int layerNumber, float radius)
    {
        GameObject layerGO = new GameObject($"Layer_{layerNumber}");
        layerGO.transform.SetParent(transform);
        layerGO.transform.position = transform.position;

        PlanetLayer layer = layerGO.AddComponent<PlanetLayer>();
        layer.Initialize(
            colorPalette: colorPalette,
            corePosition: transform.position,
            radius: radius,
            segments: segmentsPerLayer,
            missingChance: missingChance,
            ballPrefab: ballPrefab,
            ballDensity: ballDensity
        );
    }
    
    private void CalculateDifficulty()
    {
        int currentStage = PlayerPrefs.GetInt("stage", 1);

        if (currentStage <= 3)
        {
            segmentsPerLayer = 2;
            layers=1;
            missingChance = 0.05f;
        }
        else if (currentStage is > 3 and <= 5)
        {
            segmentsPerLayer = 3;
            layers=2;
            missingChance = 0.1f;
        }
        else if (currentStage is > 5 and <= 10)
        {
            segmentsPerLayer = 4;
            layers=3;
            missingChance = 0.15f;
        }
        else if (currentStage is > 10 and <= 20)
        {
            segmentsPerLayer = 5;
            layers=3;
            missingChance = 0.1f;
        }
        else if (currentStage is > 20 and <= 40)
        {
            segmentsPerLayer = 4;
            layers=4;
            coreRadius = 8f;
            missingChance = 0.1f;
        }
        else
        {
            segmentsPerLayer = 6;
            layers=3;
            coreRadius = 6f;
            missingChance = 0.1f;
            ballDensity = 2f;
        }
    }
}