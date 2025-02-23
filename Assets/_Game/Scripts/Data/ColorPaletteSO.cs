using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Color Palette")]
public class ColorPaletteSO : ScriptableObject
{
    public Color[] colors;
    
    public Color GetRandomColor()
    {
        if (colors.Length == 0) return Color.white;
        return colors[Random.Range(0, colors.Length)];
    }
}