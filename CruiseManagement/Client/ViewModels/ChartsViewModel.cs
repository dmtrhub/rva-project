using System.Collections.ObjectModel;
using Client.Services;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace Client.ViewModels
{
    public class ChartsViewModel : ViewModelBase
    {
        private readonly StateTracker _stateTracker;

        public ObservableCollection<ISeries> VoyageStateSeries { get; set; }
        public ObservableCollection<ISeries> VoyageStatePieSeries { get; set; }

        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }

        public SolidColorPaint LegendPaint { get; set; }

        private int _totalVoyages;

        public int TotalVoyages
        {
            get => _totalVoyages;
            set => SetProperty(ref _totalVoyages, value);
        }

        private string _currentStateDistribution;

        public string CurrentStateDistribution
        {
            get => _currentStateDistribution;
            set => SetProperty(ref _currentStateDistribution, value);
        }

        public ChartsViewModel(StateTracker stateTracker)
        {
            _stateTracker = stateTracker;

            VoyageStateSeries = new ObservableCollection<ISeries>();
            VoyageStatePieSeries = new ObservableCollection<ISeries>();

            SetupChartStyles();
            RefreshCharts();
        }

        private void SetupChartStyles()
        {
            LegendPaint = new SolidColorPaint
            {
                Color = SKColors.Black,
                FontFamily = "Arial"
            };

            XAxes = new[]
            {
                new Axis
                {
                    Name = "Voyage States",
                    NamePaint = LegendPaint,
                    LabelsPaint = new SolidColorPaint(SKColors.DarkSlateGray),
                    LabelsRotation = 0
                }
            };

            YAxes = new[]
            {
                new Axis
                {
                    Name = "Number of Voyages",
                    NamePaint = LegendPaint,
                    LabelsPaint = new SolidColorPaint(SKColors.DarkSlateGray),
                    MinLimit = 0,
                    Labeler = value => value.ToString("N0")
                }
            };
        }

        public void RefreshCharts()
        {
            try
            {
                var stateCounts = _stateTracker.GetDetailedVoyageStateCounts();
                TotalVoyages = stateCounts.Values.Sum();
                CurrentStateDistribution = string.Join(" | ", stateCounts.Select(kvp => $"{kvp.Key}: {kvp.Value}"));

                // Ažurira X osu
                if (XAxes != null && XAxes.Length > 0)
                {
                    XAxes[0].Labels = stateCounts.Keys.ToArray();
                    OnPropertyChanged(nameof(XAxes));
                }

                // Ažurira BAR CHART
                VoyageStateSeries.Clear();
                VoyageStateSeries.Add(new ColumnSeries<int>
                {
                    Values = stateCounts.Values.ToArray(),
                    Name = "Voyages"
                });

                // Ažurira PIE CHART
                VoyageStatePieSeries.Clear();
                foreach (var kvp in stateCounts)
                {
                    VoyageStatePieSeries.Add(new PieSeries<int>
                    {
                        Values = new[] { kvp.Value },
                        Name = kvp.Key,
                        Fill = GetColorForState(kvp.Key)
                    });
                }
            }
            catch (Exception ex)
            {
                // Fallback na prazne grafikone
                VoyageStateSeries.Clear();
                VoyageStatePieSeries.Clear();
            }
        }

        private SolidColorPaint GetColorForState(string state)
        {
            var color = state switch
            {
                "Scheduled" => SKColors.Blue,
                "OnTime" => SKColors.Green,
                "Delayed" => SKColors.Orange,
                "Arrived" => SKColors.Gray,
                "Cancelled" => SKColors.Red,
                _ => SKColors.Purple
            };

            return new SolidColorPaint(color);
        }

        public void OnVoyageStateChanged()
        {
            RefreshCharts();
        }
    }
}