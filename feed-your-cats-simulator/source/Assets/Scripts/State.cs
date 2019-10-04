public abstract class State<T> {
    protected T self;

    public virtual void Tick() { }
    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }

    public State(T self) {
        this.self = self;
    }
}