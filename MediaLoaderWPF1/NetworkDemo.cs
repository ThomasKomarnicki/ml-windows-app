using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace MediaLoaderWPF1 {
    class NetworkDemo {

        public NetworkDemo()
        {
            //Retrive all network interface using GetAllNetworkInterface() method off NetworkInterface class.

            NetworkInterface[] niArr = NetworkInterface.GetAllNetworkInterfaces();



            Console.WriteLine(@"Retriving basic information of network.\n\n");

            //Display all information of NetworkInterface using foreach loop.

            foreach (NetworkInterface tempNetworkInterface in niArr) {

                Console.WriteLine(@"Network Discription  :  " + tempNetworkInterface.Description);

//                Console.WriteLine(@"Network ID  :  " + tempNetworkInterface.Id);
//
//                Console.WriteLine(@"Network Name  :  " + tempNetworkInterface.Name);
//
//                Console.WriteLine(@"Network interface type  :  " + tempNetworkInterface.NetworkInterfaceType.ToString());
//
//                Console.WriteLine(@"Network Operational Status   :   " + tempNetworkInterface.OperationalStatus.ToString());
//
//                Console.WriteLine(@"Network Spped   :   " + tempNetworkInterface.Speed);
//
//                Console.WriteLine(@"Support Multicast   :   " + tempNetworkInterface.SupportsMulticast);
//
//                Console.WriteLine();

            }
        }
    }
}
