using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestingTutor.Dev.Data.Models
{
    public class SurveyAnswerQualitative : SurveyAnswer
    {
        public SurveyAnswerQualitative()
        {
            Type = SurveyAnswerTypes.Qualitative;
        }

        [DisplayName("Survey's Response")]
        public string Response { get; set; }
    }
}
