using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;


namespace XT {

public static class XtNetwork
{
    public static readonly int XT_PORT = 22612;
    public static readonly string XT_DISCOVERY_REQUEST = "XT";

    public static string GetLocalIPv4()
    {
        foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
                return ip.ToString();
        }
        return "0.0.0.0";
    }
}

} // namespace XT
