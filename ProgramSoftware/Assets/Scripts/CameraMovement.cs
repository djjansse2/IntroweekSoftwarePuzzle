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
		// Save camera component to variable for performance
		_camera = GetComponent<Camera>();
	}

	void Update()
    {
		/*
		 * Zoom in and out using the mouse wheel,
		 * only when the cursor is positioned
		 * within the view window.
		 */
		if (isMouseOverGameWindow())
		{
			// Change the camera size on scroll
			_camera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
			// Clamp the camera size to remain between predefined values
			_camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, minScroll, maxScroll);
		}


		/*
		 * If the move key is pressed, disable
		 * the cursor and move the camera by
		 * moving the mouse.
		 * 
		 * (Default move key is middle mouse
		 * button)
		 */
        if (Input.GetKey(moveKey))
		{
			// Disable cursor movement
			Cursor.lockState = CursorLockMode.Locked;
			// Make cursor invisible
			Cursor.visible = false;
			// Move the camera
			transform.position += new Vector3(-Input.GetAxis("Mouse X") * movespeed, -Input.GetAxis("Mouse Y") * movespeed, 0);
		}
		/*
		 * When the move key is released,
		 * re-enable the cursor.
		 */
		else if(Input.GetKeyUp(moveKey))
		{
			// Free the cursor
			Cursor.lockState = CursorLockMode.None;
			// Make the cursor visible again
			Cursor.visible = true;
		}
    }

	/*
	 * Check whether the cursor is located
	 * within the view window
	 * 
	 * Returns	: Boolean whether camera is
	 *			  within view window.
	 */
	private bool isMouseOverGameWindow()
	{
		return !(0 > Input.mousePosition.x || 0 > Input.mousePosition.y || Screen.width < Input.mousePosition.x || Screen.height < Input.mousePosition.y);
	}
}
