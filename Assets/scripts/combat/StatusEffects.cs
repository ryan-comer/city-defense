using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATUS_EFFECT
{
    BURN,
    STUN,
    BLIND
}

public class StatusEffects : MonoBehaviour
{

    private Dictionary<STATUS_EFFECT, System.DateTime> statusEffectTimes = new Dictionary<STATUS_EFFECT, System.DateTime>();
    private Dictionary<STATUS_EFFECT, float> statusEffectDurations = new Dictionary<STATUS_EFFECT, float>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkClearStatusEffect(); 
    }

    // Add a new status effect for this entity
    public void AddStatus(STATUS_EFFECT statusEffect, float duration)
    {
        statusEffectTimes[statusEffect] = System.DateTime.Now;
        statusEffectDurations[statusEffect] = duration;
    }

    // Clear a status from this entity
    public void ClearStatus(STATUS_EFFECT statusEffect)
    {
        if (statusEffectTimes.ContainsKey(statusEffect))
        {
            statusEffectTimes.Remove(statusEffect);
        }

        if (statusEffectDurations.ContainsKey(statusEffect))
        {
            statusEffectDurations.Remove(statusEffect);
        }
    }

    // Check if the entity has a status effect
    public bool CheckStatus(STATUS_EFFECT statusEffect)
    {
        return statusEffectTimes.ContainsKey(statusEffect);
    }

    // See if any status effects can be cleared
    private void checkClearStatusEffect()
    {
        System.DateTime currentTime = System.DateTime.Now;

        foreach(STATUS_EFFECT statusEffect in statusEffectTimes.Keys)
        {
            float secondsPassed = (float)(currentTime - statusEffectTimes[statusEffect]).TotalSeconds;
            if(secondsPassed > statusEffectDurations[statusEffect])
            {
                ClearStatus(statusEffect);
            }
        }
    }

}
