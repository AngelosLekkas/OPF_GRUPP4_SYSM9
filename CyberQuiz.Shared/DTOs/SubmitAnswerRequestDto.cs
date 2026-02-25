using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.Shared.DTOs
{
    //Request: UI/API -> BLL när User svarar på en fråga
    public class SubmitAnswerRequestDto
    {
        public int QuestionId { get; set; }
        public int AnswerOptionId { get; set; }
    }
}
