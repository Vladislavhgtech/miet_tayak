using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Emark
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Action<string> _textChaged;

        public MainWindow(Action<string> textChanged)
        {
            InitializeComponent();

            _textChaged = textChanged;
            Program.Text = File.ReadAllText("file.txt");
            Closing += (s, e) => File.WriteAllText("file.txt", Program.Text);
            Program.TextChanged += Program_TextChanged;
        }

        private void Program_TextChanged(object sender, TextChangedEventArgs e)
        {
            _textChaged(Program.Text);
        }

        public void OnError(string error)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                Error.Text = error;
            }));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _textChaged(Program.Text);
        }
    }
}
