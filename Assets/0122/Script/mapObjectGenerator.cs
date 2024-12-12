using UnityEngine;

public static class mapObjectGenerator
{
    // Manifold Garden style colors with emission values
    private static Color[] baseColors = new Color[]
    {
        new Color(0.95f, 0.23f, 0.23f), // Bright Red
        new Color(0.23f, 0.95f, 0.23f), // Bright Green
        new Color(0.23f, 0.23f, 0.95f), // Bright Blue
        new Color(0.95f, 0.95f, 0.23f), // Bright Yellow
        new Color(0.95f, 0.23f, 0.95f), // Bright Magenta
        new Color(0.23f, 0.95f, 0.95f)  // Bright Cyan
    };

    private static Color[] emissionColors = new Color[]
    {
        new Color(1.0f, 0.1f, 0.1f), // Red glow
        new Color(0.1f, 1.0f, 0.1f), // Green glow
        new Color(0.1f, 0.1f, 1.0f), // Blue glow
        new Color(1.0f, 1.0f, 0.1f), // Yellow glow
        new Color(1.0f, 0.1f, 1.0f), // Magenta glow
        new Color(0.1f, 1.0f, 1.0f)  // Cyan glow
    };

    // Updated Create methods to include style parameters
    public static GameObject CreateCube(float size, int colorIndex, bool useManifoldStyle = true, float emissionIntensity = 0.5f, float edgeWidth = 0.03f)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = "MapObject_Cube";
        cube.transform.localScale = Vector3.one * size;

        SetupMaterial(cube, colorIndex, useManifoldStyle, emissionIntensity);
        if (useManifoldStyle) AddEdgeHighlight(cube, edgeWidth);

        return cube;
    }

    public static GameObject CreateStairs(float size, int colorIndex, bool useManifoldStyle = true, float emissionIntensity = 0.5f, float edgeWidth = 0.03f)
    {
        GameObject stairs = new GameObject("MapObject_Stairs");
        int steps = 5;
        float stepSize = size / steps;

        for (int i = 0; i < steps; i++)
        {
            GameObject step = GameObject.CreatePrimitive(PrimitiveType.Cube);
            step.transform.parent = stairs.transform;
            step.transform.localPosition = new Vector3(i * stepSize, i * stepSize / 2, 0);
            step.transform.localScale = new Vector3(stepSize, stepSize / 2, size);

            SetupMaterial(step, colorIndex, useManifoldStyle, emissionIntensity);
            if (useManifoldStyle) AddEdgeHighlight(step, edgeWidth);
        }

        return stairs;
    }

    public static GameObject CreatePlatform(float size, int colorIndex, bool useManifoldStyle = true, float emissionIntensity = 0.5f, float edgeWidth = 0.03f)
    {
        GameObject platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
        platform.name = "MapObject_Platform";
        platform.transform.localScale = new Vector3(size * 2, size * 0.2f, size * 2);

        SetupMaterial(platform, colorIndex, useManifoldStyle, emissionIntensity);
        if (useManifoldStyle) AddEdgeHighlight(platform, edgeWidth);

        return platform;
    }

    // Add similar parameters to other Create methods (Bridge, Tower, Archway)...

    private static void SetupMaterial(GameObject obj, int colorIndex, bool useManifoldStyle, float emissionIntensity)
    {
        var renderer = obj.GetComponent<MeshRenderer>();
        Material material;

        if (useManifoldStyle)
        {
            material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            material.SetColor("_BaseColor", baseColors[colorIndex]);

            // Enable emission
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", emissionColors[colorIndex] * emissionIntensity);

            // Set metallic and smoothness
            material.SetFloat("_Metallic", 0.0f);
            material.SetFloat("_Smoothness", 0.5f);

            // Set up renderer settings
            renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes;
            renderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.BlendProbes;
        }
        else
        {
            // Original simple material setup
            material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            material.SetColor("_BaseColor", baseColors[colorIndex]);
        }

        renderer.material = material;
    }

    private static void AddEdgeHighlight(GameObject obj, float edgeWidth)
    {
        var edges = obj.AddComponent<LineRenderer>();

        // Create material for edges
        var edgeMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        edgeMaterial.color = Color.white;

        // Enable emission for edges
        edgeMaterial.EnableKeyword("_EMISSION");
        edgeMaterial.SetColor("_EmissionColor", Color.white);

        edges.material = edgeMaterial;
        edges.startWidth = edgeWidth;
        edges.endWidth = edgeWidth;
        edges.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        edges.receiveShadows = false;

        // Create outline points with slight offset
        Vector3 scale = obj.transform.localScale / 1.98f;
        Vector3[] points = new Vector3[]
        {
            new Vector3(-scale.x, scale.y, -scale.z),
            new Vector3(scale.x, scale.y, -scale.z),
            new Vector3(scale.x, scale.y, scale.z),
            new Vector3(-scale.x, scale.y, scale.z),
            new Vector3(-scale.x, scale.y, -scale.z)
        };

        edges.positionCount = points.Length;
        edges.SetPositions(points);
    }

    public static GameObject CreateRoom(float size, int colorIndex, bool useManifoldStyle = true, float emissionIntensity = 0.5f, float edgeWidth = 0.03f)
    {
        GameObject room = new GameObject("MapObject_Room");

        // Floor
        GameObject floor = CreatePlatform(size * 1.5f, colorIndex, useManifoldStyle, emissionIntensity, edgeWidth);
        floor.transform.parent = room.transform;

        // Walls
        for (int i = 0; i < 4; i++)
        {
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.transform.parent = room.transform;
            wall.transform.localPosition = Quaternion.Euler(0, i * 90, 0) * Vector3.forward * size * 1.5f;
            wall.transform.localScale = new Vector3(size * 3, size * 2, size * 0.2f);
            wall.transform.rotation = Quaternion.Euler(0, i * 90, 0);

            SetupMaterial(wall, colorIndex, useManifoldStyle, emissionIntensity);
            if (useManifoldStyle) AddEdgeHighlight(wall, edgeWidth);
        }

        return room;
    }

    public static Color GetColor(int colorIndex)
    {
        if (colorIndex >= 0 && colorIndex < baseColors.Length)
        {
            return baseColors[colorIndex];
        }
        return Color.white;
    }
}
