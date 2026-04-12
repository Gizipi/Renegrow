using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class CameraNavigation : MonoBehaviour
{
	public float ArrowPanSpeed = 8f;

	public Vector2 BoundsMin = new Vector2(-8f, -8f);
	public Vector2 BoundsMax = new Vector2(8f, 8f);

	public float FocusPlaneZ;

	private Camera _camera;
	private Vector2 _lastMouseScreen;

	private void Awake()
	{
		_camera = GetComponent<Camera>();
	}

	private void Update()
	{
		Vector3 pos = transform.position;
		float dt = Time.deltaTime;

		Mouse mouse = Mouse.current;
		if (mouse != null)
		{
			if (mouse.middleButton.wasPressedThisFrame)
				_lastMouseScreen = mouse.position.ReadValue();

			if (mouse.middleButton.isPressed)
			{
				Vector2 screenNow = mouse.position.ReadValue();
				Vector3 worldLast = ScreenToWorldOnPlane(_lastMouseScreen);
				Vector3 worldNow = ScreenToWorldOnPlane(screenNow);
				pos += worldLast - worldNow;
				_lastMouseScreen = screenNow;
			}
		}

		Keyboard keyboard = Keyboard.current;
		if (keyboard != null)
		{
			Vector2 move = Vector2.zero;
			if (keyboard.leftArrowKey.isPressed)
				move.x -= 1f;
			if (keyboard.rightArrowKey.isPressed)
				move.x += 1f;
			if (keyboard.downArrowKey.isPressed)
				move.y -= 1f;
			if (keyboard.upArrowKey.isPressed)
				move.y += 1f;
			if (move.sqrMagnitude > 1f)
				move.Normalize();
			if (move.sqrMagnitude > 0f)
				pos += (Vector3)(move * (ArrowPanSpeed * dt));
		}

		pos = ClampToBounds(pos);
		transform.position = pos;
	}

	private Vector3 ScreenToWorldOnPlane(Vector2 screenPosition)
	{
		Vector3 p = new Vector3(screenPosition.x, screenPosition.y, 0f);
		p.z = Mathf.Abs(_camera.transform.position.z - FocusPlaneZ);
		return _camera.ScreenToWorldPoint(p);
	}

	private Vector3 ClampToBounds(Vector3 position)
	{
		float minX = Mathf.Min(BoundsMin.x, BoundsMax.x);
		float maxX = Mathf.Max(BoundsMin.x, BoundsMax.x);
		float minY = Mathf.Min(BoundsMin.y, BoundsMax.y);
		float maxY = Mathf.Max(BoundsMin.y, BoundsMax.y);
		position.x = Mathf.Clamp(position.x, minX, maxX);
		position.y = Mathf.Clamp(position.y, minY, maxY);
		return position;
	}
}
