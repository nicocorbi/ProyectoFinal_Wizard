using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPos = target.position + offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPos;
    }
}
