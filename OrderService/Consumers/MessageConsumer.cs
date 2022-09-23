using MassTransit;
using Rabbit.Models;
using System.Diagnostics;

namespace OrderService.Consumers
{
    
    public class MessageConsumer : IConsumer<MessageTest>
    {
        public async Task Consume(ConsumeContext<MessageTest> context)
        {
            var data = context.Message;
            Debug.WriteLine(data.Messaget);
            Console.WriteLine(data.Messaget);
            //Validate the Ticket Data
            //Store to Database
            //Notify the user via Email / SMS
        }
    }
}
