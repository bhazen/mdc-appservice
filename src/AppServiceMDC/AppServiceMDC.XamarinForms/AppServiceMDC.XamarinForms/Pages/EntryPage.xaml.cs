using AppServiceMDC.XamarinForms.ViewModels;
using Xamarin.Forms;

namespace AppServiceMDC.XamarinForms.Pages
{
    public partial class EntryPage : ContentPage
    {
        public EntryPage()
        {
            InitializeComponent();
            BindingContext = new EntryPageViewModel();
        }
    }
}
