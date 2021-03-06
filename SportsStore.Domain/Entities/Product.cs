﻿using System.ComponentModel.DataAnnotations;

namespace SportsStore.Domain.Entities
{
    public class Product
    {
        public int ProductID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
        public string Category { get; set; }

        public byte[] ImageData { get; set;  }

        public string ImageMimeType { get; set; }
    }
}
