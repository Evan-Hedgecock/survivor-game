using Core.Entity;

namespace Core.Systems.States;
public abstract class State<T>(string name, T owner)
{
    public string Name
    {
        get { return _name.ToLower(); }
        set { _name = value.ToLower(); }
    }
    protected string _name = name.ToLower();
    protected T _owner = owner;
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update(float deltaTime);
}