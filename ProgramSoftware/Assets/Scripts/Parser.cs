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

	public	Node currentNode;
	public	int ifCount;

    public	static Parser	instance;

	public	List<Command>	cmdList			= new List<Command>();

	private NotificationHandler _notificationHandler;



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
		_notificationHandler = NotificationHandler.instance;
	}

	#region Node adding

	/*
	 * Adds a new command to the command list
	 * 
	 * aCmd		: Command
	 * aNode	: Node responsible for the command
	 * (aLabel)	: Label for command (used in optimizer)
	 */
	public void AddCommand(int aCmd, Node aNode)
	{
		cmdList.Add(new Command(aCmd, aNode));
	}

	public void AddCommand(int aCmd, int aLabel, Node aNode)
	{
		cmdList.Add(new Command(aCmd, aLabel, aNode));
	}

	#endregion

	#region Parsing

	/*
	 * Resets the parser
	 */
	public void ResetParser()
	{
		/*
		 * Check if the command list is empty
		 */
		if (cmdList.Count > 0)
		{
			/*
			 * Reset all nodes in command list
			 */
			foreach(Command cmd in cmdList)
			{
				cmd.node.ResetNode();
			}
		}

		// Empties list
		cmdList.Clear();
	}

	/*
	 * Finds the start node
	 * 
	 * Returns	: Start node
	 */
	private Node GetStartNode()
	{
		// Get all node objects
		GameObject[] allNodes = GameObject.FindGameObjectsWithTag("Node");

		/*
		 * Loop through all node objects to find
		 * start node
		 */
		foreach (GameObject go in allNodes)
		{
			if (go.GetComponent<Node>().GetNodeType() == NodeType.START)
			{
				return go.GetComponent<Node>();
			}
		}

		return default;
	}

	/*
	 * Parse all nodes
	 * 
	 * Returns	: Boolean, true when parsing succes
	 */
	public bool ParseNodes()
	{
		// Get start node
		Node startNode = GetStartNode();
		
		/*
		 * Check if start node exists, if not
		 * log the error and stop parsing
		 */
		if (startNode == default)
		{
			_notificationHandler.NotifyError("No start node present");
			return false;
		}
		/*
		 * Check if start node is connected to
		 * another node, if not log the error and
		 * stop parsing
		 */
		if (startNode.nextNode == default)
		{
			_notificationHandler.NotifyError("No nodes attached to start node");
			return false;
		}

		// Reset if counter
		ifCount = 0;

		// Set current node to start node
		currentNode = startNode.nextNode;
		
		/*
		 * Parse all nodes till the end node
		 */
		while (currentNode.GetNodeType() != NodeType.END)
		{
			/*
			 * Check if the next node of the current
			 * node is set, if not log error and stop
			 * parsing
			 */
			if (currentNode.nextNode == default)
			{
				_notificationHandler.NotifyError("Floating input found at: " + currentNode);
				return false;
			}

			/*
			 * If parsing of node returns true, set 
			 * current node to next node
			 */
			if (currentNode.Parse())
				currentNode = currentNode.nextNode;
		}

		return true;
	}

	#endregion

	#region Optimizer

	/*
	 * Optimize commands, second iteration of
	 * parsing
	 */
	public void Optimize()
	{
		/*
		 * Loop through all commands, optimize
		 * if and jump commands
		 */
		for (int i = 0; i < cmdList.Count; i++)
		{
			if (cmdList[i].command == CMD_IF)
			{
				OptimizeIf(i);
			}
			if (cmdList[i].command == CMD_JUMP)
			{
				OptimizeJump(i);
			}
		}
	}

	/*
	 * Optimizes if command
	 * 
	 * aIfPosition	: Position of the if command
	 *				  in the command list
	 */
	private void OptimizeIf(int aIfPosition)
	{
		/*
		 * Loop through all commands after if to find
		 * corelating jump command, set index of "if false
		 * jump" to the first real command after that
		 */
		for (int j = aIfPosition; j < cmdList.Count; j++)
		{
			if (cmdList[j].command == CMD_JUMP &&
				cmdList[j].label == cmdList[aIfPosition].label)
			{
				cmdList[aIfPosition + 1] = new Command(j + 2, cmdList[aIfPosition + 1].label, cmdList[aIfPosition + 1].node);
				return;
			}
		}
	}

	/*
	 * Optimizes jump command
	 * 
	 * aJumpPosition	: Position of the jump
	 *					  command in the command list
	 */
	private void OptimizeJump(int aJumpPosition)
	{
		/*
		 * Loop through all commands after jump command
		 * to find corelating endif command, when found
		 * set jump parameter to this position
		 */
		for (int j = aJumpPosition; j < cmdList.Count; j++)
		{
			if (cmdList[j].command == CMD_ENDIF &&
				cmdList[j].label == cmdList[aJumpPosition].label)
			{
				cmdList[aJumpPosition + 1] = new Command(j , cmdList[aJumpPosition + 1].label, cmdList[aJumpPosition + 1].node);
				return;
			}
		}
	}

	#endregion

	/*
	 * Finds an if command with a certain label
	 * 
	 * aLabel	: Label to look for
	 * 
	 * Returns	: If command with given label
	 */
	public Command FindIfWithLabel(int aLabel)
	{
		/*
		 * Loop through all commands in command list
		 * if command is IF and label matches, return
		 * command
		 */
		foreach (Command cmd in cmdList)
		{
			if (cmd.command == CMD_IF &&
				cmd.label == aLabel)
				return cmd;
		}

		return default;
	}
}

/*
 * Data container for commands
 * 
 * command	: Command code
 * label	: Label used in optimizer
 * node		: Node responsible for this command
 */
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
