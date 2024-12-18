using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

#if UNITY_EDITOR
public class mapObjectGeneratorWindow : EditorWindow
{
    private StructureType selectedType = StructureType.CustomPrefab;
    private float size = 10f;
    private int colorIndex = 0;
    private bool autoConnect = true;

    // Prefab fields
    private GameObject singlePrefab;
    [SerializeField] private List<GameObject> prefabList = new List<GameObject>();
    private bool useMultiplePrefabs = false;
    private int prefabCount = 0;

    private enum StructureType
    {
        CustomPrefab,
        Cube,
        Stairs,
        Platform,
        Bridge,
        Tower,
        Archway
    }

    [MenuItem("Window/Map Object Generator")]
    public static void ShowWindow()
    {
        GetWindow<mapObjectGeneratorWindow>("Map Object Generator");
    }

    void OnGUI()
    {
        GUILayout.Label("Map Structure Generator", EditorStyles.boldLabel);
        EditorGUILayout.Space(10);

        selectedType = (StructureType)EditorGUILayout.EnumPopup("Structure Type:", selectedType);

        if (selectedType == StructureType.CustomPrefab)
        {
            EditorGUILayout.Space(5);
            GUILayout.Label("Prefab Settings", EditorStyles.boldLabel);

            useMultiplePrefabs = EditorGUILayout.Toggle("Use Multiple Prefabs", useMultiplePrefabs);

            if (useMultiplePrefabs)
            {
                // Multiple prefabs interface
                prefabCount = EditorGUILayout.IntField("Number of Prefabs", prefabCount);

                // Resize list if needed
                while (prefabList.Count < prefabCount)
                    prefabList.Add(null);
                while (prefabList.Count > prefabCount)
                    prefabList.RemoveAt(prefabList.Count - 1);

                // Show prefab fields
                for (int i = 0; i < prefabList.Count; i++)
                {
                    prefabList[i] = (GameObject)EditorGUILayout.ObjectField(
                        $"Prefab {i + 1}:",
                        prefabList[i],
                        typeof(GameObject),
                        false
                    );
                }
            }
            else
            {
                // Single prefab interface
                singlePrefab = (GameObject)EditorGUILayout.ObjectField(
                    "Custom Prefab:",
                    singlePrefab,
                    typeof(GameObject),
                    false
                );
            }
        }

        EditorGUILayout.Space(10);
        size = EditorGUILayout.Slider("Size:", size, 0.1f, 50f);
        colorIndex = EditorGUILayout.IntSlider("Color:", colorIndex, 0, 5);
        autoConnect = EditorGUILayout.Toggle("Auto Connect:", autoConnect);

        EditorGUILayout.Space(20);
        if (GUILayout.Button("Generate Structure"))
        {
            GenerateStructure();
        }
    }

    void GenerateStructure()
    {
        GameObject structure = null;

        if (selectedType == StructureType.CustomPrefab)
        {
            if (useMultiplePrefabs && prefabList != null && prefabList.Count > 0)
            {
                // Filter out null entries
                List<GameObject> validPrefabs = prefabList.FindAll(p => p != null);

                if (validPrefabs.Count > 0)
                {
                    GameObject selectedPrefab = validPrefabs[Random.Range(0, validPrefabs.Count)];
                    structure = GenerateCustomPrefab(selectedPrefab);
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Please assign at least one prefab!", "OK");
                    return;
                }
            }
            else if (singlePrefab != null)
            {
                structure = GenerateCustomPrefab(singlePrefab);
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Please assign a prefab!", "OK");
                return;
            }
        }
        else
        {
            // Original structure generation
            switch (selectedType)
            {
                case StructureType.Cube:
                    structure = mapObjectGenerator.CreateCube(size, colorIndex);
                    break;
                case StructureType.Stairs:
                    structure = mapObjectGenerator.CreateStairs(size, colorIndex);
                    break;
                    // ... other cases
            }
        }

        if (structure != null)
        {
            Selection.activeGameObject = structure;
            Undo.RegisterCreatedObjectUndo(structure, "Generate Structure");
        }
    }

    GameObject GenerateCustomPrefab(GameObject prefab)
    {
        if (prefab == null) return null;

        // Instantiate the prefab
        GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

        // Apply scale
        instance.transform.localScale = Vector3.one * size;

        // Apply color if the prefab has a renderer
        Renderer[] renderers = instance.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            material.SetColor("_BaseColor", mapObjectGenerator.GetColor(colorIndex));
            renderer.material = material;
        }

        return instance;
    }
}
#endif