using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfNode : Node
{
	public Node altNextNode;

	public override NodeType GetNodeType()
	{
		return NodeType.IF;
	}

	public override void Link(Node aNodeToLinkTo, Port aPortToLinkFrom)
	{
		if (aPortToLinkFrom.isInput)
		{
			previousNode = aNodeToLinkTo;
		}
		else
		{
			if (aPortToLinkFrom.isAlternative)
			{
				altNextNode = aNodeToLinkTo;
			}
			else
			{
				nextNode = aNodeToLinkTo;
			}
		}
	}

	public override bool Parse()
	{
		Parser parser = Parser.instance;

		// Increment if count and add if command with if count as label
		parser.AddCommand(Parser.CMD_IF, ++parser.ifCount, this);
		// Add if counter as argument
		parser.AddCommand(parser.ifCount, this);
		return true;
	}
}
