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

	/*
	 * Link this port to another
	 * 
	 * aPortToLinkTo	: Other port
	 * aLineObject		: Line between ports
	 * aLineIndex		: Position on the line (start or end)
	 */
	public void Link(GameObject aPortToLinkTo, GameObject aLineObject, int aLineIndex)
	{
		Node parentNode	= GetComponentInParent<Node>();
		lineObject		= aLineObject;
		lineIndex		= aLineIndex;

		portLinkedTo = aPortToLinkTo;

		/*
		 * If this is an input port, set the
		 * previous node on parent node
		 */
		if (isInput)
		{
			parentNode.previousNode = aPortToLinkTo.GetComponentInParent<Node>();
		}
		/*
		 * If this is an output port, set
		 * the next node on parent node
		 */
		else
		{
			parentNode.nextNode = aPortToLinkTo.GetComponentInParent<Node>();
		}

		// Set is linked flag
		isLinked = true;
	}

	/*
	 * Unlink this node from linked node
	 */
	public void UnLink()
	{
		/*
		 * If this node is not linked, don't
		 * run this method
		 */
		if (!isLinked)
			return;

		// Save parent node to variable
		Node parentNode	= GetComponentInParent<Node>();

		// Unlink other port aswell
		portLinkedTo.GetComponent<Port>().UnLink();

		/*
		 * If this is an input port, reset
		 * the parent previous node
		 */
		if (isInput)
		{
			parentNode.previousNode = default;
		}
		/*
		 * If this in an output port,
		 * reset the parent next node
		 */
		else
		{
			parentNode.previousNode = default;
		}

		// Reset is linked flag
		isLinked = false;
	}

	/*
	 * Updates the position of the line
	 */
	public void UpdateLine()
	{
		/*
		 * If this node isn't linked,
		 * don't run this method
		 */
		if (!isLinked)
			return;

		// Update the line
		lineObject.GetComponent<LineRenderer>().SetPosition(lineIndex, transform.position);
	}
}
