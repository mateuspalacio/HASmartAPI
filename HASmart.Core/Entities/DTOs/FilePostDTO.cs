using HASmart.Core.Architecture;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HASmart.Core.Entities.DTOs
{
    public class FilePostDTO : DTO
    {
        public IFormFile File { get; set; }
    }
}
