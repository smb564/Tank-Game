using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace ServerConfig
{
    public delegate void MessageReceiveHandler(string msg);
    class Connection
    {
        private static Connection connection;
        public event MessageReceiveHandler MessageReceiveEvent;

        // Data out
        private NetworkStream outStream;
        private TcpClient client;
        private BinaryWriter writer;

        // Data in
        private NetworkStream inStream;
        private TcpListener listner;

        private Thread receiver;

        private const int SERVER_PORT = 6000;
        private const string SERVER_IP = "127.0.0.1";
        private const int CLIENT_PORT = 7000;
        private bool isStarted = false;

        public static Connection getInstance()
        {
            if (connection == null)
            {
                connection = new Connection();
            }
            return connection;
        }

        private Connection()
        {
            // Singleton
        }

        public void startListening()
        {
            if (isStarted == false)
            {
                receiver = new Thread(new ThreadStart(receiveData));
                receiver.Start();
                isStarted = true;
            }
        }

        public void receiveData()
        {
            Socket sconnection = null;

            this.listner = new TcpListener(IPAddress.Parse("127.0.0.1"), CLIENT_PORT);
            this.listner.Start();

            string msg = "";

            Debug.Log("Receving Started");
            while (true)
            {
                try
                {
                    //connection is connected socket
                    sconnection = listner.AcceptSocket();
                    if (sconnection.Connected)
                    {
                        //To read from socket create NetworkStream object associated with socket
                        this.inStream = new NetworkStream(sconnection);

                        SocketAddress sockAdd = sconnection.RemoteEndPoint.Serialize();
                        string s = sconnection.RemoteEndPoint.ToString();
                        List<Byte> inputStr = new List<byte>();
                        int i = 0;
                        int receivedByte;
                        do
                        {
                            receivedByte = this.inStream.ReadByte();
                            inputStr.Add((Byte)receivedByte);

                            i++;
                        } while (inStream.DataAvailable);

                        msg = Encoding.UTF8.GetString(inputStr.ToArray());
                        this.inStream.Close();
                        sconnection.Close();

                        MessageReceiveEvent(msg);
                    }
                }
                catch (Exception ex)
                {

                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        public void stopListening()
        {
            if (isStarted)
            {
                listner.Stop();
                receiver.Abort();
                receiver.Join();
                isStarted = false;
            }
        }

        public void sendData(string msg)
        {
            if (msg.Length > 0)
            {
                //Opening the connection
                this.client = new TcpClient();

                try
                {

                    this.client.Connect(IPAddress.Parse(SERVER_IP), SERVER_PORT);

                    if (this.client.Connected)
                    {
                        //To write to the socket
                        this.outStream = client.GetStream();

                        //Create objects for writing across stream
                        this.writer = new BinaryWriter(outStream);
                        Byte[] tempStr = Encoding.ASCII.GetBytes(msg);

                        //writing to the port                
                        this.writer.Write(tempStr);
                        this.writer.Close();
                        this.outStream.Close();
                    }

                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
                finally
                {
                    this.client.Close();
                }
            }
        }

    }


}
