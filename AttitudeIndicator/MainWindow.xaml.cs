using System;
using System.Collections.Generic;
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
using System.Windows.Media.Media3D;
using System.ComponentModel;
using HelixToolkit.Wpf;
using AttitudeIndicator.ViewModels;
using System.Windows.Controls.Primitives;

namespace AttitudeIndicator
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Do Init here // Just because I m lazy
            DataContext = new ApplicationViewModel();

            
            
        }

        private void DarkModeButton_Click(object sender, RoutedEventArgs e)
        {
            var toggle = sender as ToggleButton;

            if(toggle.IsChecked == true)
            {
                this.Resources["ThemeBackGround"] = new SolidColorBrush(Colors.Black);
                this.Resources["ThemeForeground"] = new SolidColorBrush(Colors.White);
            }
            else
            {
                this.Resources["ThemeBackGround"] = new SolidColorBrush(Colors.White);
                this.Resources["ThemeForeground"] = new SolidColorBrush(Colors.Black);
            }


        }
    }
}
