namespace Rabbit.Models
{
    public class MessageTest
    {
        public string Messaget { get; set; }

        public MessageTest()
        {
            Messaget = "I love you";
        }
    }

    public class StudentReport
    {
        public string StudentNumber { get; set; }
        public string Provider { get; set; } // "email" or "fax"
        public string Target { get; set; }
    }

    public class Report
    {
        public string StudentNumber { get; set; }
        public string Provider { get; set; } // "email" or "fax"
        public string Target { get; set; }
        public bool IsPublic { get; set; }
    }
}
