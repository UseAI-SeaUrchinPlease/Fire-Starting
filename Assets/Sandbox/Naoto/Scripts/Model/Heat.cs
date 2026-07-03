using UnityEngine;

public class Heat
{
    public float HeatState { get; private set; }

    public void AddHeat(float amount)
    {
        HeatState += amount;
        if (HeatState > 1f) HeatState = 1f;
    }

    public void Cool(float amount)
    {
        HeatState -= amount;
        if (HeatState < 0f) HeatState = 0f;
    }
}
