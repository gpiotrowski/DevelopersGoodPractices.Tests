using System;

namespace DGP.Testing.App.Models
{
    public class UserTask
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
