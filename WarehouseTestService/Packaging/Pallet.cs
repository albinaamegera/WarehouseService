using System.Collections.ObjectModel;

namespace WarehouseTestService.Packaging
{
    public class Pallet : Package
    {
        private const double DEFAULT_PALLET_WEIGHT = 30;

        private List<Box> Boxes { get; }

        public Pallet(int id, double width, double height, double depth)
        {
            Id = id;
            Width = width;
            Height = height;
            Depth = depth;
            Boxes = new();
        }
        public override double GetWeight() => GetWeightOfAllBoxes() + DEFAULT_PALLET_WEIGHT;
        public override double GetVolume() => GetVolumeOfAllBoxes() + GetPalletVolume();
        public override DateTime GetExpireDate() => GetMinExpireDateAmongBoxes();
        public ReadOnlyCollection<Box> GetBoxes() => new ReadOnlyCollection<Box>(Boxes);
        public void AddBox(Box box)
        {
            if (box == null)
                throw new ArgumentNullException(nameof(box), "попытка добавить пустое значение в список коробок");

            bool isBoxUnkeepable = box.Width > Width || box.Depth > Width || box.Width > Depth || box.Depth > Depth;

            if (isBoxUnkeepable)
                throw new InvalidOperationException("размеры коробки превышают размеры паллеты");

            Boxes.Add(box);
        }
        private double GetWeightOfAllBoxes()
        {
            double weight = 0;

            foreach (var box in Boxes)
            {
                weight += box.GetWeight();
            }

            return weight;
        }
        private double GetVolumeOfAllBoxes()
        {
            double volume = 0;

            foreach (var box in Boxes)
            {
                volume += box.GetVolume();
            }

            return volume;
        }
        private DateTime GetMinExpireDateAmongBoxes()
        {
            if (Boxes.Count == 0)
            {
                throw new InvalidOperationException("невозможно рассчитать срок годности паллеты, потому что нет доступных коробок");
            }

            return Boxes.MinBy(b => b.GetExpireDate())!.GetExpireDate();
        }
        private double GetPalletVolume() => Width * Height * Depth;
    }
}
