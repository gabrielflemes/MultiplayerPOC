using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

public class Client : MonoBehaviour
{
    public static Client instance;
    public static int dataBufferSize = 4096; //4mb

    public string ip = "127.0.0.1"; //localhost
    public int port = 26950; //the same server

    public int myId = 0;
    public TCP tcp;
    public UDP udp;

    private bool isConnected = false;

    private delegate void PacketHandler(Packet _packet);
    private static Dictionary<int, PacketHandler> packetHandlers;


    //singleton, so that we have only one instance and not multiple connection on server
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object.");
            Destroy(this);
        }
    }

    private void OnApplicationQuit()
    {
        Disconnect(); // Disconnect when the game is closed
    }

    /// <summary>Attempts to connect to the server.</summary>
    public void ConnectToServer()
    {

        tcp = new TCP();
        udp = new UDP();

        InitializeClientData();

        isConnected = true;

        tcp.Connect(); // Connect tcp, udp gets connected once tcp is done
    }

    //TCP Protocol
    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private Packet receivedData; //received data from the server
        private byte[] receiveBuffer;

        /// <summary>Attempts to connect to the server via TCP.</summary>
        public void Connect()
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            receiveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(instance.ip, instance.port, ConnectionCallback, socket);
        }

        /// <summary>Initializes the newly connected client's TCP-related info.</summary>
        private void ConnectionCallback(IAsyncResult _result)
        {
            socket.EndConnect(_result);

            if (!socket.Connected)
            {
                return;
            }

            stream = socket.GetStream();

            receivedData = new Packet();

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }


        /// <summary>Sends data to the client via TCP.</summary>
        /// <param name="_packet">The packet to send.</param>
        public void SendData(Packet _packet)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);  // Send data to server
                }
            }
            catch (Exception _err)
            {

                Debug.Log($"Error sending data to serve via TCP: {_err}");
            }
        }


        /// <summary>Reads incoming data from the stream.</summary>
        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int byteLenght = stream.EndRead(_result);

                if (byteLenght <= 0)
                {
                    //disconnect
                    instance.Disconnect();
                    return;
                }

                byte[] data = new byte[byteLenght];
                Array.Copy(receiveBuffer, data, byteLenght);

                //handle data
                receivedData.Reset(HandlerData(data));  // Reset receivedData if all data was handled
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch
            {
                Disconnect();
            }
        }


        /// <summary>Prepares received data to be used by the appropriate packet handler methods.</summary>
        /// <param name="_data">The recieved data.</param>
        private bool HandlerData(byte[] _data)
        {
            int packetLenght = 0;

            receivedData.SetBytes(_data);

            if (receivedData.UnreadLength() >= 4)
            {
                // If client's received data contains a packet
                packetLenght = receivedData.ReadInt();
                if (packetLenght <= 0)
                {
                    // If packet contains no data
                    return true; // Reset receivedData instance to allow it to be reused
                }
            }

            while (packetLenght > 0 && packetLenght <= receivedData.UnreadLength())
            {
                // While packet contains data AND packet data length doesn't exceed the length of the packet we're reading
                byte[] packetBytes = receivedData.ReadBytes(packetLenght);

                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(packetBytes))
                    {
                        int packetId = _packet.ReadInt();

                        //execute a method on dictionary
                        packetHandlers[packetId](_packet); // Call appropriate method to handle the packet
                    }
                });

                packetLenght = 0; // Reset packet length
                if (receivedData.UnreadLength() >= 4)
                {
                    // If client's received data contains another packet
                    packetLenght = receivedData.ReadInt();
                    if (packetLenght <= 0)
                    {
                        // If packet contains no data
                        return true; // Reset receivedData instance to allow it to be reused
                    }
                }
            }

            if (packetLenght <= 1)
            {
                return true; // Reset receivedData instance to allow it to be reused
            }

            return false;
        }


        /// <summary>Disconnects from the server and cleans up the TCP connection.</summary>
        private void Disconnect()
        {
            instance.Disconnect();

            stream = null;
            receiveBuffer = null;
            receivedData = null;
            socket = null;
        }


    }


    //UDP Protocol
    public class UDP
    {
        public UdpClient socket; //protocol
        public IPEndPoint endPoint; //endpoint

        public UDP()
        {
            endPoint = new IPEndPoint(IPAddress.Parse(instance.ip), instance.port);
        }


        /// <summary>Attempts to connect to the server via UDP.</summary>
        /// <param name="_localPort">The port number to bind the UDP socket to.</param>
        public void Connect(int localPort)
        {
            socket = new UdpClient(localPort); //instantiate a udp client
            socket.Connect(endPoint); //connect to endpoint
            socket.BeginReceive(ReceiveCallback, null); //Receives a datagram from a remote host asynchronously.

            using (Packet packet = new Packet())
            {
                SendData(packet);
            }
        }


        /// <summary>Sends data to the client via UDP.</summary>
        /// <param name="_packet">The packet to send.</param>
        public void SendData(Packet packet)
        {
            try
            {
                packet.InsertInt(instance.myId); // Insert the client's ID at the start of the packet
                if (socket != null)
                {
                    socket.BeginSend(packet.ToArray(), packet.Length(), null, null);
                }
            }
            catch (Exception ex)
            {

                Debug.Log($"Error sending data to server via UDP: {ex}");
            }
        }

        /// <summary>Receives incoming UDP data.</summary>
        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                byte[] data = socket.EndReceive(_result, ref endPoint); //Ends a pending asynchronous receive.
                socket.BeginReceive(ReceiveCallback, null); //Receives a datagram from a remote host asynchronously.

                //make sure we have data to handle
                if (data.Length < 4)
                {
                    //disconnect
                    instance.Disconnect();
                    return;
                }

                HandleData(data);
            }
            catch
            {
                // disconnect
                Disconnect();
            }
        }

        /// <summary>Prepares received data to be used by the appropriate packet handler methods.</summary>
        /// <param name="_data">The recieved data.</param>
        private void HandleData(byte[] _data)
        {
            using (Packet packet = new Packet(_data))
            {
                int packetLength = packet.ReadInt();
                _data = packet.ReadBytes(packetLength);
            }

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet packet = new Packet(_data))
                {
                    int packetId = packet.ReadInt();
                    packetHandlers[packetId](packet); // Call appropriate method to handle the packet
                }
            });
        }

        /// <summary>Disconnects from the server and cleans up the UDP connection.</summary>
        private void Disconnect()
        {
            instance.Disconnect();

            endPoint = null;
            socket = null;
        }

    }


    /// <summary>Initializes all necessary client data.</summary>
    private void InitializeClientData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
                { (int)ServerPackets.welcome, ClientHandle.Welcome },
                { (int)ServerPackets.spawnPlayer, ClientHandle.SpawnPlayer },
                { (int)ServerPackets.playerPosition, ClientHandle.PlayerPosition },
                { (int)ServerPackets.playerRotation, ClientHandle.PlayerRotation },
                { (int)ServerPackets.moveTo, ClientHandle.MoveTo }
        };

        Debug.Log("Initialized packetes.");

    }

    /// <summary>Disconnects from the server and stops all network traffic.</summary>
    private void Disconnect()
    {
        if (isConnected)
        {
            isConnected = false;
            tcp.socket.Close();
            udp.socket.Close();

            Debug.Log("Disconnected from server.");
        }
    }
}
