using UnityEngine;
using UnityEngine.InputSystem;

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
	private SpriteButton _hoveredSpriteButton;

	public MouseTracker(Board board, Camera cameraOverride = null, LayerMask layerMask = default)
	{
		_board = board;
		_cameraOverride = cameraOverride;
		_layerMask = layerMask == default ? ~0 : layerMask;
	}

	public void Update(float deltaTime)
	{
		Camera cam = _cameraOverride != null ? _cameraOverride : Camera.main;
		if (cam == null)
			return;

		Pointer pointer = Pointer.current;
		if (pointer == null)
		{
			if (_hoveredSpriteButton != null)
			{
				_hoveredSpriteButton.OnHoverEnd();
			}
			_hoveredSpriteButton = null;
			return;
		}

		Vector2 screenPoint = pointer.position.ReadValue();
		Vector3 mouseScreen = new Vector3(screenPoint.x, screenPoint.y, 0f);
		mouseScreen.z = -cam.transform.position.z;
		MouseWorldPosition = cam.ScreenToWorldPoint(mouseScreen);

		Ray ray = cam.ScreenPointToRay(screenPoint);
		LastHit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, _layerMask);

		HoveredCollider = LastHit.collider;

		if (HoveredCollider != null)
		{
			HoverLogic();
			if (pointer.press.wasPressedThisFrame)
			{
				Debug.Log("MouseTracker: Player clicked");
				_hoveredSpriteButton.OnClick();
			}
		}
		else
		{
			if (_hoveredSpriteButton != null)
			{
				_hoveredSpriteButton.OnHoverEnd();
			}
			_hoveredSpriteButton = null;
		}
	}

	private void HoverLogic()
	{
		SpriteButton spriteButton = HoveredCollider.GetComponent<SpriteButton>();

		if (spriteButton != _hoveredSpriteButton)
		{
			if (_hoveredSpriteButton != null)
			{
				_hoveredSpriteButton.OnHoverEnd();
			}
			if (spriteButton != null)
			{
				spriteButton.OnHoverStart();
			}
		}
		_hoveredSpriteButton = spriteButton;
	}
}
