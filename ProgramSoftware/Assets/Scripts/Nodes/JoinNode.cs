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
		// Link either input ports
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

		// If the parser hasn't visited this join yet
		if (!_isSaturated)
		{
			// Add jump command with if count as label
			parser.AddCommand(Parser.CMD_JUMP, parser.ifCount, this);
			// Add if count as argument
			parser.AddCommand(parser.ifCount, this);
			// Get the if node related to this join
			IfNode prevIf = (IfNode)parser.FindIfWithLabel(parser.ifCount).node;
			// Set the parsers node to parse back to the false stream of
			// the if node
			parser.currentNode = prevIf.altNextNode;
			// Mark this node as visited
			_isSaturated = true;
			return false;
		}
		else
		{
			// Add end if command with if count as label
			parser.AddCommand(Parser.CMD_ENDIF, parser.ifCount, this);
			// Decrease if count
			--parser.ifCount;
			return true;
		}
	}

	public override void ResetNode()
	{
		// Reset visited flag
		_isSaturated = false;
	}
}
