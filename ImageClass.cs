using System;
namespace Test.Models
{
	public class ImageClass
	{
		public int Id { get; set; }

		public string PdfHeading { get; set; }


		public IFormFile formFile { get; set; }
	}
}

