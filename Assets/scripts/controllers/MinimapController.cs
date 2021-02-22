using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct MinimapIcon
{
    public string name;
    public Image icon;
}

public class MinimapController : MonoBehaviour
{

    public Minimap minimap; // Minimap for the game
    public MinimapIcon[] minimapIcons;  // Icons to use for tracked objects

    private HashSet<GameObject> trackedObjects = new HashSet<GameObject>(); // Set of tracked objects for the minimap
    private Dictionary<GameObject, Image> trackedObjectIcons = new Dictionary<GameObject, Image>(); // Used to map objects to their icons

    // Update is called once per frame
    void Update()
    {
        moveTrackedObjects();
    }

    // Add a tracked object on the minimap
    public void AddTrackedObject(GameObject obj, string iconName)
    {
        // Find the icon
        Image imageIcon_p = null;
        foreach(MinimapIcon minimapIcon in minimapIcons)
        {
            if(minimapIcon.name == iconName)
            {
                imageIcon_p = minimapIcon.icon;
                break;
            }
        }

        // No image found
        if(imageIcon_p == null)
        {
            return;
        }

        // Already tracking
        if (trackedObjects.Contains(obj))
        {
            return;
        }

        // Instantiate the image and track
        Image imageIcon = Instantiate(imageIcon_p, minimap.transform);
        trackedObjects.Add(obj);
        trackedObjectIcons[obj] = imageIcon;
    }

    // Remove an object from the minimap
    public void RemoveTrackedObject(GameObject obj)
    {
        if (trackedObjects.Contains(obj))
        {
            Destroy(trackedObjectIcons[obj].gameObject);
            trackedObjects.Remove(obj);
            trackedObjectIcons.Remove(obj);
        }
    }

    // Set the limits of the map for minimap placement
    public void SetWorldLimits(float worldX1, float worldX2, float worldY1, float worldY2)
    {
        minimap.SetWorldLimits(worldX1, worldX2, worldY1, worldY2);
    }

    private void moveTrackedObjects()
    {
        foreach(GameObject obj in trackedObjects)
        {
            Image icon = trackedObjectIcons[obj];

            Vector3 newPosition = minimap.MapWorldPositionToMinimapPosition(obj.transform.position);
            newPosition.x *= -1;
            newPosition.x -= minimap.x2;
            newPosition.y -= minimap.y2;
            Quaternion newRotation = Quaternion.Euler(
                transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y,
                obj.transform.rotation.eulerAngles.y
            );

            icon.rectTransform.localPosition = newPosition;
            icon.rectTransform.localRotation = newRotation;
        }
    }

}
