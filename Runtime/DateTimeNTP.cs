using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
public class DateTimeNTP
{



    /// <summary>
    /// Gets the current DateTime from time-a.nist.gov.
    /// </summary>
    /// <returns>A DateTime containing the current time.</returns>
    public static DateTime GetNetworkTime()
    {
        return GetNetworkTime("time.windows.com"); // time-a.nist.gov
    }

    /// <summary>
    /// Gets the current DateTime from <paramref name="ntpServer"/>.
    /// </summary>
    /// <param name="ntpServer">The hostname of the NTP server.</param>
    /// <returns>A DateTime containing the current time.</returns>
    public static DateTime GetNetworkTime(string ntpServer)
    {
        IPAddress[] address = Dns.GetHostEntry(ntpServer).AddressList;

        if (address == null || address.Length == 0)
            throw new ArgumentException("Could not resolve ip address from '" + ntpServer + "'.", "ntpServer");

        IPEndPoint ep = new IPEndPoint(address[0], 123);

        return GetNetworkTime(ep);
    }

    /// <summary>
    /// Gets the current DateTime form <paramref name="ep"/> IPEndPoint.
    /// </summary>
    /// <param name="ep">The IPEndPoint to connect to.</param>
    /// <returns>A DateTime containing the current time.</returns>
    public static DateTime GetNetworkTime(IPEndPoint ep)
    {
        Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        s.Connect(ep);

        byte[] ntpData = new byte[48]; // RFC 2030 
        ntpData[0] = 0x1B;
        for (int i = 1; i < 48; i++)
            ntpData[i] = 0;

        s.Send(ntpData);
        s.Receive(ntpData);

        byte offsetTransmitTime = 40;
        ulong intpart = 0;
        ulong fractpart = 0;

        for (int i = 0; i <= 3; i++)
            intpart = 256 * intpart + ntpData[offsetTransmitTime + i];

        for (int i = 4; i <= 7; i++)
            fractpart = 256 * fractpart + ntpData[offsetTransmitTime + i];

        ulong milliseconds = (intpart * 1000 + (fractpart * 1000) / 0x100000000L);
        s.Close();

        TimeSpan timeSpan = TimeSpan.FromTicks((long)milliseconds * TimeSpan.TicksPerMillisecond);

        DateTime dateTime = new DateTime(1900, 1, 1);
        dateTime += timeSpan;

        TimeSpan offsetAmount = TimeZone.CurrentTimeZone.GetUtcOffset(dateTime);
        DateTime networkDateTime = (dateTime + offsetAmount);

        return networkDateTime;
    }


    //public static DateTime GetNetworkTime()
    //{
    //    //default Windows time server
    //    const string ntpServer = "time.windows.com";

    //    // NTP message size - 16 bytes of the digest (RFC 2030)
    //    var ntpData = new byte[48];

    //    //Setting the Leap Indicator, Version Number and Mode values
    //    ntpData[0] = 0x1B; //LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)

    //    var addresses = Dns.GetHostEntry(ntpServer).AddressList;

    //    //The UDP port number assigned to NTP is 123
    //    var ipEndPoint = new IPEndPoint(addresses[0], 123);
    //    //NTP uses UDP

    //    using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
    //    {
    //        socket.Connect(ipEndPoint);

    //        //Stops code hang if NTP is blocked
    //        socket.ReceiveTimeout = 3000;

    //        socket.Send(ntpData);
    //        socket.Receive(ntpData);
    //        socket.Close();
    //    }

    //    //Offset to get to the "Transmit Timestamp" field (time at which the reply 
    //    //departed the server for the client, in 64-bit timestamp format."
    //    const byte serverReplyTime = 40;

    //    //Get the seconds part
    //    ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);

    //    //Get the seconds fraction
    //    ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

    //    //Convert From big-endian to little-endian
    //    intPart = SwapEndianness(intPart);
    //    fractPart = SwapEndianness(fractPart);

    //    var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

    //    //**UTC** time
    //    var networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);

    //    return networkDateTime;
    //}

    //// stackoverflow.com/a/3294698/162671
    //static uint SwapEndianness(ulong x)
    //{
    //    return (uint)(((x & 0x000000ff) << 24) +
    //                   ((x & 0x0000ff00) << 8) +
    //                   ((x & 0x00ff0000) >> 8) +
    //                   ((x & 0xff000000) >> 24));
    //}
}
