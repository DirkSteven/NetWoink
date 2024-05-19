using PcapDotNet.Core;
using System.Collections.Generic;
using System.Linq;

public static class PacketDotNetHelper
{
    public static IList<PacketDevice> GetPacketDevices()
    {
        // Retrieve all network devices
        IList<PacketDevice> devices = LivePacketDevice.AllLocalMachine.ToList<PacketDevice>();

        return devices;
    }
}
