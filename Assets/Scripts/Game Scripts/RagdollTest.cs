using UnityEngine;

public class RagdollTest : MonoBehaviour
{
    Animator animator;
    Rigidbody[] ragdollRigidbodies;
    Collider mainCollider;
    CharacterController characterController;

    void Awake()
    {
        animator = GetComponent<Animator>();
        mainCollider = GetComponent<Collider>();
        characterController = GetComponent<CharacterController>();

        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();

        SetRagdoll(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SetRagdoll(true);
        }
    }

    public void SetRagdoll(bool enable)
    {
        if (animator)
            animator.enabled = !enable;

        if (mainCollider)
            mainCollider.enabled = !enable;

        if (characterController)
            characterController.enabled = !enable;

        if (enable)
            transform.position = ragdollRigidbodies[0].transform.position;

        foreach (var rb in ragdollRigidbodies)
        {
            rb.isKinematic = !enable;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

}
