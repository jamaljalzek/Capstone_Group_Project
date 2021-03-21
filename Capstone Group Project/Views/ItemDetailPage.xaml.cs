using Capstone_Group_Project.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Capstone_Group_Project.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}