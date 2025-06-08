using WarehouseTestService.Packaging;

namespace WarehouseTestService.Tests
{
    public class BoxTests
    {
        #region Methods Positive

        [Theory]
        [InlineData(10, 10)]
        [InlineData(6.87, 6.87)]
        [InlineData(1_000_000, 1_000_000)]
        public void GetWeight_ReturnsExpectedValue(double weight, double expected)
        {
            var box = GetBox(weight: weight);

            var actual = box.GetWeight();

            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData(1, 1, 1, 1)]
        [InlineData(20, 15, 17, 5100)]
        [InlineData(6.5, 1.3, 9.3, 78.585)]
        public void GetVolume_ReturnsExpectedValue(double width, double height, double depth, double expected)
        {
            var box = GetBox(width, height, depth);

            var actual = box.GetVolume();

            Assert.Equal(expected, actual, 4);
        }
        [Fact]
        public void GetExpireDate_ReturnsExpectedValue()
        {
            var date = GetDate();
            var box = GetBoxWithDate(date);

            var actual = box.GetExpireDate();

            Assert.Equal(GetDateHundredDaysAdded(date), actual);
        }

        #endregion

        #region Create Instance Negative
        
        [Fact]
        public void CreateInstance_InvalidWidth_ThrowsArgumentOutOfRangeException()
        {
            var invalidWidth = 0;

            Assert.Throws<ArgumentOutOfRangeException>(() => GetBox(width: invalidWidth));
        }
        [Fact]
        public void CreateInstance_InvalidHeight_ThrowsArgumentOutOfRangeException()
        {
            var invalidHeight = 0;

            Assert.Throws<ArgumentOutOfRangeException>(() => GetBox(height: invalidHeight));
        }
        [Fact]
        public void CreateInstance_InvalidDepth_ThrowsArgumentOutOfRangeException()
        {
            var invalidDepth = 0;

            Assert.Throws<ArgumentOutOfRangeException>(() => GetBox(depth: invalidDepth));
        }
        [Fact]
        public void CreateInstance_InvalidWeight_ThrowsArgumentOutOfRangeException()
        {
            var invalidWeight = -1;

            Assert.Throws<ArgumentOutOfRangeException>(() => GetBox(weight: invalidWeight));
        }
        [Fact]
        public void CreateInstance_InvalidProductionDate_ThrowsArgumentOutOfRangeException()
        {
            var invalidProductionDate = DateTime.MaxValue;

            Assert.Throws<ArgumentOutOfRangeException>(() => GetBoxWithDate(invalidProductionDate));
        }
        #endregion
        #region Methods Negative
        [Fact]
        public void GetVolume_ThrowsOverflowException()
        {
            var height = double.MaxValue;
            var width = double.MaxValue;
            var box = GetBox(width: width, height: height);

            Assert.Throws<OverflowException>(() => box.GetVolume());
        }
        #endregion
        #region Private
        private const int DEFAULT_EXPIRATION_OFFSET_IN_DAYS = 100;

        private Box GetBox(double width = 1, double height = 1, double depth = 1, double weight = 1)
        {
            return GetBoxWithDate(GetDate(), width, height, depth, weight);
        }
        private Box GetBoxWithDate(DateTime productionDate, double width = 1, double height = 1, double depth = 1, double weight = 1)
        {
            return new Box(width, height, depth, weight, productionDate);
        }
        private DateTime GetDate() => DateTime.MaxValue.AddDays(-DEFAULT_EXPIRATION_OFFSET_IN_DAYS - 1);
        private DateTime GetDateHundredDaysAdded(DateTime dateTime) => dateTime.AddDays(DEFAULT_EXPIRATION_OFFSET_IN_DAYS);
        #endregion
    }
}