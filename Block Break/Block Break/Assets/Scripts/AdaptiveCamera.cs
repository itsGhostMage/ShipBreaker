using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Camera))]

public class AdaptiveCamera : MonoBehaviour
{

    [Header("Reference Resolution")]
    public Vector2 referenceResolution = new Vector2(1920, 1080); // 16:9 aspect ratio
    public float referenceOrthoSize = 5f;

    [Header("Scaling Mode")]
    public bool scaleToWidth = false; // If true, camera will scale based on width; otherwise, height

    [Header("Object Scaling")]
    public bool scaleObjects = true;
    public float baseScale = 1f;
    public List<Transform> objectsToScale = new List<Transform>();

    private Camera cam;
    private float referenceAspect;
    private float lastWidth;
    private float lastHeight;

#if UNITY_EDITOR
    [Header("Editor Tools")]
    public bool includeInactiveObjects = true;
    public bool showDebugLogs = true;
#endif

    void Awake()
    {
        cam = GetComponent<Camera>();
        referenceAspect = referenceResolution.x / referenceResolution.y;
        AdjustCamera();
        ScaleObjects();
    }

    void Update()
    {
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            lastWidth = Screen.width;
            lastHeight = Screen.height;
            AdjustCamera();
            ScaleObjects();
        }
    }

    void AdjustCamera()
    {
        lastWidth = Screen.width;
        lastHeight = Screen.height;

        float screenAspect = (float)Screen.width / Screen.height;

        if (cam.orthographic)
        {
            // Orthographic scaling
            if (scaleToWidth)
            {
                cam.orthographicSize = referenceOrthoSize * (referenceAspect / screenAspect);
            }
            else
            {
                // Scale based on height
                cam.orthographicSize = referenceOrthoSize;
            }
        }
        else
        {
            // Perspective camrera scaling
            float horizontalFOV = 2 * Mathf.Atan(Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad / 2) * screenAspect) * Mathf.Rad2Deg;
            float targetFOV = Mathf.Clamp(horizontalFOV, 60f, 120f);
            cam.fieldOfView = targetFOV;
        }
    }

    void ScaleObjects()
    {
        if (!scaleObjects) return;

        float scaleFactor = Mathf.Min((float)Screen.width / referenceResolution.x, (float)Screen.height / referenceResolution.y);

        foreach (Transform obj in objectsToScale)
        {
            if (obj != null)
            {
                obj.localScale = Vector3.one * baseScale * scaleFactor;
            }
        }
    }
#if UNITY_EDITOR
    [ContextMenu("Auto Collect Objects")]
    void CollectSceneObjects()
    {
        objectsToScale.Clear();
        int addedCount = 0;

        // Find objects by tag with more reliable search
        AddObjectsByTag("Player", "Player");
        AddObjectsByTag("Ship", "Enemy Ships");
        AddObjectsByTag("PowerUp", "PowerUps");
        AddObjectsByTag("Ball", "Ball");
        AddObjectsByTag("Obstacle", "Obstacles");

        if (showDebugLogs)
        {
            Debug.Log($"Total objects in scaling list: {objectsToScale.Count} (Added {addedCount} objects)");
        }

        void AddObjectsByTag(string tag, string logName)
        {
            // Use non-deprecated method with proper parameters
            GameObject[] foundObjects = GameObject.FindGameObjectsWithTag(tag);
            AddGameObjects(foundObjects, logName);
        }

        void AddGameObjects(GameObject[] objects, string logName)
        {
            if (objects == null || objects.Length == 0) return;

            foreach (GameObject obj in objects)
            {
                if (obj == null) continue;
                if (objectsToScale.Contains(obj.transform)) continue;

                objectsToScale.Add(obj.transform);
                addedCount++;

                if (showDebugLogs)
                {
                    Debug.Log($"Added {logName}: {obj.name}", obj);
                }
            }
        }
    }

    [ContextMenu("Print Current Tags")]
    void PrintCurrentTags()
    {
        Debug.Log("Current Tags in Project:");
        var allTags = UnityEditorInternal.InternalEditorUtility.tags;
        foreach (string tag in allTags)
        {
            Debug.Log($"- {tag}");
        }

        Debug.Log("Objects with tags in scene:");
        var allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (!string.IsNullOrEmpty(go.tag) && go.tag != "Untagged")
            {
                Debug.Log($"{go.name} - {go.tag}", go);
            }
        }
    }


    [ContextMenu("Add Children of Spawn Manager")]
    void AddChildrenOfSpawnManager()
    {
        SpawnManager spawnManager = FindFirstObjectByType<SpawnManager>();
        if (spawnManager != null)
        {
            for (int i = 0; i < spawnManager.transform.childCount; i++)
            {
                Transform child = spawnManager.transform.GetChild(i);
                if (!objectsToScale.Contains(child))
                {
                    objectsToScale.Add(child);
                    Debug.Log($"Added child: {child.name}");
                }
            }
        }
    }
#endif
}