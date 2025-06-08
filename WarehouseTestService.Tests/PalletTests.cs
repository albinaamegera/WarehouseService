using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseTestService.Packaging;

namespace WarehouseTestService.Tests
{
    public class PalletTests
    {
        #region CreateInstance

        [Fact]
        public void CreateInstance_InvalidWidth_ThrowsArgumentOutOfRangeException()
        {
            double invalidWidth = 0;

            Assert.Throws<ArgumentOutOfRangeException>(() => GetPallet(width: invalidWidth));
        }
        [Fact]
        public void CreateInstance_InvalidHeight_ThrowsArgumentOutOfRangeException()
        {
            double invalidHeight = 0;

            Assert.Throws<ArgumentOutOfRangeException>(() => GetPallet(height: invalidHeight));
        }
        [Fact]
        public void CreateInstance_InvalidDepth_ThrowsArgumentOutOfRangeException()
        {
            double invalidDepth = 0;

            Assert.Throws<ArgumentOutOfRangeException>(() => GetPallet(depth: invalidDepth));
        }

        #endregion

        #region AddBox

        [Theory]
        [InlineData(3, 7, 3, 7)]
        [InlineData(5, 5, 6, 6)]
        [InlineData(2, 2, 2, 2)]
        [InlineData(7, 3, 3, 7)]
        [InlineData(5, 6, 6, 5)]
        public void AddBox_BoxFits_AddsBox(double boxWidth, double boxDepth, double palletWidth, double palletDepth)
        {
            var box = GetBox(width: boxWidth, depth: boxDepth);
            var pallet = GetPallet(width: palletWidth, depth: palletDepth);

            pallet.AddBox(box);

            Assert.Contains(box, pallet.GetBoxes());
        }
        [Fact]
        public void AddBox_Null_ThrowsArgumentNullException()
        {
            var box = GetNullBox();
            var pallet = GetPallet();

            Assert.Throws<ArgumentNullException>(() => pallet.AddBox(box));
        }
        [Theory]
        [InlineData(10, 10)]
        [InlineData(3, 15)]
        public void AddBox_BoxTooBig_ThrowsInvalidOperationException(double boxWidth, double boxDepth)
        {
            var box = GetBox(width: boxWidth, depth: boxDepth);
            var pallet = GetPallet();

            Assert.Throws<InvalidOperationException>(() => pallet.AddBox(box));
        }

        #endregion

        #region GetWeight

        [Fact]
        public void GetWeight_EmptyBoxList_ReturnsPalletWeight()
        {
            var pallet = GetPallet();

            var actual = pallet.GetWeight();

            Assert.Equal(DEFAULT_PALLET_WEIGHT, actual);
        }
        [Theory]
        [InlineData(3, 4)]
        [InlineData(5.5, 7.65)]
        public void GetWeight_PalletWithBoxes_ReturnsSumWeight(double firstWeight, double secondWeight)
        {
            var box1 = GetBox(weight: firstWeight);
            var box2 = GetBox(weight: secondWeight);
            var pallet = GetPallet();
            var expected = firstWeight + secondWeight + DEFAULT_PALLET_WEIGHT;
            pallet.AddBox(box1);
            pallet.AddBox(box2);

            var actual = pallet.GetWeight();

            Assert.Equal(expected, actual, 3);
        }
        [Fact]
        public void GetWeight_TooLargeWeight_ThrowsOverflowException()
        {
            var box1 = GetOverflowedWeightBox();
            var box2 = GetOverflowedWeightBox();
            var pallet = GetPallet();
            pallet.AddBox(box1);
            pallet.AddBox(box2);

            Assert.Throws<OverflowException>(() => pallet.GetWeight());
        }

        #endregion

        #region GetVolume

