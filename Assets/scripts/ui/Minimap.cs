using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{

    [HideInInspector]
    public float x1, x2, y1, y2;    // Used for the minimap part of the mapping

    private float worldX1, worldX2, worldY1, worldY2;   // Used for the city part of the mapping

    private void Start()
    {
        setMinimapLimits();
    }

    public void SetWorldLimits(float worldX1, float worldX2, float worldY1, float worldY2)
    {
        this.worldX1 = worldX1;
        this.worldX2 = worldX2;
        this.worldY1 = worldY1;
        this.worldY2 = worldY2;
    }

    private void setMinimapLimits()
    {
        RectTransform rect = GetComponent<RectTransform>();
        x1 = -1 * rect.sizeDelta.x / 2;
        y1 = -1 * rect.sizeDelta.y / 2;
        x2 = rect.sizeDelta.x / 2;
        y2 = rect.sizeDelta.y / 2;
    }

    // Map the objects position to a position on the minimap
    public Vector3 MapWorldPositionToMinimapPosition(Vector3 worldPosition)
    {
        Vector3 returnVector;

        float percentX = Mathf.InverseLerp(worldX1, worldX2, worldPosition.x);
        float percentY = Mathf.InverseLerp(worldY1, worldY2, worldPosition.z);

        returnVector = new Vector3(
            Mathf.Lerp(x1, x2, percentX),
            Mathf.Lerp(y1, y2, percentY),
            0.0f
        );

        return returnVector;
    }


}
