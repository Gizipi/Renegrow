using UnityEngine;

public class MouseTracker : IMatchBehaviour 
{
	private readonly Board _board;
	private readonly Camera _cameraOverride;
	private readonly LayerMask _layerMask;

	public EMatchState[] ActiveStates { get; } = { EMatchState.Playing };

	public Vector3 MouseWorldPosition { get; private set; }
	public RaycastHit2D LastHit { get; private set; }
	public CircleCollider2D HoveredCircleCollider { get; private set; }
	public Collider2D HoveredCollider { get; private set; }

	public MouseTracker(Board board, Camera cameraOverride = null, LayerMask layerMask = default)
	{
		_board = board;
		_cameraOverride = cameraOverride;
		_layerMask = layerMask == default ? ~0 : layerMask;
	}

	public void Update(float deltaTime) {
		Camera cam = _cameraOverride != null ? _cameraOverride : Camera.main;
		if (cam == null)
			return;

		Vector3 mouseScreen = Input.mousePosition;
		mouseScreen.z = -cam.transform.position.z;
		MouseWorldPosition = cam.ScreenToWorldPoint(mouseScreen);

		Ray ray = cam.ScreenPointToRay(Input.mousePosition);
		LastHit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, _layerMask);

		HoveredCollider = LastHit.collider;
		HoveredCircleCollider = HoveredCollider as CircleCollider2D;
	}
}
