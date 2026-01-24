using UnityEngine;

public class SpawnPointGizmo : MonoBehaviour
{
    [Header("What will spawn here")]
    [SerializeField] private GameObject _prefab;

    [Header("Preview settings")]
    [SerializeField] private Color _gizmoColor = new Color(1f, 1f, 1f, 0.3f);
    [SerializeField] private float _previewScale = 1f;
    [SerializeField] private Vector3 _offset = Vector3.zero;

    private void OnDrawGizmos()
    {
        if (_prefab == null)
            return;

        // Try to find a mesh on the prefab
        var meshFilter = _prefab.GetComponentInChildren<MeshFilter>();
        if (meshFilter == null || meshFilter.sharedMesh == null)
            return;

        var mesh = meshFilter.sharedMesh;

        // Set a transform matrix for the gizmo
        var matrix = Matrix4x4.TRS(
            transform.position + _offset,
            transform.rotation,
            Vector3.one * _previewScale
        );

        Gizmos.matrix = matrix;

        // Solid-ish ghost mesh
        Gizmos.color = _gizmoColor;
        Gizmos.DrawMesh(mesh);

        // Wireframe outline to make it clearer
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireMesh(mesh);

        // Reset matrix afterward (good habit)
        Gizmos.matrix = Matrix4x4.identity;
    }
}