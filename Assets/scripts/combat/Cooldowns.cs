using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldowns : MonoBehaviour
{

    private Dictionary<string, System.DateTime> cooldownsLastUsedTime = new Dictionary<string, System.DateTime>();  // Last time the cooldown was used
    private Dictionary<string, float> cooldowns = new Dictionary<string, float>();  // Last time the cooldown was used

    // Register a new cooldown to track
    public void RegisterCooldown(string name, float cooldown)
    {
        cooldownsLastUsedTime[name] = System.DateTime.MinValue;
        cooldowns[name] = cooldown;
    }

    public float GetTimeLeft(string name)
    {
        float timeSinceUsed = (float) (System.DateTime.Now - cooldownsLastUsedTime[name]).TotalSeconds;
        if(timeSinceUsed > cooldowns[name])
        {
            return 0.0f;
        }
        else
        {
            return Mathf.Max(cooldowns[name] - timeSinceUsed, 0.0f);
        }
    }

    public void StartCooldown(string name)
    {
        cooldownsLastUsedTime[name] = System.DateTime.Now;
    }
}
