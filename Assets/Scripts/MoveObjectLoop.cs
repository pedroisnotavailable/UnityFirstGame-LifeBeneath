using System.Collections;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [Header("Movement Points")]
    public Transform startPoint;
    public Transform endPoint;

    [Header("Settings")]
    public float moveDuration = 2f;

    void Start()
    {
        // Optional fallback
        if (startPoint == null)
            startPoint = transform;

        StartCoroutine(MoveLoop());
    }

    IEnumerator MoveLoop()
    {
        while (true)
        {
            yield return Move(startPoint.localPosition, endPoint.localPosition);
            yield return Move(endPoint.localPosition, startPoint.localPosition);
        }
    }

    IEnumerator Move(Vector3 from, Vector3 to)
    {
        float timeElapsed = 0f;

        while (timeElapsed < moveDuration)
        {
            transform.localPosition = Vector3.Lerp(from, to, timeElapsed / moveDuration);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = to;
    }
}