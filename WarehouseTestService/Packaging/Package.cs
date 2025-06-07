using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseTestService.Packaging
{
    public abstract class Package
    {
        private int _id;
        private double _width;
        private double _height;
        private double _depth;
        public int Id 
        { 
            get => _id;
            protected set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Id), "индекс не может быть отрицательным");
                _id = value;
            }
        }
        public double Width 
        { 
            get => _width;
            protected set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Width), "значение ширины не может быть отрицательным");
                _width = value;
            }
        }
        public double Height 
        { 
            get => _height;
            protected set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Height), "значение высоты не может быть отрицательным");
                _height = value;
            }
        }
        public double Depth 
        { 
            get => _depth;
            protected set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(Depth), "значение глубины не может быть отрицательным");
                _depth = value;
            }
        }
        public abstract double GetWeight();
        public abstract double GetVolume();
        public abstract DateTime GetExpireDate();
    }
}