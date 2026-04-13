using UnityEngine;

[System.Serializable]
public abstract class WinCondition : ScriptableObject {
	public Sprite icon;
	public delegate void OnProgressChanged(float progress);
	public event OnProgressChanged onProgressChanged;
	protected void ProgressChanged(float progress)
	{
		if (onProgressChanged != null)
			onProgressChanged(progress);
	}

	protected IBoard _board;

	public virtual void Initialize(IBoard board){
		_board = board;
	}
}
