using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public float minDistance = 1.0f;
    public float maxDistance = 4.0f;
    public float smooth = 10.0f;
    Vector3 dollyDir;
    public Vector3 dollyDirAdjusted;
    public float distance;

    private void Awake()
    {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    private void Update()
    {
        Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);
        RaycastHit hit;

        if (Physics.Linecast(transform.parent.position, desiredCameraPos, out hit))
        {
            distance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            distance = maxDistance;
        }
        transform.localPosition = Vector3.Lerp(transform.position, dollyDir * distance, Time.deltaTime * smooth);
    }
}
