using UnityEngine;

public class RotateOnce : MonoBehaviour
{
    public float speed = 2f;
    public Vector3 pos = new Vector3(0, 0, 0);

    private Quaternion targetRotation;
    public bool shouldRotateAtStartup = false;

    void Start()
    {
        targetRotation = Quaternion.Euler(transform.eulerAngles + pos);
    }

    void Update()
    {
        if (shouldRotateAtStartup)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * speed
            );
        }
    }

    public void TriggerRotate()
    {
        shouldRotateAtStartup = true;
    }
}