using UnityEngine;

public class ARPlacementManager : MonoBehaviour
{
    private GameObject placedObject;

    public void SetPlacedObject(GameObject obj)
    {
        placedObject = obj;
    }

    public GameObject GetPlacedObject()
    {
        return placedObject;
    }

    public bool HasObject()
    {
        return placedObject != null;
    }
}