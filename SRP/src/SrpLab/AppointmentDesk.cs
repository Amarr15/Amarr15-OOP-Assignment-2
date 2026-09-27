namespace SrpLab;

public sealed class AppointmentDesk
{
    private readonly HashSet<DateTimeOffset> _booked = new();

    public TimeOnly Open { get; }
    public TimeOnly Close { get; }
    public int SlotMinutes { get; }

    public AppointmentDesk(
        TimeOnly open,
        TimeOnly close,
        int slotMinutes)
    {
        Open = open;
        Close = close;
        SlotMinutes = slotMinutes;
    }

    public bool IsWithinBusinessHours(DateTimeOffset when)
    {
        if (when.DayOfWeek is DayOfWeek.Friday or DayOfWeek.Saturday)
            return false;

        var time = TimeOnly.FromDateTime(when.DateTime);

        return time >= Open &&
               time.AddMinutes(SlotMinutes) <= Close;
    }

    public DateTimeOffset? FindNextSlot(
        DateTimeOffset from,
        int searchHours)
    {
        var cursor = Align(from);
        var end = from.AddHours(searchHours);

        while (cursor < end)
        {
            if (IsWithinBusinessHours(cursor) &&
                !_booked.Contains(cursor))
            {
                return cursor;
            }

            cursor = cursor.AddMinutes(SlotMinutes);
        }

        return null;
    }

    public bool TryBook(DateTimeOffset slot)
    {
        if (!IsWithinBusinessHours(slot) ||
            _booked.Contains(slot))
        {
            return false;
        }

        _booked.Add(slot);

        return true;
    }

    public string SmsReminder(
        DateTimeOffset slot,
        string clinicPhone)
    {
        return new SmsReminderGenerator()
            .Generate(slot, clinicPhone);
    }

    private DateTimeOffset Align(DateTimeOffset from)
    {
        var minutes = from.Minute -
                      (from.Minute % SlotMinutes);

        return new DateTimeOffset(
            from.Year,
            from.Month,
            from.Day,
            from.Hour,
            minutes,
            0,
            from.Offset);
    }
}


public sealed class IcsCalendarGenerator
{
    public string Generate(
        DateTimeOffset slot,
        int slotMinutes,
        string patientName,
        string clinician)
    {
        var uid = Guid.NewGuid();
        var end = slot.AddMinutes(slotMinutes);

        return "BEGIN:VCALENDAR\n" +
               "VERSION:2.0\n" +
               "BEGIN:VEVENT\n" +
               $"UID:{uid}\n" +
               $"DTSTART:{slot:yyyyMMdd'T'HHmmss'Z'}\n" +
               $"DTEND:{end:yyyyMMdd'T'HHmmss'Z'}\n" +
               $"SUMMARY:Visit {patientName} / {clinician}\n" +
               "END:VEVENT\n" +
               "END:VCALENDAR\n";
    }
}


public sealed class SmsReminderGenerator
{
    public string Generate(
        DateTimeOffset slot,
        string clinicPhone)
    {
        return $"Reminder: appointment {slot:MMM dd HH:mm}. " +
               $"Call {clinicPhone} to reschedule.";
    }
}