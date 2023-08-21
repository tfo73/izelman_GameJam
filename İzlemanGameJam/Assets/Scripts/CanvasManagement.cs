using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManagement : MonoBehaviour
{
    [Header("Countdown")] //Çalışmıyor
    float currentTime = 0;
    public float startingTimeMinute, startingTimeSecond;
    [SerializeField] Text CountdownText;

    void Start(){
        currentTime = startingTimeSecond + (startingTimeMinute * 60);
    }

    void Update(){
        currentTime -= 1 * Time.deltaTime;
        CountdownText.text = currentTime.ToString("0");

        if(currentTime <= 0){
            currentTime = 0;
        }
    }
}