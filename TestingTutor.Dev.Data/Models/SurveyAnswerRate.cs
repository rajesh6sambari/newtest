using System.ComponentModel;

namespace TestingTutor.Dev.Data.Models
{
    public class SurveyAnswerRate : SurveyAnswer
    {
        public SurveyAnswerRate()
        {
            Type = SurveyAnswerTypes.Rate;
        }

        [DisplayName("Survey's Selection")]
        public int Selection { get; set; }
    }
}
