using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace UITest.userControl
{
    /// <summary>
    /// MyChart.xaml 的交互逻辑
    /// </summary>
    public partial class MyChart : UserControl, INotifyPropertyChanged
    {
        private bool _mariaSeriesVisibility;
        private bool _charlesSeriesVisibility;
        private bool _johnSeriesVisibility;
        public static Dictionary<string, List<string>> driverData = new Dictionary<string, List<string>>();
        ChartValues<int> crashes = new LiveCharts.ChartValues<int>(); ChartValues<int> total = new ChartValues<int>(); ChartValues<int> tmad = new ChartValues<int>();

        public MyChart()
        {
            InitializeComponent();

            CrashesVisibility = true;
            TotalVisibility = true;
            TMADVisibility = false;
            if (Util.Tool.driverData != null)
            {
                driverData = Util.Tool.driverData;
            }
            if (driverData.Count == 0 )
            {
                driverData.Add("2020-11-20", new List<string> { "30024", "1254700","3960034" });
                driverData.Add("2020-11-23", new List<string> { "36682", "1368270","3980034" });
                driverData.Add("2020-11-25", new List<string> { "39573", "1375160", "3990034" });
                driverData.Add("2020-11-30", new List<string> { "30024", "1322710", "4037633" });
                driverData.Add("2020-12-3", new List<string> { "40294", "1292510", "4057633" });
                driverData.Add("2020-12-6", new List<string> { "43809", "1310800", "4037633" });
                driverData.Add("2020-12-11", new List<string> { "45336", "1293440", "4057333" });
                driverData.Add("2020-12-16", new List<string> { "47277", "1303080", "4087633" });
            }
            string[] dateTimes = new string[driverData.Keys.Count];
            int i = 0;

            foreach (var item in driverData)
            {
                dateTimes.SetValue(item.Key.ToString(), i);
                crashes.Add(Convert.ToInt32(item.Value[0]));
                total.Add(Convert.ToInt32(item.Value[1]));
                tmad.Add(Convert.ToInt32(item.Value[2]));
                i += 1;
            }

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "rltkapou64.dll",
                    Values = crashes,
                    //DataLabels = true,
                    PointGeometrySize = 5,
                    Fill = Brushes.Transparent,
                    Stroke = Brushes.Orange,
                    Visibility = 0
                },
                new LineSeries
                {
                    Title = "TMAD",
                    Values = tmad,
                    //DataLabels = true,
                    PointGeometrySize = 5,
                    Fill = Brushes.Transparent,
                    Stroke = Brushes.Black,
                    ScalesYAt = 1
                },
                new LineSeries
                {
                    Title = "Total",
                    Values = total,
                    //DataLabels = true,
                    PointGeometrySize = 5,
                    Fill = Brushes.Transparent,
                    Stroke = Brushes.Red,
                    ScalesYAt = 1,
                    //Visibility = CrashesVisibility==true?0:Visibility.Hidden,
                },
            };

            Labels = dateTimes;

            
            //YFormatter = value => value.ToString("C");

            //modifying the series collection will animate and update the chart
            /*            SeriesCollection.Add(new LineSeries
                        {
                            Values = new ChartValues<double> { 5, 3, 2, 4 },
                            LineSmoothness = 0 //straight lines, 1 really smooth lines
                        });*/

            //modifying any series values will also animate and update the chart
            //SeriesCollection[1].Values.Add(5d);

            DataContext = this;

            
        }


        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public bool CrashesVisibility
        {
            get { return _mariaSeriesVisibility; }
            set
            {
                _mariaSeriesVisibility = value;
                OnPropertyChanged("CrashesVisibility");
            }
        }

        public bool TotalVisibility
        {
            get { return _charlesSeriesVisibility; }
            set
            {
                _charlesSeriesVisibility = value;
                OnPropertyChanged("TotalVisibility");
            }
        }

        public bool TMADVisibility
        {
            get { return _johnSeriesVisibility; }
            set
            {
                _johnSeriesVisibility = value;
                OnPropertyChanged("TMADVisibility");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

