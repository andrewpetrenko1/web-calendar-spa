﻿using System;
using System.Collections.Generic;
using WebCalendar.Data.Entities;

namespace WebCalendar.Data.Repositories.Interfaces
{
  public interface ICalendarItemRepository
  {
    IEnumerable<Event> GetClaendarsEventsByTimeInterval(DateTime startDateTime, DateTime endDateTime, int[] calendarsId);
  }
}
