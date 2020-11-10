using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Demos.Sf.Models;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Schedule;

namespace Demos.Sf.Pages
{
    public partial class Index
    {

        private List<Appointment> _allAppointments;


        public Index()
        {
            AppointmentsData = new List<Appointment>();
            CurrentView = View.Month;
        }


        [Inject]
        protected HttpClient HttpClient { get; set; }

        public DateTime SelectedDate { get; set; }

        public View CurrentView { get; set; }

        protected List<Appointment> AppointmentsData { get; set; } 


        protected async override Task OnInitializedAsync()
        {
            CurrentView = View.Month;
            SelectedDate = DateTime.Today;
            await LoadData();
            await base.OnInitializedAsync();
        }


        public async Task LoadData()
        {
            if(_allAppointments == null || !_allAppointments.Any()) _allAppointments = await HttpClient.GetFromJsonAsync<List<Appointment>>("sample-data/appointments.json");


            var startDate = new DateTime(SelectedDate.Year, SelectedDate.Month, 1).AddDays(-7);
            var endDate = new DateTime(SelectedDate.Year, SelectedDate.Month, DateTime.DaysInMonth(SelectedDate.Year, SelectedDate.Month)).AddDays(7);

            AppointmentsData = _allAppointments.Where(x => x.StartTime >= startDate && x.EndTime <= endDate).ToList();
        }



        public async  Task CalendarValueChanged(ChangedEventArgs<DateTime> args)
        {
            SelectedDate = args.Value;
            await LoadData();
        }

        public async Task OnCalendarViewChange(ChangeEventArgs e)
        {
            var value = (string) e.Value;
            CurrentView = (View) Enum.Parse(typeof(View), value);
            await LoadData();
        }

    }
}