        [Theory]
        [InlineData(2, 2, 2, 8)]
        [InlineData(4, 4, 4, 64)]
        [InlineData(2.5, 2.5, 2.5, 15.625)]
        public void GetVolume_EmptyBoxList_ReturnsOwnVolume(double width, double height, double depth, double expected)
        {
            var pallet = GetPallet(width: width, height: height, depth: depth);

            var actual = pallet.GetVolume();

            Assert.Equal(expected, actual, 5);
        }
        [Fact]
        public void GetVolume_PalletWithBoxes_ReturnsSumVolume()
        {
            var expected = 3;
            var box1 = GetBox();
            var box2 = GetBox();
            var pallet = GetPallet();
            pallet.AddBox(box1);
            pallet.AddBox(box2);

            var actual = pallet.GetVolume();

            Assert.Equal(expected, actual, 3);
        }
        [Fact]
        public void GetVolume_TooLargeVolume_ThrowsOverflowException()
        {
            var box1 = GetOverflowedSizeBox();
            var box2 = GetOverflowedSizeBox();
            var pallet = GetOverflowedSizePallet();
            pallet.AddBox(box1);
            pallet.AddBox(box2);

            Assert.Throws<OverflowException>(() => pallet.GetVolume());
        }

        #endregion

        #region GetExpireDate

        [Fact]
        public void GetExpireDate_PalletWithBoxes_ReturnsEarliestDate()
        {
            var expected = DateTime.MinValue.AddDays(DEFAULT_BOX_EXPIRATION_OFFSET_IN_DAYS);
            var box1 = GetBox();
            var box2 = GetBoxWithDate(DateTime.MinValue);
            var pallet = GetPallet();
            pallet.AddBox(box1);
            pallet.AddBox(box2);

            var actual = pallet.GetExpireDate();

            Assert.Equal(expected, actual);
        }
        [Fact]
        public void GetExpireDate_EmptyPallet_ThrowsInvalidOperationException()
        {
            var pallet = GetPallet();

            Assert.Throws<InvalidOperationException>(() => pallet.GetExpireDate());
        }

        #endregion

        #region GetBoxes

        [Fact]
        public void GetBoxes_ReturnsReadOnlyCollection()
        {
            var pallet = GetPallet();

            var actual = pallet.GetBoxes();

            Assert.IsType<ReadOnlyCollection<Box>>(actual);
        }
        [Fact]
        public void GetBoxes_ReturnsNotNullCollection()
        {
            var pallet = GetPallet();

            var actual = pallet.GetBoxes();

            Assert.NotNull(actual);
        }
        [Fact]
        public void GetBoxes_CollectionContainsAddedBoxes()
        {
            var box = GetBox();
            var pallet = GetPallet();
            pallet.AddBox(box);

            var actual = pallet.GetBoxes();

            Assert.Contains(box, actual);
        }

        #endregion

        #region Private
        private const int DEFAULT_PALLET_WEIGHT = 30;
        private const int DEFAULT_BOX_EXPIRATION_OFFSET_IN_DAYS = 100;

        private Pallet GetPallet(double width = 1, double height = 1, double depth = 1)
        {
            return new Pallet(width, height, depth);
        }

        private Box GetBox(double width = 1, double height = 1, double depth = 1, double weight = 1)
        {
            return GetBoxWithDate(GetDate(), width, height, depth, weight);
        }
        private Box GetBoxWithDate(DateTime date, double width = 1, double height = 1, double depth = 1, double weight = 1)
        {
            return new Box(width, height, depth, weight, date);
        }
        private DateTime GetDate()
        {
            return DateTime.MaxValue.AddDays(-DEFAULT_BOX_EXPIRATION_OFFSET_IN_DAYS - 1);
        }
        private Box GetOverflowedWeightBox() => GetBox(weight: double.MaxValue);
        private Box GetOverflowedSizeBox() => GetBox(width: double.MaxValue, height: double.MaxValue, depth: double.MaxValue);
        private Pallet GetOverflowedSizePallet() => GetPallet(width: double.MaxValue, height: double.MaxValue, depth: double.MaxValue);
        private Box GetNullBox() => null;
        #endregion
    }
}
