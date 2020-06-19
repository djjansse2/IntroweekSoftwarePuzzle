﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using UnityEngine;

public class SerialCommunication : MonoBehaviour
{
    private static SerialPort _serPort;

    private void Start()
    {
        _serPort = new SerialPort("COM7", 9600);
		_serPort.DataReceived += new SerialDataReceivedEventHandler(SerialDataReceived);

		try
		{
			_serPort.Open();
		}
		catch (IOException ex)
		{
			Debug.LogError(ex);
		}

		StartCoroutine("ReadSerialPort");
    }

	private void OnApplicationQuit()
	{
		_serPort.Close();
	}

	public static void WriteToSerialPort(int aMessage)
	{
		Debug.Log("Sent: " + aMessage);

		byte[] byteBuffer = new byte[] { (byte) aMessage };

		try
		{
			_serPort.Write(byteBuffer, 0, 1);
		}
		catch (IOException ex)
		{
			Debug.LogError(ex);
		}
	}

	public IEnumerator ReadSerialPort()
	{
		while (_serPort.IsOpen)
		{
			if (_serPort.BytesToRead > 0)
			{
				byte[] buffer = new byte[1];

				 _serPort.Read(buffer, 0, 1);

				Debug.Log("Read: " + buffer[0]);
			}

			yield return null;
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
