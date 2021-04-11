using Capstone_Group_Project.Models;
using Capstone_Group_Project.ProgramBehavior.UserAccountSystem;
using Capstone_Group_Project.Services;
using System;
using System.Threading.Tasks;

namespace Capstone_Group_Project.ProgramBehavior.ConversationSystem.ConversationCreationSystem
{
    public static class CreateNewConversationHandler
    {
        public static int NewlyCreatedConversationID;

        public static async Task<String> AttemptToCreateNewConversationWithUserAsSoleParticipant()
        {
            // For now, while the cloud is not functional, we immediately return:
            return "New conversation created!\nConversation ID: " + new Random().Next(0, Int32.MaxValue);

            String newConversationPrivateKey = SymmetricEncryption.GenerateNewRandomAesKeyAndReturnAsBase64String();
            String encryptedConversationPrivateKey = AsymmetricEncryption.EncryptPlaintextStringToCiphertextBase64String(newConversationPrivateKey, CurrentLoginState.GetCurrentUserPublicKey());
            CreateNewConversationRequestObject createNewConversationRequestObject = new CreateNewConversationRequestObject(CurrentLoginState.GetCurrentUserAccountID(), encryptedConversationPrivateKey);
            // We expect the cloud to return the exact same CreateNewConversationRequestObject that we originally sent:
            createNewConversationRequestObject = await MobileApplicationHttpClient.PostObjectAsynchronouslyAndReturnResultAsSpecificedType<CreateNewConversationRequestObject>(createNewConversationRequestObject);
            if (createNewConversationRequestObject.ResultOfRequest.Equals("CONVERSATION_CREATION_SUCCESSFUL"))
                return "New conversation created!\nConversation ID: " + createNewConversationRequestObject.Conversation_ID;
            return "ERROR, the new conversation was not successfully created!";
        }
    }
}
