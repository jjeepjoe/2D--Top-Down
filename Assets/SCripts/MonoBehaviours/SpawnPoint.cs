using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    //CONFIG PARAMS
    public GameObject prefabToSpawn;
    public float repeatInterval;

    //IF REPEAT-INTERVAL > 0 DO!
    private void Start()
    {
        if(repeatInterval > 0)
        {
            //THIS IS A MONO-BEHAVIOUR METHOD FOR REPEATING.
            //InvokeRepeating("method name", start wait time, next time)
            InvokeRepeating("SpawnObject", 0.0f, repeatInterval);
        }
    }
    //WE ARE GOING TO USE THIS TO REPEAT SPAWN BAD GUYS
    public GameObject SpawnObject()
    {
        if(prefabToSpawn != null)
        {
            return Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        }
        return null;
    }

}
