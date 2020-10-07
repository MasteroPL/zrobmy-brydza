using System;
using System.Net.Sockets;
using System.Text;
using System.IO;

namespace EasyHosting.Models
{
    public class ServerSocket {
        private bool _Initialized = false;
        /// <summary>
        /// Określa, czy TcpListener został zainicjalizowany i nasłuchuje połączeń
        /// </summary>
        public bool Initialized { get { return _Initialized; } private set { _Initialized = value; } }

        private TcpListener _TcpListener = null;
        /// <summary>
        /// Zainicjalizowany TcpListener
        /// </summary>
        public TcpListener TcpListener { get { return _TcpListener; } private set { _TcpListener = value; } }

        private uint _BufferSize = 16384; // 16KB
        /// <summary>
        /// Zdefiniowany rozmiar bufora sczytującego z połączeń przychodzących
        /// </summary>
        public uint BufferSize { get { return _BufferSize; } private set { _BufferSize = value; } }

        private System.Net.IPAddress _IpAddress = System.Net.IPAddress.Any;
        public System.Net.IPAddress IpAddress { get { return _IpAddress; } private set { _IpAddress = value; } }

        private int _Port = 33564;
        public int Port { get { return _Port; } private set { _Port = value; } }

		private void Listen() {
            TcpListener = new TcpListener(IpAddress, Port);
            TcpListener.Start();

            while (true) {
                Console.WriteLine("Awating connections");

                TcpClient client = TcpListener.AcceptTcpClient();

                NetworkStream stream = client.GetStream();
                StreamReader sr = new StreamReader(client.GetStream());
                StreamWriter sw = new StreamWriter(client.GetStream());

                try {
                    byte[] buffer = new byte[BufferSize];
                    stream.Read(buffer, 0, buffer.Length);
                    int recv = 0;

                    foreach (byte b in buffer) {
                        if (b != 0) {
                            recv++;
                        }
                    }

                    string request = Encoding.UTF8.GetString(buffer, 0, recv);

                    Console.WriteLine("request received");
                    
                    sw.WriteLine("You rock!");
                    sw.Flush();
                } catch (Exception e) {
                    Console.WriteLine("Something went wrong.");
                    sw.WriteLine(e.ToString());
                }
            }
        }

        public ServerSocket(System.Net.IPAddress ipAddress = null, int port = 33564, uint bufferSize = 16384) {
            if (ipAddress != null) {
                IpAddress = ipAddress;
            }
            Port = port;
            BufferSize = bufferSize;
        }

        public void Start() {
            Listen();
        }

        public void StartInThread() {
            throw new NotImplementedException();
        }
	}
}
