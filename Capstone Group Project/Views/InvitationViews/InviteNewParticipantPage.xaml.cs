using Capstone_Group_Project.ViewModels.InvitationViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Capstone_Group_Project.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InviteNewParticipantPage : ContentPage
    {
        public InviteNewParticipantPage()
        {
            InitializeComponent();
            this.BindingContext = new InviteNewParticipantViewModel();
        }
    }
}