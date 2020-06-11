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
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			if (_isDragging)
				_isDragging = false;
			else
				UserClickedMouse();
		}
	}

	private void UserClickedMouse()
	{
		Vector3 mousePosition3d	= Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 mousePosition	= new Vector2(mousePosition3d.x, mousePosition3d.y);

		RaycastHit2D hit		= Physics2D.Raycast(mousePosition, Vector2.zero);

		if (hit.collider != null)
		{
			HandleClick(hit.transform);
		}
		else
		{
			ClickedEmpty();
		}
	}

	public void HandleClick(Transform aObjectClicked)
	{
		if (aObjectClicked.CompareTag("Dragable"))
		{
			if (!_isLinking)
			{
				_nodeDragging = aObjectClicked.parent.gameObject;
				StartCoroutine("DragNode");
			}
		}
		else if (aObjectClicked.CompareTag("Port"))
		{
			if (!_isDragging)
			{
				LinkNodes(aObjectClicked.gameObject);
			}
		}
	}

	public void LinkNodes(GameObject aPortToLink)
	{
		Port portToLink = aPortToLink.GetComponent<Port>();

		if (portToLink.isLinked)
		{
			portToLink.UnLink();
		}

		if (!_isLinking)
		{
			_firstPort = aPortToLink;
			_isLinking = true;

			_lineObject = (GameObject)Instantiate(Resources.Load("Line"));
			LineRenderer line = _lineObject.GetComponent<LineRenderer>();
			line.SetPosition(0, aPortToLink.transform.position);
			StartCoroutine(MoveLine(line));
		}
		else
		{
			if (_firstPort.GetComponent<Port>().isInput !=
				portToLink.isInput)
			{
				_firstPort.GetComponent<Port>().Link(aPortToLink, _lineObject, 0);
				portToLink.Link(_firstPort, _lineObject, 1);
				_isLinking = false;
				portToLink.GetComponent<Port>().UpdateLine();
			}
		}
	}

	public void StopLinking()
	{
		if (!_isLinking)
			return;

		Destroy(_lineObject);
		_isLinking = false;
	}

	public void ClickedEmpty()
	{
		if (_isLinking)
		{
			StopLinking();
		}
	}

	public void AddNode(GameObject aNewNode)
	{
		_nodeDragging = (GameObject)Instantiate(aNewNode);
		_nodeDragging.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		StartCoroutine("DragNode");
	}

	public IEnumerator DragNode()
	{
		_isDragging = true;

		while (_isDragging)
		{
			_nodeDragging.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			
			foreach (Port port in _nodeDragging.GetComponentsInChildren<Port>())
			{
				port.UpdateLine();
			}

			yield return null;
		}
	}

	public IEnumerator MoveLine(LineRenderer aLine)
	{		
		while (_isLinking)
		{
			aLine.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
			yield return null;
		}
	}
}
