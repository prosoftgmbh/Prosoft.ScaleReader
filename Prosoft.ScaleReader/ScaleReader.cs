using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;

namespace Prosoft.ScaleReader
{
    public class ScaleReader : System.ComponentModel.INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        bool SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            return true;
        }

        public ScaleReader(string portName)
        {
            Port = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
        }

        public ScaleReader(string portName, int baudRate)
        {
            Port = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
        }

        public ScaleReader(string portName, int baudRate, Parity parity)
        {
            Port = new SerialPort(portName, baudRate, parity, 8, StopBits.One);
        }

        public ScaleReader(string portName, int baudRate, Parity parity, int dataBits)
        {
            Port = new SerialPort(portName, baudRate, parity, dataBits, StopBits.One);
        }

        public ScaleReader(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            Port = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
        }

        public ScaleReader()
        {
            Port = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
        }

        private SerialPort Port;

        public EventHandler<ErrorEventArgs> Error;

        decimal _value;
        public decimal Value
        {
            get { return _value; }
            private set { SetProperty(ref _value, value); }
        }

        public bool IsRunning { get; private set; } = false;

        public void Start()
        {
            try
            {
                Port.Open();
            }
            catch (Exception ex)
            {
                if (Error != null) Error.Invoke(this, new ErrorEventArgs(ex.Message));

                return;
            }

            IsRunning = true;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (!IsRunning) return;

                    string content = null;

                    content = Port.ReadLine();

                    var contents = content.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    bool isNegative = false;

                    if (contents.Any(i => i == "-")) isNegative = true;

                    for (int i = 0; i < contents.Length; i++)
                    {
                        System.Diagnostics.Debug.WriteLine(contents[i]);

                        contents[i] = contents[i].Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

                        if (decimal.TryParse(contents[i], out decimal weight))
                        {
                            if (isNegative) Value = -weight;
                            else Value = weight;
                        }
                    }
                }
            }).ContinueWith((r) =>
            {
                if (IsRunning && Error != null && r.Exception != null) Error.Invoke(this, new ErrorEventArgs(r.Exception.InnerException.Message));
            });
        }

        public void Stop()
        {
            IsRunning = false;
            if (Port != null && Port.IsOpen) Port.Close();
        }

        ~ScaleReader()
        {
            Stop();
        }
    }
}
