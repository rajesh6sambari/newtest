using TestingTutor.Dev.Data.Models;

namespace TestingTutor.Dev.Engine.Data
{
    public class EmailData
    {
        public EmailData()
        {

        }

        public EmailData(Student student)
        {
            Email = student.Email;
            Name = student.Name;
        }

        public string Email { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

    }
}
