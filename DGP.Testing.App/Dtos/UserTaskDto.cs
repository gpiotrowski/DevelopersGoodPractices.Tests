using System;

namespace DGP.Testing.App.Dtos
{
    public class UserTaskDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}