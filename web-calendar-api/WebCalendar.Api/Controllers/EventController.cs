﻿using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebCalendar.Api.Extensions;
using WebCalendar.Business.Domains.Interfaces;
using WebCalendar.Business.ViewModels;

namespace WebCalendar.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class EventController : ControllerBase
  {
    private readonly IEventDomain _evDomain;

    public EventController(IEventDomain eventDomain)
    {
      _evDomain = eventDomain;
    }

    [HttpGet]
    public IActionResult GetEvent(int id)
    {
      var ev = _evDomain.GetEvent(id);
      return Ok(ev);
    }

    [HttpPost]
    public IActionResult AddCalendarEvent(EventViewModel calendarEvent)
    {
      string error = ValidateData(calendarEvent);
      if (error == string.Empty)
      {
        calendarEvent.Id = _evDomain.AddCalendarEvent(calendarEvent);
        return Ok(calendarEvent);
      }
      return BadRequest(error);
    }

    [HttpPut]
    public IActionResult UpdateCalendarEvent(EventViewModel calendarEvent)
    {
      string error = ValidateData(calendarEvent);
      if (error == string.Empty)
      {
        _evDomain.UpdateCalendarEvent(calendarEvent, User.GetId());
        return Ok(calendarEvent);
      }
      return BadRequest(error);
    }

    [HttpPut]
    [Route("EditEventSeries")]
    public IActionResult UpdateCalendarEventSeries(EventViewModel calendarEvent)
    {
      string error = ValidateData(calendarEvent);
      if (error == string.Empty)
      {
        _evDomain.UpdateCalendarEventSeries(calendarEvent, User.GetId());
        return Ok(calendarEvent);
      }
      return BadRequest(error);
    }

    [HttpDelete]
    public IActionResult DeleteEvent(int id)
    {
      _evDomain.DeleteCalendarEvent(id, User.GetId());
      return Ok();
    }

    [HttpDelete]
    [Route("DeleteSeries")]
    public IActionResult DeleteEventSeries([FromQuery] int id)
    {
      _evDomain.DeleteCalendarEventSeries(id, User.GetId());
      return Ok();
    }

    [HttpDelete]
    [Route("Unsubscribe")]
    public IActionResult UnsubscribeSharedEvent([FromQuery] int id)
    {
      _evDomain.UnsubscribeSharedEvent(id, User.GetId());
      return Ok();
    }

    [HttpDelete]
    [Route("UnsubscribeSeries")]
    public IActionResult UnsubscribeSharedEventSeries([FromQuery] int id)
    {
      _evDomain.UnsubscribeSharedEventSeries(id, User.GetId());
      return Ok();
    }

    [HttpGet("event-ics/{eventId}")]
    public IActionResult GetEventICS([FromRoute] int eventId)
    {
      var eventIcs = _evDomain.CreateEventICS(eventId, User.GetId());
      return File(Encoding.UTF8.GetBytes(eventIcs.ICSContent), "text/calendar", $"{eventIcs.CalendarName}.ics");
    }

    [HttpGet("event-series-ics/{eventId}")]
    public IActionResult GetEventSeriesICS([FromRoute] int eventId)
    {
      var eventIcs = _evDomain.CreateEventSeriesICS(eventId, User.GetId());
      return File(Encoding.UTF8.GetBytes(eventIcs.ICSContent), "text/calendar", $"{eventIcs.CalendarName}Series.ics");
    }

    string ValidateData(EventViewModel calendarEvent)
    {
      var totalTime = calendarEvent.EndDateTime - calendarEvent.StartDateTime;

      if (calendarEvent.EndDateTime < calendarEvent.StartDateTime)
      {
        return "End date/time was less than start date/time";
      }
      if (calendarEvent.EndDateTime == calendarEvent.StartDateTime)
      {
        return "The beginning and end of the event cannot be at the same time";
      }
      if (calendarEvent.Reiteration != null && totalTime.TotalDays > (int)calendarEvent.Reiteration)
      {
        return "Reiteration must be less or equal to the time interval";
      }
      return string.Empty;
    }
  }
}
