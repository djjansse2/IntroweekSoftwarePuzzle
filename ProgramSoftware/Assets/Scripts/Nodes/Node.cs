using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
	START,
	END,
	IF,
	JOIN,
	READ_INPUT,
	WRITE,
	PIN_MODE
}

/*
 * BASE CLASS FOR ALL NODES
 */
public class Node : MonoBehaviour
{
    public Node	previousNode;
	public Node	nextNode;

	public virtual bool Parse() { return true; }

	/*
	 * Links two nodes
	 * 
	 * aNodeToLinkTo	: Node to link to
	 * aPortToLinkFrom	: Port used in linking
	 */
	public virtual void Link(Node aNodeToLinkTo, Port aPortToLinkFrom)
	{
		// If the port used is an input, set previous node
		if (aPortToLinkFrom.isInput)
		{
			previousNode = aNodeToLinkTo;
		}
		// If the port used is an output, set next node
		else
		{
			nextNode = aNodeToLinkTo;
		}
	}

	/*
	 * Getter for node type
	 */
	public virtual NodeType GetNodeType()
	{
		return default;
	}

	// Reset node values
	public virtual void ResetNode() { }

	// Delete this node
	public virtual void DeleteNode()
	{
		/*
		 * Unlink all the node ports
		 */
		foreach (Port p in GetComponentsInChildren<Port>())
		{
			p.UnLink(true);
		}

		// Destroy this node
		Destroy(this.gameObject);
	}
}
