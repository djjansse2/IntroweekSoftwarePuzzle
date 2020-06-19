using UnityEngine;
using System.Collections;

public class JoinNode : Node
{
	public Node altPreviousNode;
	private bool _isSaturated = false;

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

	public override bool Parse()
	{
		Parser parser = Parser.instance;

		if (!_isSaturated)
		{
			parser.AddCommand(Parser.CMD_JUMP, parser.ifCount, this);
			parser.AddCommand(parser.ifCount, this);
			IfNode prevIf = (IfNode)parser.FindIfWithLabel(parser.ifCount).node;
			parser.currentNode = prevIf.altNextNode;
			_isSaturated = true;
			return false;
		}
		else
		{
			parser.AddCommand(Parser.CMD_ENDIF, parser.ifCount, this);
			--parser.ifCount;
			return true;
		}
	}

	public override void ResetNode()
	{
		_isSaturated = false;
	}
}
