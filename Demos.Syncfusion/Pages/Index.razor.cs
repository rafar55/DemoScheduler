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

        public Index()
        {
            AppointmentsData = new List<Appointment>();
        }


        [Inject]
        protected HttpClient HttpClient { get; set; }

        public DateTime SelectedDate { get; set; }

        protected List<Appointment> AppointmentsData { get; set; } 


        protected async override Task OnInitializedAsync()
        {
            SelectedDate = DateTime.Today;
            await LoadData();
            await base.OnInitializedAsync();
        }


        public async Task LoadData()
        {
            AppointmentsData = await HttpClient.GetFromJsonAsync<List<Appointment>>("sample-data/appointments.json");          
        }



        public void CalendarValueChanged(ChangedEventArgs<DateTime> args)
        {
            SelectedDate = args.Value;            
        }

    }
}