using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;

namespace nettest.Pages
{
    public class UrlCheckModel : PageModel
    {
        public List<string> PingResults;
        public List<string> HttpResults;

        public void OnGet(string url)
        {
            PingResults = new List<string>();
            Uri checkUri;
            try
            {
                checkUri = new Uri(url);
            }
            catch (UriFormatException e)
            {
                PingResults.Add($"URL {url} has wrong format");
                PingResults.Add(e.Message);
                return;
            }

            IPAddress clientIpAddress = HttpContext.Connection.RemoteIpAddress;
            PingResults.Add($"Request was originated from client with address: {clientIpAddress.ToString()}");

            List<IPAddress> ipAddressesToPing = new List<IPAddress>();
            if (checkUri.HostNameType == UriHostNameType.Dns)
            {
                ipAddressesToPing = Dns.GetHostAddresses(checkUri.DnsSafeHost).Where(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToList();
            }
            else
            {
                if (checkUri.HostNameType == UriHostNameType.IPv4)
                {
                    IPAddress ipAddress;
                    if (IPAddress.TryParse(checkUri.Host, out ipAddress))
                    {
                        ipAddressesToPing.Add(ipAddress);
                    }
                }
            }

            if (ipAddressesToPing.Count > 0)
            {
                foreach (var ipAddressToPing in ipAddressesToPing)
                {
                    // ping the host
                    Ping pingSender = new Ping();
                    PingOptions options = new PingOptions();

                    // Use the default Ttl value which is 128, but change the fragmentation behavior.
                    options.DontFragment = true;

                    // Create a buffer of 32 bytes of data to be transmitted.
                    string data = "test";
                    byte[] buffer = Encoding.ASCII.GetBytes(data);
                    int timeout = 30;
                    PingReply reply = pingSender.Send(ipAddressToPing, timeout, buffer, options);
                    PingResults.Add($"URL to check resolves to IP:  {reply.Address}");
                    if (reply.Status == IPStatus.Success)
                    {
                        PingResults.Add($"Try to ping succeeded, Roundtrip: {reply.RoundtripTime} ms");
                    }
                    else
                    {
                        PingResults.Add($"Try to ping failed!");
                    }
                }
            }

            // check http connect
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage httpResponse = httpClient.GetAsync(url).Result;
            HttpResults = new List<string>();
            string httpResult = httpResponse.Content.ReadAsStringAsync().Result;
            if (!string.IsNullOrEmpty(httpResult))
            {
                if (httpResponse.Content.Headers.ContentType.MediaType.Equals("application/json", StringComparison.InvariantCultureIgnoreCase))
                {
                    ResponderResult result = JsonConvert.DeserializeObject<ResponderResult>(httpResult);
                    HttpResults.Add($"Json response:");
                    HttpResults.Add($"Time: {result.NetInfo.CurrentDateTime}");
                    HttpResults.Add($"Hostname: {result.NetInfo.HostName}");
                    HttpResults.Add($"Domain: {result.NetInfo.DomainName}");
                    HttpResults.Add($"Interfaces (IPv4 info only):");
                    foreach (var interfaceInfo in result.NetInfo.InterfaceInfos)
                    {
                        string ipAddresses = string.Empty;
                        foreach (var ipAddress in interfaceInfo.Item2)
                        {
                            ipAddresses = ipAddresses + (string.IsNullOrEmpty(ipAddresses) ? "" : ", ") + ipAddress;
                        }
                        if (string.IsNullOrEmpty(ipAddresses))
                        {
                            ipAddresses = "-";
                        }
                        string gatewayAddresses = string.Empty;
                        foreach (var gatewayAddress in interfaceInfo.Item3)
                        {
                            gatewayAddresses = gatewayAddresses + (string.IsNullOrEmpty(gatewayAddresses) ? "" : ", ") + gatewayAddress;
                        }
                        if (string.IsNullOrEmpty(gatewayAddresses))
                        {
                            gatewayAddresses = "-";
                        }
                        string dnsServerAddresses = string.Empty;
                        foreach (var dnsServerAddress in interfaceInfo.Item4)
                        {
                            dnsServerAddresses = dnsServerAddresses + (string.IsNullOrEmpty(dnsServerAddresses) ? "" : ", ") + dnsServerAddress;
                        }
                        if (string.IsNullOrEmpty(dnsServerAddresses))
                        {
                            dnsServerAddresses = "-";
                        }
                        HttpResults.Add($"{interfaceInfo.Item1} (IP: {ipAddresses}, GW: {gatewayAddresses}, DNS: {dnsServerAddresses}");
                    }
                    if (result.HttpInfo.HttpHeaders.Count > 0)
                    {
                        HttpResults.Add($"Http headers (only some):");
                        foreach (var header in result.HttpInfo.HttpHeaders)
                        {
                            HttpResults.Add($"{header.Key}: {header.Value}");
                        }
                    }
                }
                else
                {
                    HttpResults.Add($"Http response: {httpResult}");
                }
            }
            if (HttpResults.Count == 0)
            {
                HttpResults.Add($"No response from {url}");
            }
        }
    }
}
