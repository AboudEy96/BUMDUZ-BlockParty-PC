using UnityEngine;

public class RagdollTest : MonoBehaviour
{
    private Animator animator;
    private Rigidbody[] allRigidbodies;
    public  GameObject map;
    void Awake()
    {

        animator = GetComponent<Animator>();
        allRigidbodies = GetComponentsInChildren<Rigidbody>();
        
        ToggleRagdoll(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ToggleRagdoll(true);
            Destroy(map);
        }
    }

    public void ToggleRagdoll(bool isRagdoll)
    {
        if (animator != null)
            animator.enabled = !isRagdoll;
        
        foreach (Rigidbody rb in allRigidbodies)
        {
            rb.isKinematic = !isRagdoll;
        }
    }
}