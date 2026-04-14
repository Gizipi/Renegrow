using UnityEngine;
using System.Threading.Tasks;

public class MatchState
{
	const float WAIT_TIMEOUT = 10f;
	private EMatchState _state = EMatchState.Idle;
	public EMatchState State => _state;

	public void SetState(EMatchState state)
	{
		_state = state;
	}

	public async Task WaitTillState(EMatchState state, float timeout = WAIT_TIMEOUT)
	{
		float startTime = Time.time;
		while (_state != state && Time.time - startTime < timeout)
		{
			await Task.Delay(100);
		}
	}
}
