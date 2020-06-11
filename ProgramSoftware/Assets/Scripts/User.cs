using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
	private GameObject	_nodeDragging;
	private bool		_isDragging;

	private GameObject	_firstPort;
	private bool		_isLinking;
	private GameObject	_lineObject;

	private void Update()
	{
		/*
		 * If user clicked left mouse button,
		 * handle user click
		 */
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			UserClickedMouse();
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
		 */
		if (aObjectClicked.CompareTag("Dragable"))
		{
			/*
			 * Only handle dragging when not
			 * linking.
			 */
			if (!_isLinking)
			{
				// Save node to variable
				_nodeDragging = aObjectClicked.parent.gameObject;
				// Start dragging in coroutine (seperate task)
				StartCoroutine("DragNode");
			}
		}
		/*
		 * If object is a port, handle node
		 * linking
		 */
		else if (aObjectClicked.CompareTag("Port"))
		{
			/*
			 * Check whether a node is being dragged
			 */
			if (!_isDragging)
			{
				// Handle linking
				LinkNodes(aObjectClicked.gameObject);
			}
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
			portToLink.UnLink();
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
		/*
		 * If a node is being dragged, stop
		 * dragging and leave node there
		 */
		if (_isDragging)
		{
			_isDragging = false;
		}
	}

	/*
	 * Creates a new node
	 * 
	 * aNewNode	: Node to be created
	 */
	public void AddNode(GameObject aNewNode)
	{
		// Immediately start dragging new node
		_nodeDragging = (GameObject)Instantiate(aNewNode);
		StartCoroutine("DragNode");
	}

	/*
	 * Corourinte for dragging a node
	 */
	public IEnumerator DragNode()
	{
		// Set dragging flag
		_isDragging = true;

		/*
		 * Loop while a node is being dragged
		 */
		while (_isDragging)
		{
			// Set node position to mouse position
			_nodeDragging.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			
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
