# Enviro3

## セットアップ

- Document.pdfを参照

## タイムラプスの実装

``` cs[TimeLapser.cs]
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelapseTester : MonoBehaviour
{
    public float timelapseLength = 8f;
    public float normalSpeed = 5f;
    public float fastSpeed = 0.5f;
    private bool timelapse;
    private double startTime;
    private double endTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("押された");
            StartTimelapse();
            timelapse = true;
        }


        if (timelapse && EnviroSkyLite.instance.currentTimeInHours >= endTime)
        {
            EndTimelapse();
            timelapse = false;
        }
    }

    public void StartTimelapse()
    {
        EnviroSkyLite.instance.GameTime.cycleLengthInMinutes = fastSpeed;
        startTime = EnviroSkyLite.instance.currentTimeInHours;
        endTime = startTime + timelapseLength;
        Debug.Log(endTime - startTime);
    }

    public void EndTimelapse()
    {
        EnviroSkyLite.instance.GameTime.cycleLengthInMinutes = normalSpeed;
    }
}
```
