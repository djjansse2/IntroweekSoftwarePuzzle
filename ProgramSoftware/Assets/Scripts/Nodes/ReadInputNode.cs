using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReadInputNode : Node
{
	public InputField	portInput;

	public override NodeType GetNodeType()
	{
		return NodeType.READ_INPUT;
	}

	public override bool Parse()
	{
		Parser parser = Parser.instance;
		
		parser.AddCommand(Parser.CMD_READ, this);
		parser.AddCommand(int.Parse(portInput.text), this);

		return true;
	}
}
