using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Port : MonoBehaviour
{
	public bool			isInput;
	public bool			isLinked;

	public GameObject	portLinkedTo;

	public GameObject	lineObject;
	public int			lineIndex;

	public void Link(GameObject aPortToLinkTo, GameObject aLineObject, int aLineIndex)
	{
		Node parentNode	= GetComponentInParent<Node>();
		lineObject		= aLineObject;
		lineIndex		= aLineIndex;

		portLinkedTo = aPortToLinkTo;

		if (isInput)
		{
			parentNode.previousNode = aPortToLinkTo.GetComponentInParent<Node>();
		}
		else
		{
			parentNode.nextNode = aPortToLinkTo.GetComponentInParent<Node>();
		}

		isLinked = true;
	}

	public void UnLink()
	{
		if (!isLinked)
			return;

		Node parentNode	= GetComponentInParent<Node>();

		portLinkedTo.GetComponent<Port>().UnLink();

		if (isInput)
		{
			parentNode.previousNode = default;
		}
		else
		{
			parentNode.previousNode = default;
		}

		isLinked = false;
	}

	public void UpdateLine()
	{
		if (!isLinked)
			return;

		lineObject.GetComponent<LineRenderer>().SetPosition(lineIndex, transform.position);
	}
}
