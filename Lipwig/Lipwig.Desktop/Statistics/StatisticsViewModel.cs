using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lipwig.Desktop.Models;
using System.Globalization;
using System.Threading;
using Lipwig.Services.Contracts;
using Lipwig.Models;
using Lipwig.Utilities;

namespace Lipwig.Desktop.Statistics
{
    public class StatisticsViewModel : BindableBase
    {
        private ObservableCollection<CartesianData> cartesianData;
        private ObservableCollection<PieChartData> pieChartData;
        private CultureInfo calendarDateCulture;
        private StatisticsDateType dateType;
        private string dateSelectionMode;
        private DateTime startDate;
        private DateTime endDate;
        private DateTime periodDate;
        private bool isPeriod;
        private bool isBase;
        private IUsersService usersService;
        private User user;

        public StatisticsViewModel(IUsersService usersService)
        {
            this.DateType = StatisticsDateType.Period;
            this.FilterCommand = new RelayCommand(Filter);

            this.usersService = usersService;
            this.user = this.usersService.GetUserByEmail(Constants.Email);

            this.PeriodPieChart(this.StartDate, this.EndDate);
            this.DailyCartesianChart(this.StartDate, this.EndDate);
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

        public DateTime StartDate
        {
            get
            {
                return this.startDate;
            }
            set
            {
                SetProperty(ref this.startDate, value);
            }
        }

        public DateTime EndDate
        {
            get
            {
                return this.endDate;
            }
            set
            {
                SetProperty(ref this.endDate, value);
            }
        }

        public DateTime PeriodDate
        {
            get
            {
                return this.periodDate;
            }
            set
            {
                SetProperty(ref this.periodDate, value);
            }
        }

        public RelayCommand FilterCommand { get; private set; }

        public ObservableCollection<PieChartData> PieChartData
        {
            get
            {
                return this.pieChartData;
            }
            set
            {
                SetProperty(ref this.pieChartData, value);
            }
        }
        public ObservableCollection<CartesianData> CartesianData
        {
            get
            {
                return this.cartesianData;
            }
            set
            {
                SetProperty(ref this.cartesianData, value);
            }
        }

        private void UpdateCalendar()
        {
            var culture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            this.CalendarDateCulture = culture;

            if (this.DateType == StatisticsDateType.Period)
            {
                this.IsPeriod = true;
                this.IsBase = false;
                this.StartDate = DateTime.Today.AddDays(-7);
                this.EndDate = DateTime.Today;
            }
            else if(this.DateType == StatisticsDateType.Monthly)
            {
                this.IsPeriod = false;
                this.IsBase = true;

                this.PeriodDate = DateTime.Today;
                culture.DateTimeFormat.ShortDatePattern = "MM/yyyy";
                this.CalendarDateCulture.DateTimeFormat.ShortDatePattern = culture.DateTimeFormat.ShortDatePattern;
                this.DateSelectionMode = "Month";
            }
            else
            {
                this.IsPeriod = false;
                this.IsBase = true;

                this.PeriodDate = DateTime.Today;
                culture.DateTimeFormat.ShortDatePattern = "yyyy";
                this.CalendarDateCulture.DateTimeFormat.ShortDatePattern = culture.DateTimeFormat.ShortDatePattern;
                this.DateSelectionMode = "Year";
            }
        }

        private void Filter()
        {
            if(this.DateType == StatisticsDateType.Period)
            {
                this.DailyCartesianChart(this.StartDate, this.EndDate);
                this.PeriodPieChart(this.StartDate, this.EndDate);
            }
            else if(this.DateType == StatisticsDateType.Monthly)
            {
                var firstDayOfMonth = new DateTime(this.PeriodDate.Date.Year, this.PeriodDate.Date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                this.DailyCartesianChart(firstDayOfMonth, lastDayOfMonth, true);
                this.PeriodPieChart(firstDayOfMonth, lastDayOfMonth);
            }
            else
            {
                var firstDayOfYear = new DateTime(this.PeriodDate.Date.Year, 1, 1);
                var lastDayOfYear = firstDayOfYear.AddYears(1).AddDays(-1);

                this.MonthlyCartesianChart(firstDayOfYear, lastDayOfYear);
                this.PeriodPieChart(firstDayOfYear, lastDayOfYear);
            }
        }
        
        private void DailyCartesianChart(DateTime startDate, DateTime endDate, bool monthly = false)
        {
            var data = this.user.Expenses.Where(e => e.Date.Date >= startDate.Date && e.Date.Date <= endDate.Date)
                .GroupBy(e => e.Date.Date)
                .Select(g => new
                {
                    Key = g.Key,
                    Value = g.Sum(e => e.LocalizedAmount)
                })
                .ToList();

            var collection = new ObservableCollection<CartesianData>();

            for (var day = startDate.Date; day <= endDate.Date; day = day.AddDays(1))
            {
                var match = data.FirstOrDefault(d => d.Key.Date == day);

                var format = "{0:dd/MM}";

                if (monthly)
                {
                    format = "{0:dd}";
                } 

                collection.Add(new CartesianData() {
                    Date = string.Format(format, day),
                    Value = match != null ? match.Value : 0M
                });
            }

            this.CartesianData = collection;
        }

        private void MonthlyCartesianChart(DateTime startDate, DateTime endDate)
        {
            var data = this.user.Expenses.Where(e => e.Date.Date >= startDate.Date && e.Date.Date <= endDate.Date)
                .GroupBy(e => e.Date.Date.Month)
                .Select(g => new
                {
                    Key = g.Key,
                    Value = g.Sum(e => e.LocalizedAmount)
                })
                .ToList();

            var collection = new ObservableCollection<CartesianData>();

            for (var month = startDate.Date.Month; month <= endDate.Date.Month; month += 1)
            {
                var match = data.FirstOrDefault(d => d.Key == month);

                var format = "{0}";

                collection.Add(new CartesianData()
                {
                    Date = string.Format(format, month),
                    Value = match != null ? match.Value : 0M
                });
            }

            this.CartesianData = collection;
        }

        private void PeriodPieChart(DateTime startDate, DateTime endDate)
        {
            var data = this.user.Expenses.Where(e => e.Date.Date >= startDate.Date && e.Date.Date <= endDate.Date)
                .GroupBy(e => e.CategoryName)
                .Select(g => new PieChartData
                {
                    Category = g.Key,
                    Value = g.Sum(e => e.LocalizedAmount)
                })
                .ToList();

            this.PieChartData = new ObservableCollection<PieChartData>(data);
        } 
    }
}
