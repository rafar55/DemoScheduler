using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorPro.BlazorSize;
using Demos.Sf.Models;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Schedule;

namespace Demos.Sf.Pages
{
    public partial class Index : IDisposable
    {



        private List<Appointment> _allAppointments;

        private bool _dataLoaded = false;


        public Index()
        {
            CurrentView = View.Month;
            AppointmentsData = new List<Appointment>();
        }


        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Inject]
        private ResizeListener ResizeListener { get; set; }

        public DateTime SelectedDate { get; set; }

        public View CurrentView { get; set; }

        protected List<Appointment> AppointmentsData { get; set; }


        protected async override Task OnInitializedAsync()
        {
            SelectedDate = DateTime.Today;
            await LoadData();
            _dataLoaded = true;
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                ResizeListener.OnResized += WindowResized;
            }
        }


        private async void WindowResized(object _, BrowserWindowSize window)
        {
            // Get the browsers's width / height
            var browser = window;

            await SelectCalendarView();


            StateHasChanged();
        }


        private async Task SelectCalendarView()
        {

            var IsSmallMedia = await ResizeListener.MatchMedia(Breakpoints.SmallDown);

            if (IsSmallMedia && _dataLoaded)
            {
                CurrentView = View.MonthAgenda;
            }

            else if (CurrentView == View.MonthAgenda)
            {
                CurrentView = View.Month;
            }
        }

        public async Task LoadData()
        {
            if (_allAppointments == null || !_allAppointments.Any())
            {
                _allAppointments = await HttpClient.GetFromJsonAsync<List<Appointment>>("sample-data/appointments.json");
            }


            var startDate = new DateTime(SelectedDate.Year, SelectedDate.Month, 1).AddDays(-7);
            var endDate = new DateTime(SelectedDate.Year, SelectedDate.Month, DateTime.DaysInMonth(SelectedDate.Year, SelectedDate.Month)).AddDays(7);

            AppointmentsData = _allAppointments.Where(x => x.StartTime >= startDate && x.EndTime <= endDate).ToList();




            //AppointmentsData = new List<Appointment>();
        }


        public async Task OnDataBound(DataBoundEventArgs<Appointment> args)
        {
            await SelectCalendarView();
        }



        public async Task CalendarValueChanged(ChangedEventArgs<DateTime> args)
        {
            //SelectedDate = args.Value;
            await LoadData();
        }

        public async Task OnCalendarViewChange(ChangeEventArgs e)
        {
            var value = (string)e.Value;
            CurrentView = (View)Enum.Parse(typeof(View), value);
            await LoadData();
        }

        public void Dispose()
        {
            ResizeListener.OnResized -= WindowResized;
        }
    }
}