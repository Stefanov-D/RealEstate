using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Models.PostInputModels
{
    public class ImageInputModel
    {
        public Guid Id = Guid.NewGuid();

        public string ImageUrl { get; set; } = null!;

        public bool IsPrimary { get; set; }

        public Guid ListingId { get; set; }
    }
}
