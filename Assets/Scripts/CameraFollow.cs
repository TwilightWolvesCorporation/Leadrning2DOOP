using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothing;
    [SerializeField] private Vector3 offSet;

    private void LateUpdate()
    {
        if (!target) return;
        var desirePos = target.position +
                        new Vector3(target.localRotation.y == 0 ? offSet.x : -offSet.x, offSet.y, offSet.z);
        transform.position = Vector3.Lerp(transform.position, desirePos, smoothing);
    }
}