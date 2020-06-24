using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WriteNode : Node
{
	public InputField	portInput;
	public Toggle		portValue;

	public override NodeType GetNodeType()
	{
		return NodeType.PIN_MODE;
	}

	public override bool Parse()
	{
		Parser parser = Parser.instance;

		int value = (portValue.isOn) ? 1 : 0;
		
		parser.AddCommand(Parser.CMD_WRITE, this);
		parser.AddCommand(int.Parse(portInput.text), this);
		parser.AddCommand(value, this);

		return true;
	}
}
