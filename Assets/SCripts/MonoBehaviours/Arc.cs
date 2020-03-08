using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arc : MonoBehaviour
{
    //CONFIG PARAMS

    public IEnumerator TravelArc(Vector3 destination, float duration)
    {
        var startPosition = transform.position;
        var percentComplete = 0.0f;
        /* 
         * 1.0F = 100 PERCENT, WE GET THE TIME SINCE LAST FRAME / DURATION IS AMOUNT OF TIME TO BE PERCENT.
         * Lerp(start pos, end pos, speed) 
         */
        while(percentComplete < 1.0f)
        {
            percentComplete += Time.deltaTime / duration;
            var currentHeight = Mathf.Sin(Mathf.PI * percentComplete);
            //Lerp : LINEAR INTERPOLATION, THIS WILL MAKE THE SHOOT MOVE IN THE SAME SPEED, RETURN A POINT.
            transform.position = Vector3.Lerp(startPosition, destination, percentComplete) + Vector3.up *
                currentHeight;
            //PAUSE UNTIL NEXT FRAME
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
