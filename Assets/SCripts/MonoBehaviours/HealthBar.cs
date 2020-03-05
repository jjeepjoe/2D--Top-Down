using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //CONFIG PARAMS
    public HitPoints hitPoints;  //SCRIPTABLE OBJECT
    [HideInInspector] public Player character;  //HANDLE TO PLAYER SCRIPT SET IN CODE
    //PROPERTIES
    public Image meterImage;
    public Text hpText;
    //LOCAL VARIABLE
    float maxHitPoints;

    //STORE THE PLAYER MAX HIT POINTS
    private void Start()
    {
        maxHitPoints = character.maxHitPoints;
    }

    //ADJUST THE IMAGE FILL SLIDER PROPERTY 0-1 SO NEEDS TO BE A PERCENTAGE
    private void Update()
    {
        if(character != null)
        {
            //CALCULATE THE PERCENTAGE OF HEALTH / MAX POINTS TO MOVE THE SLIDER.
            meterImage.fillAmount = hitPoints.value / maxHitPoints;
            //MULTIPLY BY 100 FOR TEXT DISPLAY
            hpText.text = "HP:" + (meterImage.fillAmount * 100);
        }
    }
}
