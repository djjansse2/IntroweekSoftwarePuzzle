﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
	[Header("Input")]
	public	KeyCode			selectKey	= KeyCode.Mouse0;
	public	KeyCode			deleteKey	= KeyCode.Delete;

	/*
	 * Macros
	 */
	private	const string	NodeTag	= "Node";
	private	const string	PortTag	= "Port";


	private GameObject		_nodeDragging;
	private bool			_isDragging;

	private GameObject		_firstPort;
	private bool			_isLinking;
	private GameObject		_lineObject;

	private void Update()
	{
		/*
		 * If userselect key, handle user click
		 */
		if (Input.GetKeyDown(selectKey))
		{
			UserClickedMouse();
		}
		/*
		 * If user clicked delete key, handle delete
		 */
		if (Input.GetKeyDown(deleteKey))
		{
			DeleteNode();
		}
	}

	/*
	 * Handles mouse clicks from the user
	 */
	private void UserClickedMouse()
	{
		// Get the 3d mouse position in the "world" (relative to objects)
		Vector3 mousePosition3d	= Camera.main.ScreenToWorldPoint(Input.mousePosition);
		// Translate 3d mouse position to 2d mouse position
		Vector2 mousePosition	= new Vector2(mousePosition3d.x, mousePosition3d.y);

		// Traces a ray from the mouse straight along the Z axis (depth) and
		// returns the first hit
		RaycastHit2D hit		= Physics2D.Raycast(mousePosition, Vector2.zero);

		/*
		 * Check if an object has been hit by ray
		 */
		if (hit.collider != null)
		{
			// Handle click on object
			HandleClick(hit.transform);
		}
		else
		{
			// Handle click on nothing
			ClickedEmpty();
		}
	}

	/*
	 * Handles when the player clicked
	 * on an object
	 * 
	 * aObject	: object that was clicked on
	 */
	public void HandleClick(Transform aObjectClicked)
	{
		/*
		 * If object has dragable tag (nodes),
		 * handle dragging
		 * 
		 * if the user is already dragging a node
		 * the user would likely click on this node
		 */

		/*
		 * If user is dragging, stop dragging
		 */
		if (_isDragging)
		{
			// Reset dragging flag
			_isDragging = false;
			return;
		}

		if (aObjectClicked.CompareTag(NodeTag))
		{
			/*
			 * Only handle dragging when not
			 * linking.
			 */
			if (!_isLinking)
			{
				// Handle node moving
				DragNode(aObjectClicked.gameObject);
			}
		}
		/*
		 * If object is a port, handle node
		 * linking
		 */
		else if (aObjectClicked.CompareTag(PortTag))
		{
			// Handle linking
			LinkNodes(aObjectClicked.gameObject);
		}
	}

	/*
	 * Handles node moving
	 * 
	 * aNodeToMove	: nodeToMove
	 */
	public void DragNode(GameObject aNodeToMove)
	{
		/*
		 * If user is not dragging yet, start dragging
		 */
		if (!_isDragging)
		{
			// Save node to variable
			_nodeDragging = aNodeToMove;
			// Start dragging in coroutine (seperate task)
			StartCoroutine("MoveNode");
		}
	}

	/*
	 * Handles node linking
	 * 
	 * aPortToLink	: Port to handle
	 *				  linking on
	 */
	public void LinkNodes(GameObject aPortToLink)
	{
		/*
		 * Glossary:
		 * 
		 * Node	: Programming node, like start, end, if
		 *		  and read input;
		 * 
		 * Port	: Input or Output of a node
		 */

		// Get the port that was clicked
		Port portToLink = aPortToLink.GetComponent<Port>();

		/*
		 * If port is already linked, unlink it
		 */
		if (portToLink.isLinked)
		{
			portToLink.UnLink(true);
		}

		/*
		 * If linking is not already being done,
		 * start linking
		 */
		if (!_isLinking)
		{
			// Save the starting port to a variable
			_firstPort = aPortToLink;
			// Set linking flag
			_isLinking = true;

			// Create a new line
			_lineObject = (GameObject)Instantiate(Resources.Load("Line"));
			// Save line renderer to variable
			LineRenderer line = _lineObject.GetComponent<LineRenderer>();
			// Set first line position to port position
			line.SetPosition(0, aPortToLink.transform.position);
			// Start moving second line position to mouse
			StartCoroutine(MoveLine(line));
		}
		/*
		 * If linking has already been started, link
		 * first node to the node that was clicked on
		 */
		else
		{
			/*
			 * Ports can only be linked if one of them is
			 * an input and the other an output port
			 */
			if (_firstPort.GetComponent<Port>().isInput !=
				portToLink.isInput)
			{
				// Link the first selected port to the second selected
				_firstPort.GetComponent<Port>().Link(aPortToLink, _lineObject, 0);
				// Link the second selected port to the first
				portToLink.Link(_firstPort, _lineObject, 1);
				// Reset linking flag
				_isLinking = false;
				// Update the line on the second port
				portToLink.GetComponent<Port>().UpdateLine();
			}
		}
	}

	/*
	 * Deletes current dragging node
	 */
	private void DeleteNode()
	{
		/*
		 * If not dragging, stop deleting
		 */
		if (!_isDragging)
			return;

		_nodeDragging.GetComponent<Node>().DeleteNode();

		// Reset dragging
		_nodeDragging = default;
		_isDragging = false;
	}

	/*
	 * Stop linking ports
	 */
	public void StopLinking()
	{
		/*
		 * If linking hasn't been started,
		 * don't run this method
		 */
		if (!_isLinking)
			return;

		// Destroy the line being drawn
		Destroy(_lineObject);
		// Reset linking flag;
		_isLinking = false;
	}

	/*
	 * Handle user clicked nothing
	 */
	public void ClickedEmpty()
	{
		/*
		 * If node linking has been started,
		 * stop node linking
		 */
		if (_isLinking)
		{
			StopLinking();
		}
	}

	/*
	 * Corourinte for dragging a node
	 */
	public IEnumerator MoveNode()
	{
		// Set dragging flag
		_isDragging = true;

		// Save main camera to temporary variable
		Camera mainCam = Camera.main;

		// Get current mouse position
		Vector2 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);

		// Save node transform to temporary variable
		Transform nodeTransform = _nodeDragging.transform;

		// Get the node offset from the mouse
		float nodeXOffset = mousePosition.x - nodeTransform.position.x;
		float nodeYOffset = mousePosition.y - nodeTransform.position.y;

		/*
		 * Loop while a node is being dragged
		 */
		while (_isDragging)
		{
			// Update mouse position
			mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
			// Get new node position with offset
			Vector2 newNodePosition = mousePosition - new Vector2(nodeXOffset, nodeYOffset);

			// Round new position to single decimal point values (snapping)
			newNodePosition.x = Mathf.Round(newNodePosition.x * 10) / 10;
			newNodePosition.y = Mathf.Round(newNodePosition.y * 10) / 10;

			// Set node position to mouse position
			nodeTransform.position = newNodePosition;
			
			/*
			 * Update all line positions in dragging node
			 */
			foreach (Port port in _nodeDragging.GetComponentsInChildren<Port>())
			{
				port.UpdateLine();
			}

			yield return null;
		}
	}

	/*
	 * Move line position
	 * 
	 * aLine	: line to be moved
	 */
	public IEnumerator MoveLine(LineRenderer aLine)
	{		
		/*
		 * Move line while linking nodes
		 */
		while (_isLinking)
		{
			// Set second line position to mouse position
			aLine.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
			yield return null;
		}
	}
}
