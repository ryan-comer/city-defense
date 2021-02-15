using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public CinemachineFreeLook cinemachineCamera;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(cinemachineCamera);
    }


    public void SetFollowTarget(GameObject followTarget)
    {
        cinemachineCamera.Follow = followTarget.transform;
    }

    public void SetLookAtTarget(GameObject lookAtTarget)
    {
        cinemachineCamera.LookAt = lookAtTarget.transform;
    }

}
