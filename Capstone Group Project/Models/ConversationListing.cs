using System;

namespace Capstone_Group_Project.Models
{
    class ConversationListing
    {
        public int ConversationId { get; set; }
        public String UnreadMessagesIndicator { get; set; }


        public ConversationListing()
        {
            UnreadMessagesIndicator = "No unread messages";
        }
    }
}
