using System;
namespace Test.Models
{
	public class ProductRetriveModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public bool Status { get; set; }

        public int SubcategoryId { get; set; }

        public int CategoryId { get; set; }

        public int TitleId { get; set; }

        public string TitleName { get; set; }

        public int HeadingId { get; set; }

        public string HeadingName { get; set; }

        public int DescriptionId { get; set; }

        public string DescriptionText { get; set; }
	}
}

