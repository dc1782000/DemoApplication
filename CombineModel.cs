using System;
using TestApi.Model;

namespace Test.Models
{
	public class CombineModel
	{
        public IEnumerable<ViewModelProduct> viewModelProducts { get; set; }
        public ViewModelProduct viewModelProduct { get; set; }
    }
}

