using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class Maze : MonoBehaviour
{
    [SerializeField] float cellSize = 0.1f;
    [SerializeField] int width = 10;
    [SerializeField] int height = 10;
    [SerializeField] Vector2Int goalPosition = default;
    [SerializeField] Transform goalMarker = null;

    [SerializeField, HideInInspector] bool[] walls = null;

    [SerializeField] Mesh wallMesh = null;
    [SerializeField] Material wallMaterial = null;

    public int Width { get => width; }
    public int Height { get => height; }
    public int UpperBoundX { get => width - 1; }
    public int UpperBoundY { get => height - 1; }
    public Vector2Int GoalPosition { get => goalPosition; }

    private List<Matrix4x4> wallMatrices = new List<Matrix4x4>();

    public Vector3 CellWorldPosition(int x, int y)
    {
        Vector3 right = transform.right * x * cellSize;
        Vector3 up = transform.up * y * cellSize;
        return transform.position + up + right;
    }

    public bool CellHasWall(int x, int y)
    {
        return walls[y * width + x];
    }

    public bool IsWithinBounds(int x, int y)
    {
        return
            x >= 0 && x < width
            &&
            y >= 0 && y < height;
    }

    public Vector2Int ClampPosition(Vector2Int position)
    {
        position.x = Mathf.Clamp(position.x, 0, UpperBoundX);
        position.y = Mathf.Clamp(position.y, 0, UpperBoundY);
        return position;
    }

    private void Awake()
    {
        RecomputeMatrices();
    }

    private void OnValidate()
    {
        goalPosition = ClampPosition(goalPosition);
        if(goalMarker != null)
        {
            goalMarker.position = CellWorldPosition(goalPosition.x, goalPosition.y);
        }
    }

    private void Update()
    {
        if(wallMesh == null || wallMaterial == null) return;
        Graphics.DrawMeshInstanced(wallMesh, 0, wallMaterial, wallMatrices);
    }

    public void RecomputeMatrices()
    {
        wallMatrices.Clear();
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                if(!CellHasWall(x, y)) continue;
                wallMatrices.Add(Matrix4x4.TRS(CellWorldPosition(x, y), transform.rotation, Vector3.one * cellSize));
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Maze))]
public class MazeEditor : Editor
{
    SerializedProperty widthProp, heightProp, wallsProp;
    Maze maze;

    private void OnEnable()
    {
        widthProp = serializedObject.FindProperty("width");
        heightProp = serializedObject.FindProperty("height");
        wallsProp = serializedObject.FindProperty("walls");
        maze = target as Maze;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();

        EditorGUILayout.BeginVertical("box");
        int width = widthProp.intValue;
        int height = heightProp.intValue;
        wallsProp.arraySize = width * height;
        for(int y = height - 1; y > -1; y--)
        {
            EditorGUILayout.BeginHorizontal();
            for(int x = 0; x < width; x++)
            {
                SerializedProperty cellProp = wallsProp.GetArrayElementAtIndex(y * width + x);
                cellProp.boolValue = EditorGUILayout.Toggle(cellProp.boolValue);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
        if(GUILayout.Button("Update Representaion"))
        {
            maze.RecomputeMatrices();
            ForceEditorUpdate();
        }
    }

    private void OnSceneGUI()
    {
        Handles.DrawLine(maze.CellWorldPosition(0, 0), maze.CellWorldPosition(maze.UpperBoundX, 0));
        Handles.DrawLine(maze.CellWorldPosition(0, 0), maze.CellWorldPosition(0, maze.UpperBoundY));
        Handles.DrawLine(maze.CellWorldPosition(maze.UpperBoundX, maze.UpperBoundY), maze.CellWorldPosition(maze.UpperBoundX, 0));
        Handles.DrawLine(maze.CellWorldPosition(maze.UpperBoundX, maze.UpperBoundY), maze.CellWorldPosition(0, maze.UpperBoundY));
    }

    private void ForceEditorUpdate()
    {
        if (!Application.isPlaying)
        {
            EditorApplication.QueuePlayerLoopUpdate();
        }
    }
}
#endif