using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestingTutor.Dev.Data.Models
{
    public class SurveyQuestionQualitative : SurveyQuestion
    {
        public SurveyQuestionQualitative()
        {
            Type = SurveyQuestionTypes.Qualitative;
        }

        [Required, DisplayName("Survey's Prompt"), MinLength(1)]
        public string Prompt { get; set; }
    }
}
