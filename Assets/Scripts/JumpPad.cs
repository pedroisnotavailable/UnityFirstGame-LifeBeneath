using UnityEngine;
using StarterAssets;
using System.Collections.Generic;

public class JumpPad : MonoBehaviour
{
    public float jumpForce = 15f;
    public float boostedJumpHeight = 3.5f;
    public bool requirePlayerTag = true;

    private readonly Dictionary<ThirdPersonController, float> originalJumpHeights = new Dictionary<ThirdPersonController, float>();

    private void OnTriggerEnter(Collider other)
    {
        if (requirePlayerTag && !other.CompareTag("Player"))
        {
            return;
        }

        ThirdPersonController thirdPersonController = other.GetComponentInParent<ThirdPersonController>();
        if (thirdPersonController != null)
        {
            if (!originalJumpHeights.ContainsKey(thirdPersonController))
            {
                originalJumpHeights.Add(thirdPersonController, thirdPersonController.JumpHeight);
            }

            thirdPersonController.JumpHeight = boostedJumpHeight;
        }

        Rigidbody rb = other.GetComponentInParent<Rigidbody>();
        if (rb != null)
        {
            Vector3 v = rb.linearVelocity;

            if (v.y < 0)
                v.y = 0;

            rb.linearVelocity = v;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (requirePlayerTag && !other.CompareTag("Player"))
        {
            return;
        }

        ThirdPersonController thirdPersonController = other.GetComponentInParent<ThirdPersonController>();
        if (thirdPersonController != null && originalJumpHeights.TryGetValue(thirdPersonController, out float originalJumpHeight))
        {
            thirdPersonController.JumpHeight = originalJumpHeight;
            originalJumpHeights.Remove(thirdPersonController);
        }
    }
}
