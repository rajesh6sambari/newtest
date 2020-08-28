using System.Collections.Generic;

namespace TestingTutor.EngineModels
{
    public class ClassCoverageDto
    {
            public string Name { get; set; }
            public string Container { get; set; }
            public IList<MethodCoverageDto> MethodCoveragesDto { get; set; } = new List<MethodCoverageDto>();
    }
}
