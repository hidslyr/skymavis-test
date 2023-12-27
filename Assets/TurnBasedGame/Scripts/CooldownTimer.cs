using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownTimer
{
    private float cooldown;
    private float currentCoolDownTime;
    public void Init(float time)
    {
        cooldown = time;
        currentCoolDownTime = cooldown;
    }

    // Update is called once per frame
    public void Update(float dt)
    {
        currentCoolDownTime -= dt;
    }

    public bool IsCooleddown()
    {
        return currentCoolDownTime <= 0;
    }

    public void Cooldown()
    {
        currentCoolDownTime = cooldown;
    }
}
