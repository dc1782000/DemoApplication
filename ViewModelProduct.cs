
using System;
using System.ComponentModel.DataAnnotations;
using TestApi.Model;

namespace Test.Models
{
	public class ViewModelProduct
	{
        [Key]
        public int Id { get; set; }


        public Categories? Category { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string? Categoryy { get; set; }

        public int? CategoryId { get; set; }

        public List<DemoSubCategory>? SubCategories { get; set; }

        [Required(ErrorMessage = "Atleast one Sub-Category is required")]
        public List<string>? SubCategoriess { get; set; }

        public List<int>? SubCategoriessId { get; set; }

        public string? SubCategoriessName { get; set; }

        public string? Name { get; set; }

        public string? ShortDescription { get; set; }

        public bool Status { get; set; }

        public List<string>? Title { get; set; } = new List<string>();

        public List<string>? Heading { get; set; }= new List<string>();

        public List<string>? Description { get; set; }= new List<string>();

        public List<string>? PdfName { get; set; } = new List<string>();

        public string? Content { get; set; }

        public List<string>? PdfHeading { get; set; } = new List<string>();

        public List<ImageClass>? FormFiless { get; set; } = new List<ImageClass>();

        public List< int> PdfId{ get; set; } = new List<int>();

        public IFormFile? Picture { get; set; } 

        public int PictureId { get; set; }

        public List<string> deletePdfName { get; set; } = new List<string>();

        public List<int> deletePdfId { get; set; } = new List<int>();
        public string? PictureName { get; set; }

        public string? PicturePath { get; set; }

        public string? FullDesciption { get; set; }

    }


    
}

