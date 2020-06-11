using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNode : Node
{
	public override NodeType GetNodeType()
	{
		return NodeType.START;
	}

	public override void Parse()
	{
		
	}
}
