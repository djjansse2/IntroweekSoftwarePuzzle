using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parser : MonoBehaviour
{
	////////// COMMAND MACROS //////////
	public const int CMD_WRITE		= 100;
	public const int CMD_READ		= 101;
	public const int CMD_JUMP		= 102;
	public const int CMD_IF			= 103;
	public const int CMD_ENDIF		= 104;
	public const int CMD_SET_MODE	= 105;

	////////// COMMUNICATION MACROS //////////
	public const int END_PROGRAM	= 199;



	public Node currentNode;
	public int ifCount;

    public static Parser	instance;

	private List<Command>	cmdList			= new List<Command>();

	private NotificationHandler notificationHandler;



	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("An instance of Parser already exists");
			return;
		}

		instance = this;
	}

	private void Start()
	{
		notificationHandler = NotificationHandler.instance;
	}

	public void Upload()
	{
		ResetParser();

		Node startNode = GetStartNode();

		if (startNode == default)
		{
			notificationHandler.NotifyError("No start node present");
			return;
		}

		if (!parseNodes(startNode)) return;
		Optimize();

		foreach (Command cmd in cmdList)
		{
			SerialCommunication.WriteToSerialPort(cmd.command);
		}

		SerialCommunication.WriteToSerialPort(END_PROGRAM);
	}

	public void ResetParser()
	{
		if (cmdList.Count > 0)
		{
			foreach(Command cmd in cmdList)
			{
				cmd.node.ResetNode();
			}
		}

		cmdList.Clear();
	}

	#region Node adding

	public void AddCommand(int aCmd, Node aNode)
	{
		cmdList.Add(new Command(aCmd, aNode));
	}

	public void AddCommand(int aCmd, int aLabel, Node aNode)
	{
		cmdList.Add(new Command(aCmd, aLabel, aNode));
	}

	#endregion

	private Node GetStartNode()
	{
		GameObject[] allNodes = GameObject.FindGameObjectsWithTag("Node");

		foreach (GameObject go in allNodes)
		{
			if (go.GetComponent<Node>().GetNodeType() == NodeType.START)
			{
				return go.GetComponent<Node>();
			}
		}

		return default;
	}

	private bool parseNodes(Node aStartNode)
	{
		if (aStartNode.nextNode == default)
		{
			notificationHandler.NotifyError("No nodes attached to start node");
			return false;
		}

		ifCount = 0;

		currentNode = aStartNode.nextNode;
		
		while (currentNode.GetNodeType() != NodeType.END)
		{
			if (currentNode.nextNode == default)
			{
				notificationHandler.NotifyError("Floating input found at: " + currentNode);
				return false;
			}

			if (currentNode.Parse())
				currentNode = currentNode.nextNode;
		}

		return true;
	}

	private void Optimize()
	{
		for (int i = 0; i < cmdList.Count; i++)
		{
			if (cmdList[i].command == CMD_IF)
			{
				for (int j = i; j < cmdList.Count; j++)
				{
					if (cmdList[j].command == CMD_JUMP &&
						cmdList[j].label == cmdList[i].label)
					{
						cmdList[i + 1] = new Command(j + 2, cmdList[i + 1].label, cmdList[i + 1].node);
						break;
					}
				}
			}

			if (cmdList[i].command == CMD_JUMP)
			{
				for (int j = i; j < cmdList.Count; j++)
				{
					if (cmdList[j].command == CMD_ENDIF &&
						cmdList[j].label == cmdList[i].label)
					{
						cmdList[i + 1] = new Command(j , cmdList[i + 1].label, cmdList[i + 1].node);
						break;
					}
				}
			}
		}

		return;
	}

	public Command FindIfWithLabel(int aLabel)
	{
		foreach (Command cmd in cmdList)
		{
			if (cmd.command == CMD_IF &&
				cmd.label == aLabel)
				return cmd;
		}

		return default;
	}
}

public struct Command
{
	public int	command;
	public int	label;
	public Node	node;

	public Command(int aCommand, Node aNode)
	{
		command = aCommand;
		label = 0;
		node = aNode;
	}

	public Command(int aCommand, int aLabel, Node aNode)
	{
		command = aCommand;
		label = aLabel;
		node = aNode;
	}
}
