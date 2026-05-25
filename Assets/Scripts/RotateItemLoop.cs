using UnityEngine;

public class RotateItemLoop : MonoBehaviour
{
    public Vector3 rotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frames
    void Update()
    {
        transform.Rotate(rotation * Time.deltaTime);
    }
}
