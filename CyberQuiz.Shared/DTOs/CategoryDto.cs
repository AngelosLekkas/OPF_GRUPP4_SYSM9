using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.Shared.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CompletedSubCategories { get; set; }
        public int TotalSubCategories { get; set; }

    }
}
