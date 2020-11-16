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
using Demos.Sf.Services;

namespace Demos.Sf.Pages
{
    public partial class Index : IDisposable
    {

        private List<Appointment> _allAppointments;

        public Index()
        {
            AppointmentsData = new List<Appointment>();
            CurrentView = View.Month;
        }

        [Inject] protected BrowserService _service { get; set; }

        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Inject]
        private ResizeListener ResizeListener { get; set; }

        public DateTime SelectedDate { get; set; }

        public View CurrentView { get; set; }
        
        public int HeightDimention { get; set; }
        public int WidthDimention { get; set; }

        protected List<Appointment> AppointmentsData { get; set; }


        protected async override Task OnInitializedAsync()
        {
            CurrentView = View.Month;
            SelectedDate = DateTime.Today;
            await LoadData();
            await GetDimensions();
            await base.OnInitializedAsync();
            
            
        }
        
        protected  async Task GetDimensions() {
            var dimension = await Service.GetDimensions();
            HeightDimention = dimension.Height;
            WidthDimention = dimension.Width;
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ResizeListener.OnResized += WindowResized;
            }

            return base.OnAfterRenderAsync(firstRender);
        }


        private async void WindowResized(object _, BrowserWindowSize window)
        {
            // Get the browsers's width / height
            var browser = window;

            // Check a media query to see if it was matched. We can do this at any time, but it's best to check on each resize
            var IsSmallMedia = await ResizeListener.MatchMedia(Breakpoints.SmallDown);


            if (IsSmallMedia)
            {
                GetDimensions();
                CurrentView = View.MonthAgenda;
            }
            else
            {
                GetDimensions();
                CurrentView = View.Month;
            }

            // We're outside of the component's lifecycle, be sure to let it know it has to re-render.
            StateHasChanged();
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