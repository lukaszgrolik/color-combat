using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EngineTime
{
    private float deltaTime = 0f;
    public float DeltaTime => deltaTime;

    private float time = 0f;
    public float Time => time;

    private float timeScale = 0f;
    public float TimeScale => timeScale;

    public void AddDeltaTime(float val)
    {
        deltaTime = val;
        time += val;
    }

    public void SetTime(float val)
    {
        time = val;
    }

    public void SetTimeScale(float val)
    {
        timeScale = val;
    }

    public float SecondsTo(float val) { return val - time; }
    public float SecondsFrom(float val) { return time - val; }
    public bool IsBefore(float val) { return time < val; }
    public bool IsBeforeOrSame(float val) { return time <= val; }
    public bool IsAfter(float val) { return time > val; }
    public bool IsAfterOrSame(float val) { return time >= val; }
}
