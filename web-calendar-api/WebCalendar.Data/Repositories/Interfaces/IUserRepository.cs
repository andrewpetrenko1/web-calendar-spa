﻿using System.Collections.Generic;
using WebCalendar.Data.Entities;

namespace WebCalendar.Data.Repositories.Interfaces
{
  public interface IUserRepository
  {
    User GetByEmail(string email);
    User GetUser(int id);
    IEnumerable<User> GetUsersExceptCurrent(int id);
    void Create(User user);
    bool Edit(User user);
  }
}
