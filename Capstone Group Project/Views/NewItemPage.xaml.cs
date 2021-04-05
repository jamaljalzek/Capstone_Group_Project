using Capstone_Group_Project.Models;
using Capstone_Group_Project.ViewModels;
using Xamarin.Forms;

namespace Capstone_Group_Project.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}