using Capstone_Group_Project.Services;
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


    public class IsUsernameAlreadyInUseRequestObject : CloudCommunicationObject
    {
        public String Account_Username { get; set; } = null;


        public IsUsernameAlreadyInUseRequestObject(String enteredUsername)
        {
            this.TaskRequested = "CHECK_IF_USERNAME_IS_IN_USE";
            this.Account_Username = enteredUsername;
        }
    }


    public class CreateNewUserAccountRequestObject : CloudCommunicationObject
    {
        public String Account_Username { get; set; } = null;
        public String Account_Password_Hashcode { get; set; } = null;
        public String Public_Key { get; set; } = null;
        public String Private_Key { get; set; } = null;


        public CreateNewUserAccountRequestObject(String enteredUsername, String enteredPassword)
        {
            this.TaskRequested = "CREATE_NEW_ACCOUNT";
            this.Account_Username = enteredUsername;
            this.Account_Password_Hashcode = Hashing.ConvertPasswordStringIntoSha256HashCodeBase64String(enteredPassword);
            String publicAndPrivateKey = AsymmetricEncryption.CreateNewPublicAndPrivateKeyAndReturnAsXmlString();
            this.Private_Key = SymmetricEncryption.EncryptPlaintextStringToCiphertextBase64String(publicAndPrivateKey, enteredPassword);
            this.Public_Key = AsymmetricEncryption.ExtractPublicKeyAndReturnAsXmlString(publicAndPrivateKey);
        }
    }


    public class LogUserIntoAccountRequestObject : CloudCommunicationObject
    {
        public String Account_Username { get; set; } = null;
        public String Account_Password_Hashcode { get; set; } = null;


        public LogUserIntoAccountRequestObject(String enteredUsername, String enteredPassword)
        {
            this.TaskRequested = "LOG_INTO_ACCOUNT";
            this.Account_Username = enteredUsername;
            this.Account_Password_Hashcode = Hashing.ConvertPasswordStringIntoSha256HashCodeBase64String(enteredPassword);
        }
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


    public class CreateNewConversationRequestObject : CloudCommunicationObject
    {
        public int Account_ID { get; set; } = 0;
        public String Conversation_Private_Key { get; set; } = null;
        public int Conversation_ID { get; set; } = 0;


        public CreateNewConversationRequestObject(int accountID, String conversationPrivateKey)
        {
            this.TaskRequested = "CREATE_NEW_CONVERSATION";
            this.Account_ID = accountID;
            this.Conversation_Private_Key = conversationPrivateKey;
        }
    }
}
