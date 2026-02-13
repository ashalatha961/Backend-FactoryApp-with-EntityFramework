using System.ComponentModel.DataAnnotations;

namespace PopsicleFactoryCo.Models

{
    public class PopsicleModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Flavour { get; set; }

        public int Price { get; set; }

        public int Quantity { get; set; }
    }
}
