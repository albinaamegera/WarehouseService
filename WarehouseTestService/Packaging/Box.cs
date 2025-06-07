namespace WarehouseTestService.Packaging
{
    public class Box : Package
    {
        private const int DEFAULT_EXPIRATION_OFFSET_IN_DAYS = 100;

        private double _weight;
        private DateTime _productionDate;

        public double Weight { get => _weight; private set => _weight = value; }
        public DateTime ProductionDate { get => _productionDate; private set => _productionDate = value; }
        public Box(int id, double width, double height, double depth, double weight, DateTime productionDate)
        {
            Id = id;
            Width = width;
            Height = height;
            Depth = depth;
            Weight = weight;
            ProductionDate = productionDate;
        }
        public override double GetWeight() => Weight;
        public override double GetVolume() => Width * Height * Depth;
        public override DateTime GetExpireDate() => ProductionDate.AddDays(DEFAULT_EXPIRATION_OFFSET_IN_DAYS);
    }
}
