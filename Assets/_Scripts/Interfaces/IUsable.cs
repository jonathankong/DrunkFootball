using System;
public interface IUsable
{
    public event Action OnItemUse;
    void Use();
}
