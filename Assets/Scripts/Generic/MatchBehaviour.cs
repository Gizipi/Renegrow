public enum EMatchState
{
    Idle,
    Playing,
    Paused,
    GameOver,
}

public interface IMatchBehaviour
{
	EMatchState[] ActiveStates { get; }
	void Update(float deltaTime);
}
