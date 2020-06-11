using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndNode : Node
{
	public override NodeType GetNodeType()
	{
		return NodeType.END;
	}

	public override void Parse()
	{
		
	}
}
