using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace MultiGridPlayer
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Main : Window
    {
        private int xcol = 3;
        private int ycol = 3;

        private List<MediaElement> _me_list = new List<MediaElement>();
        private List<string> _files = new List<string>();

        private bool _fullscreen = false;
        public Main()
        {
            InitializeComponent();
            InitGridElements();
        }

        private void mnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void SetGrid_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null && sender is MenuItem)
                switch ((sender as MenuItem).Name)
                {
                    case "gr33":
                        xcol = 3;
                        ycol = 3;
                        break;
                    case "gr43":
                        xcol = 4;
                        ycol = 3;
                        break;
                    case "gr44":
                        xcol = 4;
                        ycol = 4;
                        break;
                    case "gr54":
                        xcol = 5;
                        ycol = 4;
                        break;
                    default:
                        xcol = ycol = 3;
                        break;
                }
            InitGridElements();
        }

        /// <summary>
        /// Расположить элементы по сетке
        /// </summary>
        private void InitGridElements()
        {
            if (xcol >= 1 && ycol >= 1)
            {

                foreach (var me in _me_list)
                {
                    me.Stop();
                    me.Source = null;
                }
                //if (_me_list.Count <= (xcol * ycol))
                {
                    ugPlayers.Children.Clear();
                    ugPlayers.Columns = xcol;
                    ugPlayers.Rows = ycol;
                    for (int i = _me_list.Count; i < (xcol * ycol); i++)
                    {
                        var me = 
                        new MediaElement()
                        {
                            LoadedBehavior = MediaState.Manual,
                            UnloadedBehavior = MediaState.Manual,
                            Margin = new Thickness(0.0d),
                            Stretch = Stretch.UniformToFill
                        };
                        _me_list.Add(me);
                    }

                    int cnt = 0;
                    for (int x = 0; x < xcol; x++)
                    {
                        for (int y = 0; y < ycol; y++)
                        {
                            var me = _me_list[cnt];
                            ugPlayers.Children.Add(me);
                            cnt++;
                        }
                    }
                }
            }
        }

        private void mnOpenMany_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog() { Multiselect = true };
            bool? ret = ofd.ShowDialog();

            if (ret.GetValueOrDefault(false) && ofd.FileNames.Length > 0)
            {
                _files.Clear();
                _files.AddRange(ofd.FileNames);
                int cnt = 0;
                foreach (var file in _files)
                {
                    if (cnt < _me_list.Count)
                    {
                        _me_list[cnt].Source = new Uri(file);
                        _me_list[cnt].Play();
                        _me_list[cnt].Volume = 1.0d;

                    }
                    cnt++;
                }
            }
        }

        private void mnGotoMiddle_Click(object sender, RoutedEventArgs e)
        {
            foreach (var me in _me_list)
            {
                me.Position = new TimeSpan(me.NaturalDuration.TimeSpan.Ticks / 2);
            }
        }

        private void mnGotoStart_Click(object sender, RoutedEventArgs e)
        {
            foreach (var me in _me_list)
            {
                me.Position = new TimeSpan(0);
            }
        }


        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_fullscreen)
            {
                mnMain.Visibility = Visibility.Collapsed;
            }
            else
            {
                mnMain.Visibility = Visibility.Visible;
            }
            _fullscreen = !_fullscreen;
        }
    }
}
