#region License
/*
ENet for C#
Copyright (c) 2011, 2013 James F. Bellinger <jfb@zer7.com>

Permission to use, copy, modify, and/or distribute this software for any
purpose with or without fee is hereby granted, provided that the above
copyright notice and this permission notice appear in all copies.

THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
*/
#endregion

using System;
using System.Threading;

namespace ENetDemo
{
    // The general arrangement is as follows:
    //   Create a Host. For a client, the peer count is 1. If it's a client, you don't
    //    want someone to be able to connect to it, so the address parameter is null.
    //   Connect if you're a client.
    //   Service() checks for new notifications to read (Connect, Disconnect, Receive).
    //    It returns true if there's something to read, and false if there isn't.
    //    An exception is thrown if some error occured.
    //   CheckEvents() pulls a notification from the queue. It returns true if an event
    //    is dequeued, and false if there are no more.
    //    An exception is thrown if some error occured.
    // Send and receive packets. One thing to keep in mind is, after reading a packet
    //  from a Receive notification, you need to Dispose of it. Otherwise you'll get
    //  a memory leak. Also, if on Peer.Send you give it a Packet instead of a byte
    //  array, be sure to Dispose of the packet afterwards. Host.Broadcast does
    //  disposal automatically.
    class Program
    {
        static void Server()
        {
            using (ENet.Host host = new ENet.Host())
            {
                Console.WriteLine("Initializing server...");

                host.InitializeServer(5000, 1);
                ENet.Peer peer = new ENet.Peer();

                while (true)
                {
                    ENet.Event @event;

                    if (host.Service(15000, out @event))
                    {
                        do
                        {
                            switch (@event.Type)
                            {
                                case ENet.EventType.Connect:
                                    peer = @event.Peer;
                                    // If you are using ENet 1.3.4 or newer, the following two methods will work:
                                    //peer.SetPingInterval(1000);
                                    //peer.SetTimeouts(8, 5000, 60000);
                                    Console.WriteLine("Connected to client at IP/port {0}.", peer.GetRemoteAddress());
                                    for (int i = 0; i < 200; i++)
                                    {
                                        ENet.Packet packet = new ENet.Packet();
                                        packet.Initialize(new byte[] { 0, 0 }, 0, 2, ENet.PacketFlags.Reliable);
                                        packet.SetUserData(i);
                                        packet.SetUserData("Test", i * i);
                                        packet.Freed += p =>
                                            {
                                                Console.WriteLine("Initial packet freed (channel {0}, square of channel {1})",
                                                    p.GetUserData(),
                                                    p.GetUserData("Test"));
                                            };
                                        peer.Send((byte)i, packet);
                                    }
                                    break;

                                case ENet.EventType.Receive:
                                    byte[] data = @event.Packet.GetBytes();
                                    ushort value = BitConverter.ToUInt16(data, 0);
                                    if (value % 1000 == 1) { Console.WriteLine("  Server: Ch={0} Recv={1}", @event.ChannelID, value); }
                                    value++; peer.Send(@event.ChannelID, BitConverter.GetBytes(value), ENet.PacketFlags.Reliable);
                                    @event.Packet.Dispose();
                                    break;
                            }
                        }
                        while (host.CheckEvents(out @event));
                    }
                }
            }
        }

        static void Client()
        {
            using (ENet.Host host = new ENet.Host())
            {
                Console.WriteLine("Initializing client...");
                host.Initialize(null, 1);

                ENet.Peer peer = host.Connect("127.0.0.1", 5000, 1234, 200);
                while (true)
                {
                    ENet.Event @event;

                    if (host.Service(15000, out @event))
                    {
                        do
                        {
                            switch (@event.Type)
                            {
                                case ENet.EventType.Connect:
                                    Console.WriteLine("Connected to server at IP/port {0}.", peer.GetRemoteAddress());
                                    break;

                                case ENet.EventType.Receive:
                                    byte[] data = @event.Packet.GetBytes();
                                    ushort value = BitConverter.ToUInt16(data, 0);
                                    if (value % 1000 == 0) { Console.WriteLine("  Client: Ch={0} Recv={1}", @event.ChannelID, value); }
                                    value++; peer.Send(@event.ChannelID, BitConverter.GetBytes(value), ENet.PacketFlags.Reliable);
                                    @event.Packet.Dispose();
                                    break;

                                default:
                                    Console.WriteLine(@event.Type);
                                    break;
                            }
                        }
                        while (host.CheckEvents(out @event));
                    }
                }
            }
        }

        static void PacketManipulationDemo()
        {
            Console.WriteLine("Packet manipulation test/demo... should print 3 2 1...");
            using (ENet.Packet packet = new ENet.Packet())
            {
                packet.Initialize(new byte[0]);
                packet.Add((byte)1);
                packet.Insert(0, (byte)3);
                packet.Insert(1, (byte)2);
                packet.Insert(packet.IndexOf((byte)3), 4);
                packet.Remove(1);
                packet.RemoveAt(0);
                if (packet.Contains(3)) { packet.Add((byte)1); }
                if (packet.Contains(4)) { packet.Add((byte)5); }

                byte[] bytes = packet.GetBytes();
                for (int i = 0; i < bytes.Length; i++)
                {
                    Console.WriteLine(bytes[i]);
                }
            }            
        }

        static void Main(string[] args)
        {
            Console.WriteLine("ENet demo");
            ENet.Library.Initialize(); // Since 1.3.6.1, Initialize() is no longer required.

            Thread server = new Thread(Server); server.Start();
            Thread.Sleep(250);
            Thread client = new Thread(Client); client.Start();

            PacketManipulationDemo();

            server.Join();
            client.Join();

            ENet.Library.Deinitialize(); // since 1.3.6.1, Deinitialize() does nothing.
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }
    }
}
