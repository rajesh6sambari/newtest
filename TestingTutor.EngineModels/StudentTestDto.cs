using System.Collections.Generic;

namespace TestingTutor.EngineModels
{
    public class StudentTestDto
    {
        public string Name { get; set; }
        public TestStatusEnum TestStatus { get; set; }
        public bool Passed { get; set; }

        public override bool Equals(object obj)
        {
            var dto = obj as StudentTestDto;
            return dto != null &&
                   Name == dto.Name &&
                   TestStatus == dto.TestStatus &&
                   Passed == dto.Passed;
        }

        public static bool operator ==(StudentTestDto dto1, StudentTestDto dto2)
        {
            return EqualityComparer<StudentTestDto>.Default.Equals(dto1, dto2);
        }

        public static bool operator !=(StudentTestDto dto1, StudentTestDto dto2)
        {
            return !(dto1 == dto2);
        }
    }
}
