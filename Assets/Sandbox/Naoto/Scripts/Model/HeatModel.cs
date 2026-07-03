using UnityEngine;

public class HeatModel
{
    public float Heat { get; private set; }

    public void AddHeat(float amount)
    {
        Heat += amount;
        if (Heat > 1f) Heat = 1f;
    }

    public void Cool(float amount)
    {
        Heat -= amount;
        if (Heat < 0f) Heat = 0f;
    }
}
