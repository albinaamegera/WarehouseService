using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseTestService.Data;

namespace WarehouseTestService.Helpers
{
    public class DBDataGenerator
    {
        private readonly int _palletMinCount = 100;
        private readonly int _palletMaxCount = 101;
        private readonly int _boxMinCount = 5;
        private readonly int _boxMaxCount = 10;
        private readonly int _minPropertyValue = 4;
        private readonly int _maxPropertyValue = 15;
        private readonly double _boxPropertyValueOffset = 0.1;
        private readonly int _minProductionDateInYears = 2024;
        private readonly int _maxProductionDateInYears = 2025;
        private readonly int _offsetFactor = 0;
        private readonly int _negativeValueFactor = 0;
        private readonly int _critFactor = 0;

        WarehouseContext _context;
        Random _randomizer;
        public DBDataGenerator(WarehouseContext contenxt)
        {
            _context = contenxt;
            _randomizer = new();
        }
        /// <summary>
        /// удаляет существующую базу данных,
        /// создает новую и заполняет ее случайными значениями
        /// </summary>
        /// <returns></returns>
        public async Task Generate()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();

            var palletCount = GetPalletCount();

            for (int i = 0; i < palletCount; i++)
            {
                var pallet = new PalletModel()
                {
                    Width = GetPalletPropertyValue(),
                    Height = GetPalletPropertyValue(),
                    Depth = GetPalletPropertyValue()
                };

                _context.Pallets.Add(pallet);

                await _context.SaveChangesAsync();

                var palletId = pallet.Id;
                var boxesCount = GetBoxCount();

                for (int j = 0; j < boxesCount; j++)
                {
                    var box = new BoxModel()
                    {
                        PalletId = palletId,
                        Width = GetBoxPropertyValue(),
                        Height = GetBoxPropertyValue(),
                        Depth = GetBoxPropertyValue(),
                        Weight = GetBoxPropertyValue(),
                        ProductionDate = GetRandomDate()
                    };

                    _context.Boxes.Add(box);
                }
            }

            await _context.SaveChangesAsync();
        }
        private int GetPalletCount() => _randomizer.Next(_palletMinCount, _palletMaxCount);
        private int GetBoxCount() => _randomizer.Next(_boxMinCount, _boxMaxCount);
        private double GetPalletPropertyValue()
        {
            return CheckCritFactor() ? GetExtremeDoubleValue() : GetRandomDouble(_minPropertyValue, _maxPropertyValue);
        }
        private double GetBoxPropertyValue()
        {
            var random = GetRandomDouble(_minPropertyValue, _maxPropertyValue);
            var offsetChance = _randomizer.Next(0, 100) < _offsetFactor;
            var sighChance = _randomizer.Next(0, 100) < 50;
            var randomWithOffset = sighChance ? random - _boxPropertyValueOffset : random + _boxPropertyValueOffset;
            return CheckCritFactor() ? GetExtremeDoubleValue() : offsetChance ? randomWithOffset : random;
        }

        private bool CheckCritFactor()
        {
            var chance = _randomizer.Next(0, 100);
            
            return chance < _critFactor;
        }
        private int GetNegativeMultiplyer()
        {
            var chance = _randomizer.Next(0, 100);

            return chance < _negativeValueFactor ? -1 : 1;
        }
        private double GetRandomDouble(int min, int max)
        {
            var number = _randomizer.Next(min, max);
            return number * GetNegativeMultiplyer();
        }
        private double GetExtremeDoubleValue()
        {
            var chance = _randomizer.Next(0, 100);
            return chance < 50 ? double.MinValue : double.MaxValue;
        }
        private DateTime GetRandomDate()
        {
            if (CheckCritFactor())
            {
                var chance = _randomizer.Next(0, 100);
                return chance < 50 ? DateTime.MinValue : DateTime.MaxValue;
            }
            var day = 15;
            var month = _randomizer.Next(1, 13);
            var year = _randomizer.Next(_minProductionDateInYears, _maxProductionDateInYears);
            return new DateTime(year, month, day);
        }
    }
}
