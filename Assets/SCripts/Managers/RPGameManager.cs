using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGameManager : MonoBehaviour
{
    //CONFIG PARMS
    /*
     * This Class we are making a STATIC varaible to hold an Instance of the same class.
     * NOTE: the Static will ensure that this only is available to this class, which should only
     * be one copy.
     * AWAKE: we check to see if a second is becoming awake, if not set this as the variable.
     */
    public static RPGameManager sharedInstance = null;
    public SpawnPoint playerSpawnPoint;
    public RPGCameraManager cameraManager;

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
    }
    //WE START THE PLAYER ONCE THE SCRIPT LOADS.
    private void Start()
    {
        SetupScene();
    }
    //
    public void SetupScene()
    {
        SpawnPlayer();
    }
    //CHECK FIRST > THEN, WE CALL THE METHOD ON THE SPAWN SCRIPT.
    //WE MODIFIED TO INCLUDE THE CAMERA ACTION
    public void SpawnPlayer()
    {
        if(playerSpawnPoint != null)
        {
            GameObject player = playerSpawnPoint.SpawnObject();
            cameraManager.virtualCamera.Follow = player.transform;
        }
    }
}
