using System;
using System.IO;
using System.IO.Ports;
using System.Device.Location;
using System.Threading;
using NMEAParser;


public class Parse
{
    static bool _continue;
    static SerialPort _serialPort;
    

    //static Vessel pinniped = SignalK.vessel("235078477");
   static Vessel pinniped = new Vessel(); 

    public static void Main()
    {
 //       string name;
        string message;
        StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
        Thread readThread = new Thread(Read);

        // Create a new SerialPort object with default settings.
        _serialPort = new SerialPort();

        // Allow the user to set the appropriate properties.
        _serialPort.PortName = "Com10";
//        _serialPort.PortName = SetPortName(_serialPort.PortName);
//        _serialPort.BaudRate = 4800;
        _serialPort.BaudRate = 115200;

//        _serialPort.BaudRate = SetPortBaudRate(_serialPort.BaudRate);
//        _serialPort.Parity = SetPortParity(_serialPort.Parity);
//        _serialPort.DataBits = SetPortDataBits(_serialPort.DataBits);
//        _serialPort.StopBits = SetPortStopBits(_serialPort.StopBits);
//        _serialPort.Handshake = SetPortHandshake(_serialPort.Handshake);

        // Set the read/write timeouts
        _serialPort.ReadTimeout = 500;
        _serialPort.WriteTimeout = 500;
        try
        {
            _serialPort.Open();
            _continue = true;
            readThread.Start();



//          Console.Write("Name: ");
//          name = Console.ReadLine();

            Console.WriteLine("Self: {0}, Source: {1}, Verson: {2}, TimeStamp: {3}",SignalK.self,SignalK.source,SignalK.version,SignalK.timestamp);

            while (_continue)
            {
                message = Console.ReadLine();

                if (stringComparer.Equals("quit", message))
                {
                    _continue = false;
                }
                else
                {
                    //              _serialPort.WriteLine(String.Format("<{0}>: {1}", name, message));
                }
            }
            readThread.Join();
            _serialPort.Close();
        }
        catch (System.IO.IOException) { Console.WriteLine("Com port does not exist or in use"); }
    }

    public static void Read()
    {
        string nmealogname = string.Format("NMEALog_{0}{1:d2}{2:d2}.txt",DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day);
        using(StreamWriter nmealog = File.AppendText(nmealogname))
        {
            while (_continue)
            {
                try
                {
                    string message = _serialPort.ReadLine();
                    if (message.Length > 7)
                    {
                        NMEA0183.parseNmea0183(message,pinniped);
                        string logmessage= string.Format("{0}:{1:d3}UTC;{2}", DateTime.UtcNow,DateTime.UtcNow.Millisecond, message);
                        nmealog.WriteLine(logmessage);
                    }
                }
                catch (TimeoutException) { }
            }
            nmealog.Flush();
        }
    }


    // Display Port values and prompt user to enter a port. 
    public static string SetPortName(string defaultPortName)
    {
        string portName;

        Console.WriteLine("Available Ports:");
        foreach (string s in SerialPort.GetPortNames())
        {
            Console.WriteLine("   {0}", s);
        }

        Console.Write("Enter COM port value (Default: {0}): ", defaultPortName);
        portName = Console.ReadLine();

        if (portName == "" || !(portName.ToLower()).StartsWith("com"))
        {
            portName = defaultPortName;
        }
        return portName;
    }
    // Display BaudRate values and prompt user to enter a value. 
    public static int SetPortBaudRate(int defaultPortBaudRate)
    {
        string baudRate;

        Console.Write("Baud Rate(default:{0}): ", defaultPortBaudRate);
        baudRate = Console.ReadLine();

        if (baudRate == "")
        {
            baudRate = defaultPortBaudRate.ToString();
        }

        return int.Parse(baudRate);
    }

    // Display PortParity values and prompt user to enter a value. 
    public static Parity SetPortParity(Parity defaultPortParity)
    {
        string parity;

        Console.WriteLine("Available Parity options:");
        foreach (string s in Enum.GetNames(typeof(Parity)))
        {
            Console.WriteLine("   {0}", s);
        }

        Console.Write("Enter Parity value (Default: {0}):", defaultPortParity.ToString());
        parity = Console.ReadLine();

        if (parity == "")
        {
            parity = defaultPortParity.ToString();
        }

        return (Parity)Enum.Parse(typeof(Parity), parity, true);
    }
    // Display DataBits values and prompt user to enter a value. 
    public static int SetPortDataBits(int defaultPortDataBits)
    {
        string dataBits;

        Console.Write("Enter DataBits value (Default: {0}): ", defaultPortDataBits);
        dataBits = Console.ReadLine();

        if (dataBits == "")
        {
            dataBits = defaultPortDataBits.ToString();
        }

        return int.Parse(dataBits.ToUpperInvariant());
    }

    // Display StopBits values and prompt user to enter a value. 
    public static StopBits SetPortStopBits(StopBits defaultPortStopBits)
    {
        string stopBits;

        Console.WriteLine("Available StopBits options:");
        foreach (string s in Enum.GetNames(typeof(StopBits)))
        {
            Console.WriteLine("   {0}", s);
        }

        Console.Write("Enter StopBits value (None is not supported and \n" +
         "raises an ArgumentOutOfRangeException. \n (Default: {0}):", defaultPortStopBits.ToString());
        stopBits = Console.ReadLine();

        if (stopBits == "")
        {
            stopBits = defaultPortStopBits.ToString();
        }

        return (StopBits)Enum.Parse(typeof(StopBits), stopBits, true);
    }
    public static Handshake SetPortHandshake(Handshake defaultPortHandshake)
    {
        string handshake;

        Console.WriteLine("Available Handshake options:");
        foreach (string s in Enum.GetNames(typeof(Handshake)))
        {
            Console.WriteLine("   {0}", s);
        }

        Console.Write("End Handshake value (Default: {0}):", defaultPortHandshake.ToString());
        handshake = Console.ReadLine();

        if (handshake == "")
        {
            handshake = defaultPortHandshake.ToString();
        }

        return (Handshake)Enum.Parse(typeof(Handshake), handshake, true);
    }
}

