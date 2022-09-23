using MassTransit;
using Rabbit.Models;
using System.Diagnostics;

namespace CatalogService.Consumers
{
    public class EmailReportConsumer : IConsumer<StudentReport>
    {
        public async Task Consume(ConsumeContext<StudentReport> context)
        {
            var data = context.Message;
            //Validate the Ticket Data
            //Store to Database
            //Notify the user via Email / SMS
        }
    }

    public class FaxReportConsumer : IConsumer<StudentReport>
    {
        public async Task Consume(ConsumeContext<StudentReport> context)
        {
            var data = context.Message;
            //Validate the Ticket Data
            //Store to Database
            //Notify the user via Email / SMS
        }
    }

}
