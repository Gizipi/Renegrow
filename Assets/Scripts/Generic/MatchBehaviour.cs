public enum EMatchState
{
    Idle,
    Animating,
    Playing,
    Paused,
    GameOver,
}

public interface IMatchBehaviour
{
	EMatchState[] ActiveStates { get; }
	void Update(float deltaTime);
}
