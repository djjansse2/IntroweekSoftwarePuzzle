using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using UnityEngine;

public class SerialCommunication : MonoBehaviour
{
	public string				defaultCOMPort	= "com1";

	public delegate void DataReceivedCallback(int aData);
	public static DataReceivedCallback dataReceivedCallback;

    private static SerialPort	_serPort;

	private static string		_serPortID;

	private NotificationHandler	notificationHandler;

    private void Start()
    {
		notificationHandler = NotificationHandler.instance;

		OpenSerialPort();

		StartCoroutine("ReadSerialPort");
    }

	private void OnApplicationQuit()
	{
		_serPort.Close();
	}

	public static void SetSerialPortID(string aSerialPortNumber)
	{
		_serPortID = "com" + aSerialPortNumber;
	}

	public void OpenSerialPort()
	{
		if (_serPort != null) return;

		if (_serPortID == default)
		{
			_serPortID = defaultCOMPort;
			notificationHandler.NotifyWarning("Using Default COM port");
		}

		_serPort = new SerialPort(_serPortID, 9600);
		_serPort.DataReceived += new SerialDataReceivedEventHandler(SerialDataReceived);

		try
		{
			_serPort.Open();
		}
		catch (IOException ex)
		{
			notificationHandler.NotifyError(ex.Message);
		}
	}

	public static void WriteToSerialPort(int aMessage)
	{
		byte[] byteBuffer = new byte[] { (byte) aMessage };

		try
		{
			_serPort.Write(byteBuffer, 0, 1);
		}
		catch (IOException ex)
		{
			NotificationHandler.instance.NotifyError(ex.Message);
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

				if (dataReceivedCallback != null)
				{
					dataReceivedCallback.Invoke(buffer[0]);
				}
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
			notificationHandler.NotifyError(ex.Message);
		}
	}
}
