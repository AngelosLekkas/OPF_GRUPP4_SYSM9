using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.Shared.DTOs
{
    public class SubCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsLocked { get; set; }
        public int QuestionCount { get; set; }
    }
}
