using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Port : MonoBehaviour
{
	public bool			isInput;
	public bool			isAlternative;
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
		Node parentNode		= GetComponentInParent<Node>();
		Node nodeToLinkTo	= aPortToLinkTo.GetComponentInParent<Node>();
		lineObject			= aLineObject;
		lineIndex			= aLineIndex;

		portLinkedTo = aPortToLinkTo;

		parentNode.Link(nodeToLinkTo, this);

		// Set is linked flag
		isLinked = true;
	}

	/*
	 * Unlink this node from linked node
	 */
	public void UnLink(bool aUnlinkLinked)
	{
		/*
		 * If this node is not linked, don't
		 * run this method
		 */
		if (!isLinked)
			return;

		// Save parent node to variable
		Node parentNode	= GetComponentInParent<Node>();

		if (aUnlinkLinked)
		{
			// Unlink other port aswell
			portLinkedTo.GetComponent<Port>().UnLink(false);
		}

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
			parentNode.nextNode = default;
		}

		// Delete line
		Destroy(lineObject);

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
