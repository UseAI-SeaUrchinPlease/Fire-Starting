using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    public GameObject branchItem;
    public GameObject branchHand;

    private CharacterController characterController;
    private BoxCollider branchBoxCollider;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        branchBoxCollider = branchItem.GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (branchBoxCollider.bounds.Intersects(characterController.bounds))
        {
            branchItem.SetActive(false);
            branchHand.SetActive(true);
        }
    }
}
