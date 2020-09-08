using System.Collections.ObjectModel;

namespace WakeOnLan
{
    public class LanPc
    {
        public static ObservableCollection<LanPc> List = new ObservableCollection<LanPc>();

        public LanPc(string name, string mac_address)
        {
            Name = name;
            MacAddress = mac_address;
        }

        public LanPc()
        {
        }

        public string Name { get; set; }
        public string MacAddress { get; set; }
        public bool Selected { get; set; }
    }
}
