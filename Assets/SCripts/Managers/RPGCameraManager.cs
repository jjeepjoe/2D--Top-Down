using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RPGCameraManager : MonoBehaviour
{
    //CONFIG PARAMS
    public static RPGCameraManager sharedInstance = null;
    [HideInInspector] public CinemachineVirtualCamera virtualCamera;

    //SINGLETON FOR THE CAMERA CONNECTION
    private void Awake()
    {
        if(sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            sharedInstance = this;
        }
        //GET A HANDLE TO THE VIRTUAL CAMERA WITH TAG
        GameObject vCamGameObject = GameObject.FindWithTag("VirtualCamera");
        virtualCamera = vCamGameObject.GetComponent<CinemachineVirtualCamera>();
    }
}
