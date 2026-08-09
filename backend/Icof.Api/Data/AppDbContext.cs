using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Icof.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Icof.Api.Data
{
    public class AppDbContext:DbContext
    {
        public DbSet<Event> Events => Set<Event>();
        public DbSet<EventRegistration> EventRegistrations => Set<EventRegistration>();
    } 
}