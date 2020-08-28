using System.Collections.Generic;

namespace TestingTutor.EngineModels
{
    public class InstructorTestDto
    {
        public string Name { get; set; }
        public string EquivalenceClass { get; set; }
        public string[] Concepts { get; set; }
        public bool Passed { get; set; }
        public TestStatusEnum TestStatus { get; set; }
        public IList<StudentTestDto> StudentTests { get; set; } = new List<StudentTestDto>();
    }
}
