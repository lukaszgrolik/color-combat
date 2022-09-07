using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameMode<T> where T : MonoBehaviour, IRegistry, IAgentTypesProvider
{
    protected T topMbScript;

    public GameMode(T topMbScript)
    {
        this.topMbScript = topMbScript;
    }

    public abstract void OnStart();

    public virtual void OnUpdate()
    {

    }
}