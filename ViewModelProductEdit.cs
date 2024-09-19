using System;
using System.ComponentModel.DataAnnotations;
using TestApi.Model;

namespace Test.Models
{
	public class ViewModelProductEdit
	{
        public int Id { get; set; }



        [Required(ErrorMessage = "Category is required")]
        public string? Categoryy { get; set; }



        [Required(ErrorMessage = "Atleast one Sub-Category is required")]
        public List<string>? SubCategoriessName { get; set; } = new List<string>();

        public string? Name { get; set; }

        public string? ShortDescription { get; set; }

        public bool Status { get; set; }

        public string? Title { get; set; }

        public string? Heading { get; set; }

        public string? Description { get; set; }

    }
}

