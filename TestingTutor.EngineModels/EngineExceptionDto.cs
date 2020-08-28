using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestingTutor.EngineModels
{
    public class EngineExceptionDto : Exception
    {
        public string Phase { get; set; }
        public string From { get; set; }
        public string Report { get; set; }
    }
}
