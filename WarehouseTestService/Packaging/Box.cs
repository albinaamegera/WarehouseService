namespace WarehouseTestService.Packaging
{
    public class Box : Package
    {
        private const int DEFAULT_EXPIRATION_OFFSET_IN_DAYS = 100;

        private double _weight;
        private DateTime _productionDate;

        /// <summary>
        /// возвращает или устанавливает вес. допустимы только положительные значения
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">выбрасывается, если value отрицательное</exception>
        /// <value>значение веса</value>
        public double Weight 
        { 
            get => _weight;
            private set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(Weight), "значение веса не может быть отрицательным");
                _weight = value;
            }
        }
        /// <summary>
        /// возвращает или устанавливает дату изготовления
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">выбрасывается, если нельзя рассчитать срок годности</exception>
        /// <value>дата</value>
        public DateTime ProductionDate 
        { 
            get => _productionDate;
            private set
            {
                if (value > DateTime.MaxValue.AddDays(-DEFAULT_EXPIRATION_OFFSET_IN_DAYS))
                    throw new ArgumentOutOfRangeException(nameof(ProductionDate), "дата производства близка к максимально возможной");
                _productionDate = value;
            }
        }
        public Box(double width, double height, double depth, double weight, DateTime productionDate)
        {
            Width = width;
            Height = height;
            Depth = depth;
            Weight = weight;
            ProductionDate = productionDate;
        }
        /// <summary>
        /// возвращает значение веса коробки
        /// </summary>
        /// <returns>значение веса</returns>
        public override double GetWeight() => Weight;
        /// <summary>
        /// возвращает значение объема коробки
        /// </summary>
        /// <exception cref="OverflowException">выбразывается, если результат бесконечный</exception>
        /// <returns>объем коробки</returns>
        public override double GetVolume()
        {
            var result = Width * Height * Depth;

            ThrowIfResultOverflowed(result, "объем коробки");

            return result;
        }
        /// <summary>
        /// возвращает дату срока годности коробки
        /// </summary>
        /// <returns>дата срока годности</returns>
        public override DateTime GetExpireDate() => ProductionDate.AddDays(DEFAULT_EXPIRATION_OFFSET_IN_DAYS);
    }
}
