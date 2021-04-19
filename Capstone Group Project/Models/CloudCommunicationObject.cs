using System;

namespace Capstone_Group_Project.Models
{
    public abstract class CloudCommunicationObject
    {
        // In order for an instance of this class to be properly serialized/deserialized to/from JSON, ALL fields must:
        // 1. Be public.
        // 2. Contain BOTH a getter AND a setter.
        public String TaskRequested { get; set; } = null;
        public String ResultOfRequest { get; set; } = null;
    }


    public class LogUserIntoAccountResponseObject : CloudCommunicationObject
    {
        public int Account_ID { get; set; } = 0;
        public String Public_Key { get; set; } = null;
        public String Private_Key { get; set; } = null;
        public int[] IdsOfConversationsUserIsParticipantIn { get; set; } = null;
        public ConversationInvitation[] ConversationInvitations { get; set; } = null;
    }


    public class ConversationInvitation
    {
        public int Conversation_ID { get; set; } = 0;
        public String Account_Username_Of_Sender { get; set; } = null;
    }
}
