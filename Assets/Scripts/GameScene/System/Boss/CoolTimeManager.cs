using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CoolTimeID // coolTimeにはいくつかのスレッドがあります
{
    Slot0, // PallarelArrow, EmissionArrow
    Slot1, // Beam
    Slot2, // ArrowBom, EmissionArrow
    Slot3  // SingleArrow
}

public class CoolTimeManager
{
    readonly Dictionary<CoolTimeID, float> coolTimes = new();

    public CoolTimeManager()
    {
        foreach (CoolTimeID id in Enum.GetValues(typeof(CoolTimeID)))
        {
            coolTimes.Add(id, 0f);
        }
    }

    public void Tick(float deltaTime)
    {
        foreach (var key in coolTimes.Keys.ToList())
        {
            coolTimes[key] = Mathf.Max(0, coolTimes[key] - deltaTime);
        }
    }

    public bool IsReady(CoolTimeID id)
    {
        return coolTimes[id] <= 0f;
    }

    public void Reset(CoolTimeID id, float time)
    {
        coolTimes[id] = time;
    }
}
