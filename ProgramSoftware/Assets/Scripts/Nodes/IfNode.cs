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

		parser.AddCommand(Parser.CMD_IF, ++parser.ifCount, this);
		parser.AddCommand(parser.ifCount, this);
		return true;
	}
}
