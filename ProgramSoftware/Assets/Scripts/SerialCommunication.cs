using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using UnityEngine;

public class SerialCommunication : MonoBehaviour
{
    private static SerialPort _serPort;

    private void Start()
    {
        _serPort = new SerialPort("COM9", 9600);
		_serPort.DataReceived += new SerialDataReceivedEventHandler(SerialDataReceived);

		try
		{
			_serPort.Open();
		}
		catch (IOException ex)
		{
			Debug.LogError(ex);
		}
    }

    public static void WriteToSerialPort(string aMessage)
	{
		Debug.Log("Sent: " + aMessage);

		try
		{
			_serPort.Write(aMessage);
		}
		catch (IOException ex)
		{
			Debug.LogError(ex);
		}
	}

	public void SerialDataReceived(object sender, SerialDataReceivedEventArgs e)
	{
		try
		{
			Debug.Log("Received: " + _serPort.ReadExisting());
		}
		catch (IOException ex)
		{
			Debug.LogError(ex);
		}
	}
}
