using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using UnityEngine;

public class SerialCommunication : MonoBehaviour
{
	private readonly int		BAUD_RATE					= 9600;

	public string				defaultCOMPort				= "com1";

	public delegate void DataReceivedCallback(int aData);
	public static DataReceivedCallback dataReceivedCallback;

    private static SerialPort	_serPort;

	private static string		_serPortID;

	private NotificationHandler	notificationHandler;

    private void Start()
    {
		notificationHandler = NotificationHandler.instance;

		OpenSerialPort();

		// Start reading the serial port in a different test
		StartCoroutine("ReadSerialPort");
    }

	private void OnApplicationQuit()
	{
		// Close port for other connections on application
		// quit
		_serPort.Close();
	}

	/*
	 * Sets the COM port to use
	 * 
	 * aSerialPortNumber	: COM port
	 */
	public static void SetSerialPortID(string aSerialPortNumber)
	{
		// Add "com" prefix to number given and store to static
		// variable for later use
		_serPortID = "com" + aSerialPortNumber;
	}

	/*
	 * Opens the serial port with the
	 * stored COM port
	 */
	public void OpenSerialPort()
	{
		/*
		 * If a serial port already exists, stop
		 * trying to open
		 */
		if (_serPort != null) return;

		/*
		 * If no port ID was set, log a warning and use
		 * the default port ID
		 */
		if (_serPortID == default)
		{
			_serPortID = defaultCOMPort;
			notificationHandler.NotifyWarning("Using Default COM port");
		}

		// Create the serial port
		_serPort = new SerialPort(_serPortID, BAUD_RATE);
		// Set the data received handler (depricated)
		_serPort.DataReceived += new SerialDataReceivedEventHandler(SerialDataReceived);

		/*
		 * Open the serial port, if it fails
		 * log the error
		 */
		try
		{
			_serPort.Open();
		}
		catch (IOException ex)
		{
			notificationHandler.NotifyError(ex.Message);
		}
	}

	/*
	 * Write a number to the serial port
	 * 
	 * aMessage	: message to write
	 */
	public static void WriteToSerialPort(int aMessage)
	{
		// Convert message to array of a single byte
		// Array is needed for the SerialPort.Write() method
		byte[] byteBuffer = new byte[] { (byte) aMessage };

		/*
		 * Try writing to the serial port, if it fails
		 * log the error
		 */
		try
		{
			// Write 1 byte with offset 0 from byteBuffer to
			// the serial port
			_serPort.Write(byteBuffer, 0, 1);
		}
		catch (IOException ex)
		{
			NotificationHandler.instance.NotifyError(ex.Message);
		}
	}

	/*
	 * Coroutine for reading the serial port
	 */
	public IEnumerator ReadSerialPort()
	{
		/*
		 * Keep reading while the port is open
		 */
		while (_serPort.IsOpen)
		{
			/*
			 * Check if data is available
			 */
			if (_serPort.BytesToRead > 0)
			{
				// Read data
				byte[] buffer = new byte[1];
				 _serPort.Read(buffer, 0, 1);

				// Log data to console
				Debug.Log("Read: " + buffer[0]);

				// Invoke the data callback, if exists
				if (dataReceivedCallback != null)
				{
					dataReceivedCallback.Invoke(buffer[0]);
				}
			}

			yield return null;
		}
	}

	// Non functional data received handler
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
