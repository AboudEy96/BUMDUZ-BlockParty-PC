using UnityEngine;

public class RagdollTest : MonoBehaviour
{
    private Animator animator;
    private Rigidbody[] allRigidbodies;
    public  GameObject map;
    public GameObject circle;
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

        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleRagdoll(false);
            Vector3 loc = transform.position + Vector3.down * 2f;
            Instantiate(circle, loc, Quaternion.identity);
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