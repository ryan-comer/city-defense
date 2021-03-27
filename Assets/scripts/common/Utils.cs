using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Determine if a layer is in a mask
    public static bool IsLayerInMask(int layer, LayerMask layerMask)
    {
        if (((1 << layer) & layerMask) != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
