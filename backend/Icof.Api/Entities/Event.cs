using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Icof.Api.Entities
{
    public class Event
    {
        public Guid Id { get; set; }
        public String Title { get; set; } 
        public DateTime StartsAt { get; set; }

        public ICollection<EventRegistration> Registration { get; set; } = new List<EventRegistration>();
    }
}