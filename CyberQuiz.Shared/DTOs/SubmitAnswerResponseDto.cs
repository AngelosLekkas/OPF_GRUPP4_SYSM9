using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;

namespace CyberQuiz.Shared.DTOs
{
    //Response: skickas tillbaka till UI efter att svaret sparats för att visa rätt/fel + uppdaterad progression
    public class SubmitAnswerResponseDto
    {
        public int QuestionId { get; set; }
        public int AnswerOptionId { get; set; }
        public bool IsCorrect { get; set; }
        public int CorrectAnswerOptionId { get; set; }
        public double SubCategoryScorePercent { get; set; }
        public bool SubCategoryCompleted { get; set; }
    }
}
