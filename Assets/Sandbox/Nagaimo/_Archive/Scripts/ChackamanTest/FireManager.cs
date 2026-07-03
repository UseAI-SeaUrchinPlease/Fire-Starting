using UnityEngine;
using UnityEngine.UI;

public class FireManager : MonoBehaviour
{

    public Button enableButton;
    public Button disableButton;
    public ParticleSystem fireEffect;

    void setFireStatus(bool litFire)
    {
        var emission = fireEffect.emission;
        emission.enabled = litFire;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enableButton.onClick.AddListener(() => setFireStatus(true));
        disableButton.onClick.AddListener(() => setFireStatus(false));
    }
}
