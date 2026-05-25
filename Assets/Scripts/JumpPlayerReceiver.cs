using UnityEngine;

public class JumpPlayerReceiver : MonoBehaviour
{
    public float verticalVelocity;

    public void Boost(float force)
    {
        verticalVelocity = force;
    }
}
