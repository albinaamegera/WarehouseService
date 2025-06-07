using System.Collections.ObjectModel;
using System.Net.WebSockets;

namespace WarehouseTestService.Packaging
{
    public class Pallet : Package
    {
        private const double DEFAULT_PALLET_WEIGHT = 30;

        private ReadOnlyCollection<Box> _caсhedBoxes;
        private List<Box> Boxes { get; }

        public Pallet(int id, double width, double height, double depth)
        {
            Id = id;
            Width = width;
            Height = height;
            Depth = depth;
            Boxes = new();
        }

        /// <summary>
        /// возвращает суммарный вес паллеты
        /// </summary>
        /// <exception cref="OverflowException">выбразывается, если результат бесконечный</exception>
        /// <returns>вес паллеты</returns>
        public override double GetWeight()
        {
            var result = GetWeightOfAllBoxes() + DEFAULT_PALLET_WEIGHT;

            ThrowIfResultOverflowed(result, "вес паллеты");

            return result;
        }

        /// <summary>
        /// возвращает суммарный объем паллеты
        /// </summary>
        /// <exception cref="OverflowException">выбразывается, если результат бесконечный</exception>
        /// <returns>объем паллеты</returns>
        public override double GetVolume()
        {
            var result = GetVolumeOfAllBoxes() + GetPalletVolume();

            ThrowIfResultOverflowed(result, "объем паллеты");

            return result;
        }

        /// <summary>
        /// возвращает дату срока годности паллеты
        /// </summary>
        /// <returns>дата срока годности</returns>
        /// <exception cref="InvalidOperationException">выбрасывается, если на паллете нет коробок для рассчета срока годности</exception>
        public override DateTime GetExpireDate() => GetMinExpireDateAmongBoxes();

        /// <summary>
        /// возвращает коллекцию коробок
        /// </summary>
        /// <returns>коллекция коробок только для чтения</returns>
        public ReadOnlyCollection<Box> GetBoxes()
        {
            if (_caсhedBoxes == null)
            {
                _caсhedBoxes = new ReadOnlyCollection<Box>(Boxes);
            }
            return _caсhedBoxes;
        }

        /// <summary>
        /// добавляет новую коробку в коллекцию коробок на паллете
        /// </summary>
        /// <param name="box">коробка для добавления</param>
        /// <exception cref="ArgumentNullException">выбрасывается при попытке добавить null значение</exception>
        /// <exception cref="InvalidOperationException">выбрасывается, если размеры коробки превышают допустимые значения для добавления в паллету</exception>
        public void AddBox(Box box)
        {
            if (box == null)
                throw new ArgumentNullException(nameof(box), "попытка добавить пустое значение в список коробок");

            bool boxFitsNormally = box.Width <= Width && box.Depth <= Depth;
            bool boxFitsRotated = box.Width <= Depth && box.Depth <= Width;

            if (!boxFitsNormally && !boxFitsRotated)
                throw new InvalidOperationException("размеры коробки превышают размеры паллеты");

            Boxes.Add(box);
        }

        /// <summary>
        /// возвращает суммарный вес всех коробок на паллете
        /// </summary>
        /// <returns>вес всех коробок</returns>
        private double GetWeightOfAllBoxes() => Boxes.Sum(b => b.GetWeight());

        /// <summary>
        /// возвращает суммарный объем всех коробок на паллете
        /// </summary>
        /// <returns>объем всех коробок</returns>
        private double GetVolumeOfAllBoxes() => Boxes.Sum(b => b.GetVolume());

        /// <summary>
        /// возвращает объем паллеты без коробок
        /// </summary>
        /// <returns>объем паллеты</returns>
        private double GetPalletVolume() => Width * Height * Depth;

        /// <summary>
        /// возваращает минимальное значение даты срока годности среди коробок
        /// </summary>
        /// <returns>дата срока годности</returns>
        /// <exception cref="InvalidOperationException">выбрасывается, если на паллете нет коробок</exception>
        private DateTime GetMinExpireDateAmongBoxes()
        {
            if (Boxes.Count == 0)
            {
                throw new InvalidOperationException("невозможно рассчитать срок годности паллеты, потому что нет доступных коробок");
            }

            return Boxes.MinBy(b => b.GetExpireDate())!.GetExpireDate();
        }
    }
}
