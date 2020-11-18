﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebCalendar.Data;
using WebCalendar.Data.Entities;
using WebCalendar.Data.Repositories;
using Xunit;

namespace WebCalendar.UnitTests.Data
{
  public class CalendarRepositoryTest : IDisposable
  {
    private readonly WebCalendarDbContext _context;
    public CalendarRepositoryTest()
    {
      var options = new DbContextOptionsBuilder();
      options.UseInMemoryDatabase("WebCalendarCalendarRepositoryTestInMemoryDatabase");

      _context = new WebCalendarDbContext(options.Options);
    }

    public void Dispose()
    {
      _context.Database.EnsureDeleted();
      _context.Dispose();
    }

    [Fact]
    [Trait("Repositories", "CalendarRepository")]
    public void GetUserCalendars_EmptyRepository_ReturnEmpty()
    {
      var calendarRepo = new CalendarRepository(_context);
      Assert.Empty(calendarRepo.GetUserCalendars(1));
    }

    [Fact]
    [Trait("Repositories", "CalendarRepository")]
    public void GetUserCalendars_ReturnUserCalendars()
    {
      var calendar1 = new Calendar
      {
        Name = "Calendar 1",
        Description = "Description 1",
        User = null,
        UserId = 1,
        Events = null
      };
      var calendar2 = new Calendar
      {
        Name = "Calendar 2",
        Description = "Description 2",
        User = null,
        UserId = 1,
        Events = null
      };      
      var calendar3 = new Calendar
      {
        Name = "Calendar 3",
        Description = "Description 3",
        User = null,
        UserId = 2,
        Events = null
      };

      _context.AddRange(calendar1, calendar2, calendar3);
      _context.SaveChanges();
      var calendarRepo = new CalendarRepository(_context);

      var actual = calendarRepo.GetUserCalendars(1);

      Assert.Collection(actual, 
        calendar => Assert.Contains("Calendar 1", calendar.Name),
        calendar => Assert.Contains("Calendar 2", calendar.Name));
    }

    [Fact]
    [Trait("Repositories", "CalendarRepository")]
    public void GetCalendar_InvalidCalendarId_ReturnNull()
    {
      var calendarRepo = new CalendarRepository(_context);
      var actual = calendarRepo.GetCalendar(3);

      Assert.Null(actual);
    }

    [Fact]
    [Trait("Repositories", "CalendarRepository")]
    public void GetCalendar_ValidCalendarId_ReturnCalendar()
    {
      var calendar1 = new Calendar
      {
        Id = 1,
        Name = "Calendar 1",
        Description = "Description 1",
        User = null,
        UserId = 1,
        Events = null
      };
      var calendar2 = new Calendar
      {
        Id = 2,
        Name = "Calendar 2",
        Description = "Description 2",
        User = null,
        UserId = 1,
        Events = null
      };

      _context.AddRange(calendar1, calendar2);
      _context.SaveChanges();
      var calendarRepo = new CalendarRepository(_context);

      var actual = calendarRepo.GetCalendar(1);

      Assert.Equal(calendar1, actual);
    }

    [Fact]
    [Trait("Repositories", "CalendarRepository")]
    public void AddCalendar_AddValid_ReturnId()
    {
      var calendar1 = new Calendar
      {
        Name = "Calendar 1",
        Description = "",
        User = null,
        UserId = 1,
        Events = null
      };
      var calendar2 = new Calendar
      {
        Name = "Calendar 2",
        Description = "",
        User = null,
        UserId = 1,
        Events = null
      };

      var calendarRepo = new CalendarRepository(_context);

      var actualId = calendarRepo.AddCalendar(calendar1);
      var actualId2 = calendarRepo.AddCalendar(calendar2);

      Assert.Equal(1, actualId);
      Assert.Equal(2, actualId2);
    }

    [Fact]
    [Trait("Repositories", "CalendarRepository")]
    public void RemoveCalendar_ValidCalendarId_ReturnTrue()
    {
      var calendar1 = new Calendar
      {
        Name = "Calendar 1",
        Description = "",
        User = null,
        UserId = 1,
        Events = null
      };
      var calendar2 = new Calendar
      {
        Name = "Calendar 2",
        Description = "",
        User = null,
        UserId = 1,
        Events = null
      };

      _context.AddRange(calendar1, calendar2);
      _context.SaveChanges();

      var calendarRepo = new CalendarRepository(_context);

      var actual = calendarRepo.DeleteCalendar(1);

      Assert.True(actual);
    }

    [Fact]
    [Trait("Repositories", "CalendarRepository")]
    public void RemoveCalendar_InvalidCalendarId_ReturnFalse()
    {
      var calendarRepo = new CalendarRepository(_context);
      var actual = calendarRepo.DeleteCalendar(2);

      Assert.False(actual);
    }

    [Fact]
    [Trait("Repositories", "CalendarRepository")]
    public void EditCalendar_CalendarChanged_ReturnTrue()
    {
      var calendar1 = new Calendar
      {
        Id = 1,
        Name = "Calendar 1",
        Description = "",
        User = null,
        UserId = 1,
        Events = null
      };

      var editedCalendar = new Calendar
      {
        Id = 1,
        Name = "EditedCalendar 1",
        Description = "Description",
        User = null,
        UserId = 1,
        Events = null
      };

      _context.Add(calendar1);
      _context.SaveChanges();

      var calendarRepo = new CalendarRepository(_context);
      var actual = calendarRepo.EditCalendar(editedCalendar);

      Assert.True(actual);
    }

    [Fact]
    [Trait("Repositories", "CalendarRepository")]
    public void EditCalendar_CalendarNotChanged_ReturnFalse()
    {
      var calendar1 = new Calendar
      {
        Id = 1,
        Name = "Calendar 1",
        Description = "",
        User = null,
        UserId = 1,
        Events = null
      };

      _context.Add(calendar1);
      _context.SaveChanges();

      var calendarRepo = new CalendarRepository(_context);
      var actual = calendarRepo.EditCalendar(calendar1);

      Assert.False(actual);
    }

    [Fact]
    [Trait("Repositories", "CalendarRepository")]
    public void EditCalendar_CalendarIdNotFound_ReturnFalse()
    {
      var calendar1 = new Calendar
      {
        Id = 1,
        Name = "Calendar 1",
        Description = "",
        User = null,
        UserId = 1,
        Events = null
      };

      var editedCalendar = new Calendar
      {
        Id = 2,
        Name = "EditedCalendar 1",
        Description = "Description",
        User = null,
        UserId = 1,
        Events = null
      };

      _context.Add(calendar1);
      _context.SaveChanges();

      var calendarRepo = new CalendarRepository(_context);
      var actual = calendarRepo.EditCalendar(editedCalendar);

      Assert.False(actual);
    }
  }
}
