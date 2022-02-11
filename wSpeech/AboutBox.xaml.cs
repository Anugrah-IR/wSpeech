using System.Windows;

namespace wSpeech
{
    public partial class AboutBox : Window
    {
        public AboutBox()
        {
            InitializeComponent();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://wspeech.sourceforge.io");
        }
    }
}
