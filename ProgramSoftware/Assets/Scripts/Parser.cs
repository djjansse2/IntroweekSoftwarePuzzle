using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parser : MonoBehaviour
{
    public static Parser	instance;

	public List<Command>	cmdList;

	private void Awake()
	{
		if (instance != null)
		{
			Debug.LogError("An instance of Parser already exists");
			return;
		}

		instance = this;
	}

	public void Upload()
	{
		SerialCommunication.WriteToSerialPort("Sbubby");
	}
}

public struct Command
{
	public int label;
	public int command;
}
