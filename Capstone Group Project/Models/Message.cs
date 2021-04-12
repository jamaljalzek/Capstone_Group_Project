using System;

namespace Capstone_Group_Project.Models
{
    public class Message
    {
        public String MessageSenderUsername { get; set; } = null;
        public String TimeAndDateMessageWasSent { get; set; } = null;
        public String MessageBody { get; set; } = null;


        public Message()
        {

        }
    }
}
