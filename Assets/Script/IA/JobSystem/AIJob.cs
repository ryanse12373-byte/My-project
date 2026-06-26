using System;
using UnityEngine;

[Serializable]
public abstract class AIJob
{
    protected AIController controller;

    public AIJob(AIController controller)
    {
        this.controller = controller;
    }

    public abstract float GetPriority();

    public abstract bool CanRun();

    public abstract void Start();

    public abstract void Tick();

    public abstract void Stop();
}