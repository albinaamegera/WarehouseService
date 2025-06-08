using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseTestService.Packaging
{
    public abstract class Package
    {
        private double _width;
        private double _height;
        private double _depth;

        /// <summary>
        /// возвращает или устанавливает ширину. допустимы только положительные значения
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">выбрасывается, если value отрицательное</exception>
        /// <value>значение ширины</value>
        public double Width 
        { 
            get => _width;
            protected set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(Width), "значение ширины не может быть отрицательным");
                _width = value;
            }
        }
        /// <summary>
        /// возвращает или устанавливает высоту. допустимы только положительные значения
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">выбрасывается, если value отрицательное</exception>
        /// <value>значение высоты</value>
        public double Height 
        { 
            get => _height;
            protected set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(Height), "значение высоты не может быть отрицательным");
                _height = value;
            }
        }
        /// <summary>
        /// возвращает или устанавливает глубину. допустимы только положительные значения
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">выбрасывается, если value отрицательное</exception>
        /// <value>значение глубины</value>
        public double Depth 
        { 
            get => _depth;
            protected set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(Depth), "значение глубины не может быть отрицательным");
                _depth = value;
            }
        }
        public abstract double GetWeight();
        public abstract double GetVolume();
        public abstract DateTime GetExpireDate();
        /// <summary>
        /// вспомогательный метод для проверки валидности возвращаемого значения
        /// </summary>
        /// <param name="result">результат вычисления метода</param>
        /// <param name="context">контекст переполнения</param>
        /// <exception cref="OverflowException">выбрасывается, если result бесконечный</exception>
        protected void ThrowIfResultOverflowed(double result, string context)
        {
            if (double.IsInfinity(result) || double.IsNaN(result))
                throw new OverflowException($"переполнение при вычислении значения: {context}");
        }
    }
}