using UnityEngine;

public class CubeController : MonoBehaviour, IControllable
{
    Collider _col;

    private void Awake()
    {
        _col = GetComponent<Collider>();    
    }
    public void AttackPressed()
    {
        Debug.Log($"{_col.name} Attacked");
    }

    public void JumpPressed()
    {
        throw new System.NotImplementedException();
    }

    public void OnPossessed()
    {
        throw new System.NotImplementedException();
    }

    public void OnUnpossessed()
    {
        throw new System.NotImplementedException();
    }

}
