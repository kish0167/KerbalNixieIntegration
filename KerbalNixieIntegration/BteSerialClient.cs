using System;
using System.IO.Ports;
using System.Threading.Tasks;

namespace KerbalNixieIntegration;

public class BteSerialClient
{
    private SerialPort _serialPort;
    private bool _connected = false;
    
    public async Task Connect()
    {
        await FindAndConnectPort();
    }

    public void SendString(string msg)
    {
        if (!msg.EndsWith("\n"))
        {
            msg += "\n";
        }

        if (!msg.EndsWith("end.\n"))
        {
            msg = "9999999900end.\n";
        }
        
        if (!_connected)
        {
            return;
        }
        
        try
        {
            _serialPort?.WriteLine(msg);
        }
        catch
        {
            _connected = false;
            _serialPort?.Close();
            Task.Run(FindAndConnectPort);
        }
    }

    private void CreateSerialPort(int number)
    {
        _serialPort = new SerialPort
        {
            PortName = "COM" + number,
            BaudRate = 9600,
            Parity = Parity.None,
            DataBits = 8,
            StopBits = StopBits.One,
            Handshake = Handshake.None,
            ReadTimeout = 500,
            WriteTimeout = 500
        };
    }

    private async Task FindAndConnectPort()
    {
        for (int i = 0; i < 32; i++)
        {
            CreateSerialPort(i);
            
            if (await TryOpenSerialPort())
            {
                Console.WriteLine("successfully connected to COM" + i);
                _connected = true;
                return;
            }
            
            Console.WriteLine("COM" + i + " returned error");
            await Task.Delay(500);
        }

        await FindAndConnectPort();
    }

    private async Task<bool> TryOpenSerialPort()
    {
        try
        {
            _serialPort?.Open();
            await Task.Delay(500);
            SendTestMessage();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private void SendTestMessage()
    {
        _serialPort?.WriteLine("0123456700end.\n");
    }
}