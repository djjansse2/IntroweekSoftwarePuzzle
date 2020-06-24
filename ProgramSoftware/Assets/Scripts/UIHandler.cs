using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
	private Parser				_parser;
	private NotificationHandler	_notificationHandler;

	////////// COMMUNICATION MACROS //////////
	public const int END_PROGRAM	= 199;
	public const int PROGRAM_SUCCES	= 198;

	private void Start()
	{
		_parser					= Parser.instance;
		_notificationHandler	= NotificationHandler.instance;
		// Subscribe feedback checker to data received callback
		SerialCommunication.dataReceivedCallback += CheckProgramFeedback;
	}

	/*
	 * Adds a node to the node field
	 * 
	 * aNodeName	: Node name of prefab in
	 *				  Resources folder
	 */
	public void AddNode(string aNodeName)
	{
		// Get full path to node in Resources folder
		string path = "Nodes/" + aNodeName;
		// Instantiate node from resources folder
		GameObject newNode = (GameObject)Instantiate(Resources.Load(path));
		// Set node position to mouse position
		newNode.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		// Set node to current dragging
		GetComponent<User>().DragNode(newNode);
	}

	/*
	 * Uploads the commands to the Arduino
	 */
	public void Upload()
	{
		// Clear the parser
		_parser.ResetParser();

		// Parse nodes, if returns false, stop uploading
		bool parseSucces = _parser.ParseNodes();
		
		if (!parseSucces)
			return;

		// Optimize parser (Second parsing iteration)
		_parser.Optimize();

		/*
		 * Loop through command list and send
		 * each command code as an individual byte
		 * over the serial bus
		 */
		foreach (Command cmd in _parser.cmdList)
		{
			SerialCommunication.WriteToSerialPort(cmd.command);
		}

		// Send end upload code over serial bus
		SerialCommunication.WriteToSerialPort(END_PROGRAM);
	}

	/*
	 * Clears all nodes in the node field
	 */
	public void ClearNodes()
	{
		// Get all node objects
		GameObject[] allNodes = GameObject.FindGameObjectsWithTag("Node");

		/*
		 * Loop through all nodes, if not
		 * End or Start node, delete node
		 */
		foreach (GameObject go in allNodes)
		{
			Node currentNode = go.GetComponent<Node>();
			if (currentNode.GetNodeType() != NodeType.START &&
				currentNode.GetNodeType() != NodeType.END)
			{
				currentNode.DeleteNode();
			}
		}
	}

	/*
	 * Check the response from the Arduino
	 * 
	 * aFeedback	: Response from Arduino
	 */
	public void CheckProgramFeedback(int aFeedback)
	{
		/*
		 * If the respond is succes, notify succes
		 */
		if (aFeedback == PROGRAM_SUCCES)
		{
			_notificationHandler.NotifySucces("Programming succes");
		}
	}
}
