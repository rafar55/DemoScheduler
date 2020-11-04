using System;
using System.Collections.Generic;
using Demos.Sf.Models;
using Syncfusion.Blazor.Schedule;

namespace Demos.Sf.Pages
{
    public partial class Index
    {
        private List<AppointmentData> _appointmentData = new List<AppointmentData>
        {
            new AppointmentData
            {
                Id = 1,
                Subject = "Ir al Banco",
                StartTime = DateTime.Parse("2020-11-4 15:30"),
                EndTime = DateTime.Parse("2020-11-4 16:30:00")
            },
            new AppointmentData
            {
                Id = 1,
                Subject = "Ir con lula a comer",
                StartTime = DateTime.Parse("2020-11-5 16:40:00"),
                EndTime = DateTime.Parse("2020-11-5 19:30:00")
            }
        };

    }
}