using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{

    public HealthBar cityHealthBar; // Health bar for the city
    public Image cityHealthThresholdMarker;   // Used to represent the losing condition
    public float cityHealthThresholdMinX, cityHealthThresholdMaxX;  // Thresholds for positioning the threshold marker

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Move the city health threshold based on the percentage
    public void SetCityHealthThresholdMarker(float percentage)
    {
        percentage = Mathf.Clamp(percentage, 0.0f, 1.0f);

        cityHealthThresholdMarker.rectTransform.localPosition = new Vector3(
            Mathf.Lerp(cityHealthThresholdMinX, cityHealthThresholdMaxX, percentage),
            cityHealthThresholdMarker.rectTransform.localPosition.y,
            cityHealthThresholdMarker.rectTransform.localPosition.z
        );
    }

    public void SetCityHealthBar(float percentage)
    {
        percentage = Mathf.Clamp(percentage, 0.0f, 1.0f);

        cityHealthBar.SetPercentage(percentage);
    }

}
