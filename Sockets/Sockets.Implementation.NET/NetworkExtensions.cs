using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using Sockets.Plugin.Abstractions;

namespace Sockets.Plugin
{
    /// <summary>
    /// Helper methods required for the conversion of platform-specific network items to the abstracted versions. 
    /// </summary>
    public static class NetworkExtensions
    {
        /// <summary>
        /// Returns a <code>CommsInterface</code> wrapper from a platform native <code>NetworkInterface</code>. 
        /// </summary>
        /// <param name="nativeInterface"></param>
        /// <returns></returns>
        public static CommsInterface ToCommsInterfaceSummary(this NetworkInterface nativeInterface)
        {
            return CommsInterface.FromNativeInterface(nativeInterface);
        }

        /// <summary>
        /// Converts an <code>OperationalStatus</code> value to the abstracted <code>CommsInterfaceStatus</code>. 
        /// </summary>
        /// <param name="nativeStatus"></param>
        /// <returns></returns>
        public static CommsInterfaceStatus ToCommsInterfaceStatus(this OperationalStatus nativeStatus)
        {
            switch (nativeStatus)
            {
                case OperationalStatus.Up:
                    return CommsInterfaceStatus.Connected;
                case OperationalStatus.Down:
                    return CommsInterfaceStatus.Disconnected;
                case OperationalStatus.Unknown:
                    return CommsInterfaceStatus.Unknown;

                /*
                 * Treat remaining enumerations as "Unknown".
                 * It's unlikely that they will be returned on 
                 * a mobile device anyway. 
                 * 
                    case OperationalStatus.Testing:
                        break;
                    case OperationalStatus.Unknown:
                        break;
                    case OperationalStatus.Dormant:
                        break;
                    case OperationalStatus.NotPresent:
                        break;
                    case OperationalStatus.LowerLayerDown:
                        break;
                 */

                default:
                    return CommsInterfaceStatus.Unknown;
            }
            
        }

        /// <summary>
        /// Determines the broadcast address for a given IPAddress
        /// Adapted from http://blogs.msdn.com/b/knom/archive/2008/12/31/ip-address-calculations-with-c-subnetmasks-networks.aspx
        /// </summary>
        /// <param name="address"></param>
        /// <param name="subnetMask"></param>
        /// <returns></returns>
        public static IPAddress GetBroadcastAddress(this IPAddress address, IPAddress subnetMask)
        {
            var addressBytes = address.GetAddressBytes();
            var subnetBytes = subnetMask.GetAddressBytes();

            var broadcastBytes = addressBytes.Zip(subnetBytes, (a, s) => (byte) (a | (s ^ 255))).ToArray();

            return new IPAddress(broadcastBytes);
        }
    }
}