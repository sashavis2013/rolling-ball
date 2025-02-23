#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelData))]
public class LevelDataEditor : Editor
{
    private bool showGridEditor = true;
    private static readonly Color wallColor = new Color(0.7f, 0.3f, 0.3f, 1);
    private static readonly Color emptyColor = new Color(0.7f, 0.7f, 0.7f, 1);
    private static readonly Color startPositionColor = new Color(0.3f, 0.7f, 0.3f, 1);
    private const float cellSize = 30f;
    private const float padding = 5f;
    
    public override void OnInspectorGUI()
    {
        LevelData levelData = (LevelData)target;
        
        EditorGUI.BeginChangeCheck();
        Vector2Int newGridSize = EditorGUILayout.Vector2IntField("Grid Size", levelData.gridSize);
        if (EditorGUI.EndChangeCheck() && newGridSize != levelData.gridSize)
        {
            levelData.gridSize = newGridSize;
            levelData.InitializeDefaultGrid();
            EditorUtility.SetDirty(levelData);
        }
        
        levelData.startPosition = EditorGUILayout.Vector2IntField("Start Position", levelData.startPosition);
        levelData.coinsToSpawn = EditorGUILayout.IntField("Coins to Spawn", levelData.coinsToSpawn);
        levelData.steps = EditorGUILayout.IntField("Available moves", levelData.steps);
        
        EditorGUILayout.Space();
        showGridEditor = EditorGUILayout.Foldout(showGridEditor, "Visual Grid Editor");
        
        if (showGridEditor && levelData.tiles != null)
        {
            DrawGrid(levelData);
            
            if (GUILayout.Button("Initialize Grid"))
            {
                levelData.InitializeDefaultGrid();
                EditorUtility.SetDirty(levelData);
            }
        }
        
        EditorGUILayout.HelpBox(
            "Left click: Toggle Wall\n" +
            "Right click: Set Start Position\n" +
            "Green: Start Position\n" +
            "Red: Wall\n" +
            "Gray: Empty Tile", 
            MessageType.Info);
    }
    
    private void DrawGrid(LevelData levelData)
    {
        Rect gridRect = GUILayoutUtility.GetRect(
            levelData.gridSize.x * cellSize + padding * 2, 
            levelData.gridSize.y * cellSize + padding * 2);
        
        EditorGUI.DrawRect(gridRect, Color.black);
        
        Event e = Event.current;
        Vector2 mousePos = e.mousePosition;
        
        // Draw coordinate labels
        GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
        {
            fontSize = 10,
            alignment = TextAnchor.MiddleCenter
        };
        
        for (int x = 0; x < levelData.gridSize.x; x++)
        {
            for (int y = 0; y < levelData.gridSize.y; y++)
            {
                // Convert editor Y coordinate to match Unity's coordinate system
                int unityY = (levelData.gridSize.y - 1) - y;
                Vector2Int currentPos = new Vector2Int(x, unityY);
                LevelData.TileData tile = levelData.GetTileAt(currentPos);
                
                if (tile == null) continue;
                
                Rect cellRect = new Rect(
                    gridRect.x + padding + x * cellSize,
                    gridRect.y + padding + y * cellSize,
                    cellSize - 2,
                    cellSize - 2
                );
                
                // Determine cell color
                Color cellColor = tile.isWall ? wallColor : emptyColor;
                if (currentPos == levelData.startPosition)
                {
                    cellColor = startPositionColor;
                }
                
                EditorGUI.DrawRect(cellRect, cellColor);
                
                // Draw coordinates in each cell
                GUI.Label(cellRect, $"{x},{unityY}", labelStyle);
                
                // Handle mouse input
                if (cellRect.Contains(mousePos))
                {
                    if (e.type == EventType.MouseDown && e.button == 0) // Left click
                    {
                        tile.isWall = !tile.isWall;
                        EditorUtility.SetDirty(levelData);
                        e.Use();
                    }
                    else if (e.type == EventType.MouseDown && e.button == 1) // Right click
                    {
                        levelData.startPosition = currentPos;
                        EditorUtility.SetDirty(levelData);
                        e.Use();
                    }
                }
            }
        }
        
        if (gridRect.Contains(mousePos))
        {
            Repaint();
        }
    }
}
#endif