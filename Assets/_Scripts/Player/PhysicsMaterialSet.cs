using UnityEngine;

[CreateAssetMenu(menuName = "Physics/MaterialSet")]
public class PhysicsMaterialSet : ScriptableObject
{
    public PhysicsMaterial[] materials;  // Array of materials ordered by slipperiness
    public float[] healthThresholds;    // Corresponding health thresholds (descending order)
}