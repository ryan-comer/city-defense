using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Image forground; // The forground image used for health
    public bool shouldFaceCamera;   // Should the health bar face the camera

    private void Update()
    {
        if (shouldFaceCamera)
        {
            facePlayer();
        }
    }

    // Set new health percentage for the bar
    public void SetPercentage(float percentage)
    {
        percentage = Mathf.Clamp(percentage, 0.0f, 1.0f);
        forground.rectTransform.localScale = new Vector3(
            percentage,
            forground.rectTransform.localScale.y,
            forground.rectTransform.localScale.z
        );
    }

    // Face the player so they can see the health
    private void facePlayer()
    {
        transform.LookAt(Camera.main.transform);
    }

}
