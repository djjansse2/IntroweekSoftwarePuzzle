using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	[Header("Movement Settings")]
	public float	movespeed	= 1.0f;
	public KeyCode	moveKey		= KeyCode.Mouse2;

	[Header("Scrolling Settings")]
	public float	maxScroll	= 20;
	public float	minScroll	= 1;
	public float	scrollSpeed	= 1;

	private Camera	_camera;

	private void Start()
	{
		_camera = GetComponent<Camera>();
	}

	void Update()
    {
		if (isMouseOverGameWindow())
		{
			_camera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
			_camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, minScroll, maxScroll);
		}

        if (Input.GetKey(moveKey))
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			transform.position += new Vector3(-Input.GetAxis("Mouse X") * movespeed, -Input.GetAxis("Mouse Y") * movespeed, 0);
		}
		else if(Input.GetKeyUp(moveKey))
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
    }

	private bool isMouseOverGameWindow()
	{
		return !(0 > Input.mousePosition.x || 0 > Input.mousePosition.y || Screen.width < Input.mousePosition.x || Screen.height < Input.mousePosition.y);
	}
}
