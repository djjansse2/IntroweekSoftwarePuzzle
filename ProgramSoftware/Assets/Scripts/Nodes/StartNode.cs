using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNode : Node
{
	public override NodeType GetNodeType()
	{
		return NodeType.START;
	}

	public override bool Parse()
	{
		return true;
	}

	public override void DeleteNode()
	{
		NotificationHandler.instance.NotifyWarning("Do Not Delete Start Node");
	}
}
