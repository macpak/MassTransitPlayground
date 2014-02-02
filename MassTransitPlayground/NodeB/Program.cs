using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Contracts;
using MassTransit;

namespace NodeB
{
    class Program
    {
        static void Main(string[] args)
        {
            Bus.Initialize(sbc =>
            {
                sbc.UseMsmq(c =>
                {
                    c.VerifyMsmqConfiguration();
                    c.UseMulticastSubscriptionClient();
                });
                sbc.VerifyMsDtcConfiguration();
                sbc.ReceiveFrom("msmq://localhost/test_queue?tx=true");
            });

            int counter = 1;
            while (true)
            {
                using (var tx = new TransactionScope())
                {
                    Console.WriteLine("Type sth");
                    var sth = Console.ReadLine();
                    Bus.Instance.Publish(new YourMessage() {Text = sth});
                    if (counter++%3 != 0)
                    {
                        tx.Complete();
                    }
                    Console.WriteLine("{0} sent",counter);
                }
            }
        }
    }
}
