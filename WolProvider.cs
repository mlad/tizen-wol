using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace WakeOnLan
{
    class WolProvider
    {
        async public static Task<bool> WakeUp(string macAddress)
        {
            macAddress = AddPage.MacSeparators.Replace(macAddress, "");

            var packet = new byte[6 + 6 * 16]; // 6 x FF, 16 x MAC
            var mac = new byte[6];

            int counter;
            for (counter = 0; counter < 6; counter++)
            {
                mac[counter] = byte.Parse(macAddress.Substring(counter * 2, 2), NumberStyles.HexNumber);
            }

            for (counter = 0; counter < 6; counter++)
            {
                packet[counter] = 0xFF;
            }

            for (var i = 0; i < 16; i++)
            {
                for (var j = 0; j < 6; j++)
                {
                    packet[counter] = mac[j];
                    counter++;
                }
            }

            try
            {
                using (var client = new UdpClient())
                {
                    client.Connect(new IPAddress(0xffffffff), 0x2fff);
                    client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 0);
                    await client.SendAsync(packet, packet.Length);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
