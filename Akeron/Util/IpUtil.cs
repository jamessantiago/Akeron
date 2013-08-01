using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Styx.Util
{
    public static class IpUtil
    {
        public static bool IsInIpRange(string Ip, string RangeIp, string RangeMask)
        {
            IPAddress ip = IPAddress.Parse(Ip);
            IPAddress rangeip = IPAddress.Parse(RangeIp);
            IPAddress rangemask = IPAddress.Parse(RangeMask);
            IPAddress broadcast1 = GetBroadcastAddress(rangeip, rangemask);
            IPAddress broadcast2 = GetBroadcastAddress(ip, rangemask);
            return broadcast1.Equals(broadcast2);    
        }

        public static IPAddress GetBroadcastAddress(IPAddress Ip, IPAddress SubnetMask)
        {
            byte[] ipAddressBytes = Ip.GetAddressBytes();
            byte[] subnetMaskBytes = SubnetMask.GetAddressBytes();

            if (ipAddressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of ip address and subnet mask do not match");

            byte[] broadcastAddress = new byte[ipAddressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAddressBytes[i] & subnetMaskBytes[i]);
            }
            return new IPAddress(broadcastAddress);
        }
    }
}
