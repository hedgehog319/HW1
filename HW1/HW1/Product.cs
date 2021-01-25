using System.Collections.Generic;

namespace HW1
{
    public class Product
    {
        public string Name { get; set; }
        public string PicPath { get; set; }
        public string Description { get; set; }
    }

    public class OrderProduct
    {
        public string Name { get; set; }

        public int Count { get; set; }

        public string PicPath { get; set; }
    }
}