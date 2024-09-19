using System;
using System.ComponentModel.DataAnnotations;
using TestApi.Model;

namespace Test.Models
{
	public class ViewModelSubCategories
	{
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public bool Status { get; set; }

        public Categories? Category { get; set; }

        public int CategoryId { get; set; }

        public string? Categoryy { get; set; }
    }
}

