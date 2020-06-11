﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
	START,
	END,
	IF,
	JOIN,
	READ_INPUT
}

public class Node : MonoBehaviour
{
    public Node	previousNode;
	public Node	nextNode;

	public virtual void Parse() { }

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
}
