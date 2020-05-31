using DQueensFashion.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DQueensFashion.Models
{
    public class ViewCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class AddCategoryViewModel
    {
        [Required]
        [BeginWIthAlphabeth(ErrorMessage ="Name must begin with an alphabeth")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Category name must at least be 2 characters long and not more than 50")]
        [Remote("CheckUniqueCategoryName", "Admin", ErrorMessage = "category already exists")]
        public string Name { get; set; }
    }

    public class EditCategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        [BeginWIthAlphabeth(ErrorMessage = "Name must begin with an alphabeth")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Category name must at least be 2 characters long and not more than 50")]
        [Remote("CheckUniqueCategoryNameInEdit", "Admin", ErrorMessage = "category already exists")]
        public string Name { get; set; }
    }

    public class CategoryNameAndId
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Selected { get; set; }
    }
}