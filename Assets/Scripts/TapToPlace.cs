using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class TapToPlace : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public ARPlacementManager placementManager;
    public ColorChanger colorChanger;

    [Header("Spawn Objects")]
    public GameObject object1;
    public GameObject object2;

    [Header("Spawn Settings")]
    public float spawnHeightOffset = 0.25f;

    private GameObject selectedObject;
    private GameObject spawnedObject;

    private static List<ARRaycastHit> hits = new();

    void Update()
    {
        if (spawnedObject != null)
            return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                TryPlace(touch.position);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            TryPlace(Input.mousePosition);
        }
    }

    void TryPlace(Vector2 screenPosition)
    {
        if (selectedObject == null)
        {
            Debug.Log("No object selected!");
            return;
        }

        if (raycastManager.Raycast(screenPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;

            pose.position += Vector3.up * spawnHeightOffset;

            spawnedObject = Instantiate(selectedObject, pose.position, pose.rotation);

            if (placementManager != null)
                placementManager.SetPlacedObject(spawnedObject);

            if (colorChanger != null)
                colorChanger.SetObject(spawnedObject);
        }
    }

    // BUTTON 1
    public void SelectObject1()
    {
        selectedObject = object1;
        Debug.Log("Object 1 Selected");
    }

    // BUTTON 2
    public void SelectObject2()
    {
        selectedObject = object2;
        Debug.Log("Object 2 Selected");
    }

    public void RemoveSpawnedObject()
    {
        if (spawnedObject != null)
        {
            Destroy(spawnedObject);
            spawnedObject = null;

            Debug.Log("Spawned object removed. You can place a new one now.");
        }
    }
}