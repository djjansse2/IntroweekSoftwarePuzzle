using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
	START,
	END
}

public abstract class Node : MonoBehaviour
{
    public Node	previousNode;
	public Node	nextNode;

	public abstract void Parse();
	public abstract NodeType GetNodeType();
}
