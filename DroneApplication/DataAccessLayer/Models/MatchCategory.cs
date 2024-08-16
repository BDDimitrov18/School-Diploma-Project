using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class MatchCategory
    {
        
        public List<SelectListItem> midFilesList { get; set; }
        public List<SelectListItem> exifFilesList { get; set; }
        [Required]
        public string newFileName { get; set; }
        [Required]
        public string midFileId { get; set; }
        [Required]
        public string exifFileId { get; set; }
    }
}
