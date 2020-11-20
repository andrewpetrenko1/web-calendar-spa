﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebCalendar.Data.Entities;

namespace WebCalendar.Data
{
  public interface IWebCalendarDbContext
  {
    DbSet<User> Users { get; set; }
    DbSet<Calendar> Calendars { get; set; }
    DbSet<Event> Events { get; set; }
    DbSet<EventFile> EventFile { get; set; }

    int SaveChanges();
    EntityEntry Entry(object entity);
  }
}
