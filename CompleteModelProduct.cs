using System;
using TestApi.Model;

namespace Test.Models
{
	public class CompleteModelProduct
	{
		public int Id { get; set; }

        public List<string> Titles { get; set; }

        public List<string> Headings { get; set; }

        public List<string> Descriptions { get; set; }

        public List<string> SubCategoriess { get; set; }

        public List<string> PdfHeadings { get; set; }

        public List<string> PdfName { get; set; }

        public string? PictureName { get; set; }

        public string? Content { get; set; }

        public string Name { get; set; }

        public string Shortdecription { get; set; }

        public string Category { get; set; }

        public bool Statuss { get; set; }

        
    }
}

