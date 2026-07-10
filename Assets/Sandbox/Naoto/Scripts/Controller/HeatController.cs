using UnityEngine;

public class HeatController : MonoBehaviour
{
    [SerializeField] float heatPower = 0.5f;
    [SerializeField] float cooling = 0.2f;

    HeatModel model = new HeatModel();

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            model.AddHeat(heatPower * Time.deltaTime);
        }

        model.Cool(cooling * Time.deltaTime);

        Debug.Log(model.Heat);
    }
}
