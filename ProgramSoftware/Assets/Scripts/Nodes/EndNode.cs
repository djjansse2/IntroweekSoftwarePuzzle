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
		// Notify that the node must not be deleted, and override default delete method
		NotificationHandler.instance.NotifyWarning("Do Not Delete End Node");
	}
}
