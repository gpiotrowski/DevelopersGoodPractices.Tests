using System;

namespace DGP.Testing.App
{
    public class DuplicatedTaskTextException : Exception
    {
        public DuplicatedTaskTextException() : base("Task with that text already exist")
        {
            
        }
    }
}
