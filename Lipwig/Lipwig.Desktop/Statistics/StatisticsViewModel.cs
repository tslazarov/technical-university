using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lipwig.Desktop.Models;
using System.Globalization;
using System.Threading;

namespace Lipwig.Desktop.Statistics
{
    public class StatisticsViewModel : BindableBase
    {
        private CultureInfo calendarDateCulture;
        private StatisticsDateType dateType;
        private string dateSelectionMode;
        private bool isPeriod;
        private bool isBase;

        public StatisticsViewModel()
        {
            this.Data = this.GetData();
            this.Data2 = this.GetData2();

            this.DateType = StatisticsDateType.Period;
        }

        public CultureInfo CalendarDateCulture
        {
            get
            {
                return this.calendarDateCulture;
            }
            set
            {
                SetProperty(ref this.calendarDateCulture, value);
            }
        }

        public string DateSelectionMode
        {
            get
            {
                return this.dateSelectionMode;
            }
            set
            {
                SetProperty(ref this.dateSelectionMode, value);
            }
        }

        public StatisticsDateType DateType
        {
            get
            {
                return this.dateType;
            }
            set
            {
                SetProperty(ref this.dateType, value);
                this.UpdateCalendar();
            }
        }


        public bool IsPeriod
        {
            get
            {
                return this.isPeriod;
            }
            set
            {
                SetProperty(ref this.isPeriod, value);
            }
        }

        public bool IsBase
        {
            get
            {
                return this.isBase;
            }
            set
            {
                SetProperty(ref this.isBase, value);
            }
        }

        public ObservableCollection<CategoryData> Data { get; set; }

        public ObservableCollection<StatisticsData> Data2 { get; set; }

        private void UpdateCalendar()
        {
            var culture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            this.CalendarDateCulture = culture;

            if (this.DateType == StatisticsDateType.Period)
            {
                this.IsPeriod = true;
                this.IsBase = false;
            }
            else if(this.DateType == StatisticsDateType.Monthly)
            {
                this.IsPeriod = false;
                this.IsBase = true;

                culture.DateTimeFormat.ShortDatePattern = "MM yyyy";
                this.CalendarDateCulture.DateTimeFormat.ShortDatePattern = culture.DateTimeFormat.ShortDatePattern;
                this.DateSelectionMode = "Month";
            }
            else
            {
                this.IsPeriod = false;
                this.IsBase = true;

                culture.DateTimeFormat.ShortDatePattern = "yyyy";
                this.CalendarDateCulture.DateTimeFormat.ShortDatePattern = culture.DateTimeFormat.ShortDatePattern;
                this.DateSelectionMode = "Year";
            }
        }

        private ObservableCollection<CategoryData> GetData()
        {
            var data = new ObservableCollection<CategoryData>
        {
            new CategoryData { Value = 0.63M, Category="Sport" },
            new CategoryData { Value = 0.85M, Category="Weather" },
            new CategoryData { Value = 0.75M, Category="News" },
            new CategoryData { Value = 0.96M, Category="Another" },
            new CategoryData { Value = 0.78M, Category="Another2" },
        };

            return data;
        }

        private ObservableCollection<StatisticsData> GetData2()
        {
            var data = new ObservableCollection<StatisticsData>
        {
            new StatisticsData { Value = 63M, Date = string.Format("{0:dd}", DateTime.Today) },
            new StatisticsData { Value = 20M, Date = string.Format("{0:dd}", DateTime.Today.AddDays(1)) },
            new StatisticsData { Value = 75M, Date = string.Format("{0:dd}", DateTime.Today.AddDays(2)) },
            new StatisticsData { Value = 96M, Date = string.Format("{0:dd}", DateTime.Today.AddDays(3)) },
            new StatisticsData { Value = 96M, Date = string.Format("{0:dd}", DateTime.Today.AddDays(4)) },
            new StatisticsData { Value = 96M, Date = string.Format("{0:dd}", DateTime.Today.AddDays(5)) },
            new StatisticsData { Value = 96M, Date = string.Format("{0:dd}", DateTime.Today.AddDays(6)) },
            new StatisticsData { Value = 96M, Date = string.Format("{0:dd}", DateTime.Today.AddDays(7)) },
            new StatisticsData { Value = 96M, Date = string.Format("{0:dd}", DateTime.Today.AddDays(8)) },
            new StatisticsData { Value = 96M, Date = string.Format("{0:dd}", DateTime.Today.AddDays(9)) },
            new StatisticsData { Value = 96M, Date = string.Format("{0:dd}", DateTime.Today.AddDays(10)) },
            new StatisticsData { Value = 96M, Date = string.Format("{0:dd}", DateTime.Today.AddDays(11)) },
        };

            return data;
        }
    }
}
