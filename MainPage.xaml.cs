using System;
using System.Linq;
using Tizen.Wearable.CircularUI.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WakeOnLan
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : CirclePage
    {

        public MainPage()
        {
            InitializeComponent();
            PcList.ItemsSource = LanPc.List;

            RefreshPlaceholder();
        }

        async private void ListItemTapped(object sender, ItemTappedEventArgs e)
        {
            var result = await WolProvider.WakeUp(((LanPc)e.Item).MacAddress);

            Toast.DisplayText(result ?
                Resx.AppResources.SendSuccess
                : Resx.AppResources.SendError);
        }

        async private void ToolbarAddClicked(object sender, EventArgs e)
        {
            var page = new AddPage();
            page.ItemAdded += RefreshPlaceholder;

            var nav = new NavigationPage(page);
            NavigationPage.SetHasNavigationBar(nav, false);

            await Navigation.PushModalAsync(nav);
        }

        private void ToolbarDeleteClicked(object sender, EventArgs e)
        {
            var selectedPcs = LanPc.List.Where(x => x.Selected).ToList();

            var leftButton = new MenuItem() { Icon = new FileImageSource { File = "back.png" } };
            var rightButton = new MenuItem() { Icon = new FileImageSource { File = "delete.png" } };

            var confirmPopup = new TwoButtonPopup
            {
                Title = Resx.AppResources.Delete,
                Text = string.Format(Resx.AppResources.DeleteConfirm, selectedPcs.Count),
                FirstButton = leftButton,
                SecondButton = rightButton,
            };

            leftButton.Command = new Command(() => confirmPopup.Dismiss());
            rightButton.Command = new Command(() =>
            {
                PcList.BeginRefresh();
                foreach (var i in selectedPcs)
                    LanPc.List.Remove(i);
                PcList.EndRefresh();

                RefreshPlaceholder();
                confirmPopup.Dismiss();
            });

            confirmPopup.Show();
        }

        async private void ToolbarPowerClicked(object sender, EventArgs e)
        {
            foreach (var i in LanPc.List.Where(x => x.Selected))
            {
                await WolProvider.WakeUp(i.MacAddress);
                i.Selected = false;
            }
        }

        private void RefreshPlaceholder()
        {
            if (LanPc.List.Count == 0)
            {
                EmptyPlaceholder.IsVisible = true;
                PcList.IsVisible = false;
            }
            else
            {
                EmptyPlaceholder.IsVisible = false;
                PcList.IsVisible = true;
            }
        }
    }
}