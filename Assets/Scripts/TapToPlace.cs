using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

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

    private bool canSpawn = true;
    private bool uiBlock = false;
    private bool spawnLock = false;

    private static List<ARRaycastHit> hits = new();

    private float initialPinchDistance;
    private Vector3 initialScale;
    private float rotationSpeed = 0.2f;

    void Update()
    {
        HandleSpawn();
        HandleTouchManipulation();
    }

    // SPAWN
    void HandleSpawn()
    {
        if (!canSpawn || spawnedObject != null || uiBlock || spawnLock)
            return;

        // Touch Input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return;

            if (touch.phase == TouchPhase.Began)
            {
                spawnLock = true;
                TryPlace(touch.position);
            }
        }

        // Mouse Input (Editor Testing)
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject())
                return;

            spawnLock = true;
            TryPlace(Input.mousePosition);
        }

        // Unlock after release
        if (Input.touchCount == 0 && !Input.GetMouseButton(0))
        {
            spawnLock = false;
        }
    }

    void TryPlace(Vector2 screenPosition)
    {
        if (selectedObject == null)
            return;

        if (raycastManager.Raycast(screenPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;
            pose.position += Vector3.up * spawnHeightOffset;

            spawnedObject = Instantiate(
                selectedObject,
                pose.position,
                pose.rotation
            );

            canSpawn = false;

            placementManager?.SetPlacedObject(spawnedObject);
            colorChanger?.SetObject(spawnedObject);
        }
    }

    // SCALE + ROTATE
    void HandleTouchManipulation()
    {
        if (spawnedObject == null)
            return;

        if (Input.touchCount == 2)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            if (EventSystem.current != null &&
                (EventSystem.current.IsPointerOverGameObject(t1.fingerId) ||
                 EventSystem.current.IsPointerOverGameObject(t2.fingerId)))
                return;

            float currentDistance = Vector2.Distance(t1.position, t2.position);

            if (t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began)
            {
                initialPinchDistance = currentDistance;
                initialScale = spawnedObject.transform.localScale;
            }
            else if (initialPinchDistance > 0.01f)
            {
                float scaleFactor = currentDistance / initialPinchDistance;
                spawnedObject.transform.localScale = initialScale * scaleFactor;
            }

            Vector2 prevDir =
                (t1.position - t1.deltaPosition) -
                (t2.position - t2.deltaPosition);

            Vector2 currDir =
                t1.position - t2.position;

            float angle = Vector2.SignedAngle(prevDir, currDir);

            spawnedObject.transform.Rotate(
                0f,
                -angle * rotationSpeed,
                0f,
                Space.World
            );
        }
    }

    // OBJECT SELECTION
    public void SelectObject1()
    {
        selectedObject = object1;
    }

    public void SelectObject2()
    {
        selectedObject = object2;
    }

    // DELETE OBJECTS
    public void RemoveSpawnedObject()
    {
        GameObject[] allObjects =
            GameObject.FindGameObjectsWithTag("SpawnedObject");

        foreach (GameObject obj in allObjects)
        {
            Destroy(obj);
        }

        spawnedObject = null;
        canSpawn = true;
        spawnLock = false;

        placementManager?.SetPlacedObject(null);
        colorChanger?.SetObject(null);

        Debug.Log("All spawned objects deleted and reset.");
    }

    // UI SAFETY
    public void BlockUIForOneFrame()
    {
        StartCoroutine(UIBlockFrame());
    }

    IEnumerator UIBlockFrame()
    {
        uiBlock = true;
        yield return null;
        uiBlock = false;
    }
}