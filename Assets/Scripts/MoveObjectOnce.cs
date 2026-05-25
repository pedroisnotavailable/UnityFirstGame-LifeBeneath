using System.Collections;
using UnityEngine;

public class MoveObjectOnce : MonoBehaviour
{
    [Header("Movement Points")]
    public Transform startPoint;
    public Transform endPoint;

    [Header("Settings")]
    public float moveDuration = 2f;
    public bool moveOnStart = false;

    private bool isMoving = false;

    void Start()
    {
        // Optional fallback
        if (startPoint == null)
            startPoint = transform;

        if (moveOnStart)
        {
            TriggerMove();
        }
    }

    public void TriggerMove()
    {
        if (!isMoving && endPoint != null)
        {
            StartCoroutine(MoveOnce());
        }
    }

    IEnumerator MoveOnce()
    {
        isMoving = true;
        yield return Move(startPoint.localPosition, endPoint.localPosition);
        isMoving = false;
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
