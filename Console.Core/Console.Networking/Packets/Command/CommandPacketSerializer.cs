using System.Collections.Generic;
using System.Linq;
using Console.Networking.Packets.Abstract;

namespace Console.Networking.Packets.Command
{
    public class CommandPacketSerializer : PacketSerializer<CommandPacket>
    {
        protected override CommandPacket Deserialize(byte[] data)
        {
            return new CommandPacket(data[0] == 1, NetworkingSettings.EncodingInstance.GetString(data, 1, data.Length - 1));
        }

        protected override byte[] Serialize(CommandPacket item)
        {
            List<byte> data = NetworkingSettings.EncodingInstance.GetBytes(item.Input).ToList();
            data.Insert(0, (byte)(item.Resolved ? 1 : 0));
            return data.ToArray();
        }
    }
}