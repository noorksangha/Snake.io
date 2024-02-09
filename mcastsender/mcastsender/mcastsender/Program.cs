﻿using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

class TestMulticastOptionSender
{
    static IPAddress mcastAddress;
    static int mcastPort;
    static Socket mcastSocket;

    static void Main()
    {
        mcastAddress = IPAddress.Parse("230.0.0.1");
        mcastPort = 11000;

        try
        {
            mcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint endPoint = new IPEndPoint(mcastAddress, mcastPort);

            float x = 1.0f, y=0.0f, z = 1.0f; // Initialize coordinates

            Console.WriteLine("Press 'W', 'A', 'S', 'D' to move the cube. Press 'Q' to quit.");

            bool running = true;
            while (running)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.W:
                            z += 0.1f; // Move forward
                            break;
                        case ConsoleKey.S:
                            z -= 0.1f; // Move backward
                            break;
                        case ConsoleKey.A:
                            x -= 0.1f; // Move left
                            break;
                        case ConsoleKey.D:
                            x += 0.1f; // Move right
                            break;
                        case ConsoleKey.Q:
                            running = false; // Quit program
                            break;
                    }

                    string message = "ID=2;" + x + ","+ y +"," + z;
                    byte[] bytes = Encoding.ASCII.GetBytes(message);
                    mcastSocket.SendTo(bytes, endPoint);

                    Console.WriteLine("Sent: {message}");
                }
                Thread.Sleep(100); // Small delay to reduce CPU usage
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("\n" + e.ToString());
        }
        finally
        {
            mcastSocket.Close();
            Console.WriteLine("Multicast sender closed.");
        }
    }
}
