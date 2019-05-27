using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Linq;

namespace nettest
{
    public class NetInfo
    {
        public string HostName;
        public string DomainName;
        public List<Tuple<string, List<string>, List<string>, List<string>>> InterfaceInfos;
        public DateTime CurrentDateTime;

        public NetInfo()
        {
            HostName = string.Empty;
            DomainName = string.Empty;
            InterfaceInfos = new List<Tuple<string, List<string>, List<string>, List<string>>>();
        }

        public void Update()
        {
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();

            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                UnicastIPAddressInformationCollection adapterUniCastAddresses = adapterProperties.UnicastAddresses;
                IPAddressCollection adapterDnsServerAdresses = adapterProperties.DnsAddresses;
                GatewayIPAddressInformationCollection adapterGatewayAddresses = adapterProperties.GatewayAddresses;
                if (adapterUniCastAddresses.Count > 0)
                {
                    List<string> ipAddresses = new List<string>();
                    List<string> dnsServerAddresses = new List<string>();
                    List<string> gatewayAddresses = new List<string>();
                    Tuple<string, List<string>, List<string>, List<string>> interfaceInfo = new Tuple<string, List<string>, List<string>, List<string>>(adapter.Name, ipAddresses, gatewayAddresses, dnsServerAddresses);

                    foreach (IPAddressInformation uniCastAddress in adapterUniCastAddresses)
                    {
                        if (uniCastAddress.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            interfaceInfo.Item2.Add(uniCastAddress.Address.ToString());
                            foreach (var gatewayAddress in adapterGatewayAddresses)
                            {
                                if (gatewayAddress.Address.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    interfaceInfo.Item3.Add(gatewayAddress.Address.ToString());
                                }
                            }
                            foreach (var dnsServerAddress in adapterDnsServerAdresses)
                            {
                                if (dnsServerAddress.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    interfaceInfo.Item4.Add(dnsServerAddress.ToString());
                                }
                            }
                            InterfaceInfos.Add(interfaceInfo);
                        }
                    }
                }
            }
            HostName = ipGlobalProperties.HostName;
            DomainName = ipGlobalProperties.DomainName;
            CurrentDateTime = DateTime.Now;
        }
    }
}
