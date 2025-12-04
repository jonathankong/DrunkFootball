using UnityEngine;

public class ForwardGizmo : MonoBehaviour
{
    [SerializeField] private Color _color = Color.cyan;
    [SerializeField] private float _length = 2f;
    [SerializeField] private float _arrowHeadLength = 0.5f;
    [SerializeField] private float _arrowHeadAngle = 20f;

    private void OnDrawGizmos()
    {
        Gizmos.color = _color;

        Vector3 start = transform.position;
        Vector3 end = start + transform.forward * _length;

        // Main forward line
        Gizmos.DrawLine(start, end);

        // Arrow head
        DrawArrowHead(end);
    }

    private void DrawArrowHead(Vector3 arrowTip)
    {
        Vector3 direction = transform.forward;

        Quaternion rightRot = Quaternion.LookRotation(direction) *
                              Quaternion.Euler(0, 180 + _arrowHeadAngle, 0);
        Quaternion leftRot = Quaternion.LookRotation(direction) *
                             Quaternion.Euler(0, 180 - _arrowHeadAngle, 0);

        Vector3 right = arrowTip + (rightRot * Vector3.forward) * _arrowHeadLength;
        Vector3 left = arrowTip + (leftRot * Vector3.forward) * _arrowHeadLength;

        Gizmos.DrawLine(arrowTip, right);
        Gizmos.DrawLine(arrowTip, left);
    }
}
