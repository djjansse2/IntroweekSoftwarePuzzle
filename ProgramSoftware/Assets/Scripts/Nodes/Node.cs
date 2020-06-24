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

public class Node : MonoBehaviour
{
    public Node	previousNode;
	public Node	nextNode;

	public virtual bool Parse() { return true; }

	public virtual void Link(Node aNodeToLinkTo, Port aPortToLinkFrom)
	{
		if (aPortToLinkFrom.isInput)
		{
			previousNode = aNodeToLinkTo;
		}
		else
		{
			nextNode = aNodeToLinkTo;
		}
	}

	public virtual NodeType GetNodeType()
	{
		return default;
	}

	public virtual void ResetNode() { }

	public virtual void DeleteNode()
	{
		foreach (Port p in GetComponentsInChildren<Port>())
		{
			p.UnLink(true);
		}

		Destroy(this.gameObject);
	}
}
