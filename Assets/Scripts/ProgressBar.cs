using UnityEngine;
using UnityEngine.UI;

public class ProgressBar
{
	public delegate void OnProgressCompleted();
	public event OnProgressCompleted onProgressCompleted;
	private void ProgressCompleted()
	{
		if (onProgressCompleted != null)
			onProgressCompleted();
	}
	private float _progress = 0f;
	public float Progress => _progress;
	private readonly Slider _slider;
	private readonly ProgressData _progressData;
	private WinCondition _currentWinCondition = null;

	public ProgressBar(Slider slider, ProgressData progressData)
	{
		_slider = slider;
		_progressData = progressData;
	}

	public void Activate(IBoard board) {
		if(_currentWinCondition != null) {
			Deactivate();
		}
		_progress = 0f;
		_slider.value = _progress;
		int randIndex = Random.Range(0, _progressData.WinConditions.Length);
		_currentWinCondition = _progressData.WinConditions[randIndex];
		_progressData.WinConditions[randIndex].Initialize(board);
		_progressData.WinConditions[randIndex].onProgressChanged += OnProgressChanged;

		_slider.image.sprite = _currentWinCondition.icon;
	}

	public void Deactivate() {
		if(_currentWinCondition == null) {
			return;
		}
		_progress = 0f;
		_slider.value = _progress;
		_currentWinCondition.onProgressChanged -= OnProgressChanged;
		_currentWinCondition = null;
	}

	private void OnProgressChanged(float progress) {
		_progress = progress;
		_slider.value = progress;
		if(progress >= 1f) {
			ProgressCompleted();
		}
	}
}
