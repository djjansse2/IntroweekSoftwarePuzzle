using UnityEngine;
using System.Collections;

public class JoinNode : Node
{
	public Node altPreviousNode;

	public override NodeType GetNodeType()
	{
		return NodeType.JOIN;
	}

	public override void Link(Node aNodeToLinkTo, Port aPortToLinkFrom)
	{
		if (aPortToLinkFrom.isInput)
		{
			if (aPortToLinkFrom.isAlternative)
			{
				altPreviousNode = aNodeToLinkTo;
			}
			else
			{
				previousNode = aNodeToLinkTo;
			}
		}
		else
		{
			nextNode = aNodeToLinkTo;
		}
	}

	public override void Parse()
	{
		
	}
}
