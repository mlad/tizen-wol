using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WakeOnLan
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class App : Application
    {
        private string ConfigPath;

        public App()
        {
            InitializeComponent();

            try
            {
                LoadConfig();
            }
            catch
            {

            }

            var nav = new NavigationPage(new MainPage());
            NavigationPage.SetHasNavigationBar(nav, false);

            MainPage = nav;
        }

        private void LoadConfig()
        {
            ConfigPath = Path.Combine(Tizen.Applications.Application.Current.DirectoryInfo.Data, "config.xml");
            if (!File.Exists(ConfigPath))
                return;

            using (var sr = new StreamReader(ConfigPath))
            {
                var serializer = new XmlSerializer(typeof(ObservableCollection<LanPc>));
                LanPc.List = (ObservableCollection<LanPc>)serializer.Deserialize(sr);
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            using (var sw = new StreamWriter(ConfigPath))
            {
                var serializer = new XmlSerializer(typeof(ObservableCollection<LanPc>));
                serializer.Serialize(sw, LanPc.List);
            }
        }

        protected override void OnResume()
        {
        }
    }
}

