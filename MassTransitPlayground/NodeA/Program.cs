using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using MassTransit;

namespace NodeA
{
    internal class Program
    {
        private static void Main(string[] args)
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
                    sbc.Subscribe(subs => subs.Handler<YourMessage>(msg => Console.WriteLine(msg.Text)));
                });


            Console.ReadKey();
        }
    }
}
