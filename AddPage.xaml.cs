using System;
using System.Linq;
using System.Text.RegularExpressions;
using Tizen.Wearable.CircularUI.Forms;
using Xamarin.Forms.Xaml;

namespace WakeOnLan
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPage : CirclePage
    {
        private static Regex _macRegex = new Regex("^[0-9A-Fa-f]{12}$", RegexOptions.Compiled);
        public static Regex MacSeparators = new Regex("[-: ]", RegexOptions.Compiled);

        public event Action ItemAdded;

        /*public static string GetRandomMacAddress()
        {
            var random = new Random();
            var buffer = new byte[6];
            random.NextBytes(buffer);
            var result = String.Concat(buffer.Select(x => string.Format("{0}:", x.ToString("X2"))).ToArray());
            return result.TrimEnd(':');
        }*/

        public AddPage()
        {
            InitializeComponent();

            // MacField.Text = GetRandomMacAddress();
        }

        async private void AddClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameField.Text) || string.IsNullOrWhiteSpace(MacField.Text))
            {
                Toast.DisplayText(Resx.AppResources.AllFieldsRequired);
                return;
            }

            var mac = MacSeparators.Replace(MacField.Text, "");
            if (!_macRegex.IsMatch(mac))
            {
                Toast.DisplayText(Resx.AppResources.MacFormat);
                return;
            }

            LanPc.List.Add(new LanPc(NameField.Text, MacField.Text));

            ItemAdded?.Invoke();

            await Navigation.PopModalAsync();
        }
    }
}