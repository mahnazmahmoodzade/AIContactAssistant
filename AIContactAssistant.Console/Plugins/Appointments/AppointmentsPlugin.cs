using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace AIContactAssistant.Console.Plugins.Appointments
{
    [Description("Appointment scheduling system for technical support, installation visits, and customer service meetings")]
    public class AppointmentsPlugin
    {
        [KernelFunction]
        [Description("Schedules an appointment for customer service, technical support, installation, or consultation. Returns confirmed appointment slot.")]
        public Task<object> Schedule(
            [Description("Purpose of appointment: 'technical_support', 'installation', 'consultation', 'device_pickup', 'account_review'")] string purpose, 
            [Description("Customer account identifier for appointment booking")] string accountId, 
            [Description("Array of preferred appointment times in ISO format (e.g. ['2025-08-12T10:00Z', '2025-08-12T14:00Z'])")] string[] preferredTimes) =>
            Task.FromResult<object>(new { 
                confirmedSlot = "2025-08-12T10:00Z",
                appointmentId = "appt_" + Guid.NewGuid().ToString("N")[..8],
                purpose = purpose,
                duration = "60 minutes",
                location = "Customer premises or service center",
                confirmationSent = true,
                nextSteps = "Confirmation email sent with appointment details"
            });
    }
}