using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndNode : Node
{
	public override NodeType GetNodeType()
	{
		return NodeType.END;
	}

	public override bool Parse()
	{
		return false;
	}

	public override void DeleteNode()
	{
		NotificationHandler.instance.NotifyWarning("Do Not Delete End Node");
	}
}
