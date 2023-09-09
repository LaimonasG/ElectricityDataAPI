using Girteka_task.Controllers;
using Girteka_task.data;
using Girteka_task.data.entities;
using Girteka_task.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Girteka_task.Data.DTOS.NetworkObjectDataDTOs;

namespace Girteka_task_tests
{
    public class NetworkObjectRepositoryTests
    {
        private readonly Context _Context;
        private readonly MockData _MockData;
        private readonly Mock<INetworkObjectRepository> _NetworkObjectRepositoryMock;
        private readonly NetworkObjectRepository _NetworkObjectRepository;

        public NetworkObjectRepositoryTests()
        {
            _MockData = new MockData();
            _NetworkObjectRepositoryMock = new Mock<INetworkObjectRepository>();
            DbContextOptionsBuilder<Context> DbOptions =
                new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            _NetworkObjectRepository = new NetworkObjectRepository(_Context);
            _Context = new Context(DbOptions.Options);
        }

        [Fact]
        public async Task GetAsync_WithExistingObjects_ReturnsObjects()
        {
            // Arrange
            _Context.Objects.AddRange(_MockData.ParsedList);
            await _Context.SaveChangesAsync();

            var repo = new NetworkObjectRepository(_Context);

            // Act
            IEnumerable<NetworkObjectData> result = await repo.GetAsync();

            _Context.Dispose();

            // Assert
            Assert.Equal(_MockData.ParsedList.Count, result.Count());
        }

        //[Fact]
        //public async Task PopulateDatabaseAsync_WithValidData_ReturnsObjectList()
        //{
        //    // Arrange
        //    var validDto = new PostNetObjDTO(
        //      DataURL: "https://data.gov.lt/dataset/1975/download/10746/2020-06.csv",
        //      StartDate: "2020-09-01",
        //      EndDate: "2020-09-30",
        //      GroupingField: "Network",
        //      TypeFilter: "Butas"
        //  );
        //    Func<NetworkObjectData, string> fieldFunc = (data) =>
        //    {
        //        return data.Network.ToString();
        //    };

        //    _NetworkObjectRepositoryMock.Setup(repo => repo.FetchDataAsync(validDto.DataURL))
        //                        .Returns(Task.FromResult(_MockData.UnparsedContent));

        //    _NetworkObjectRepositoryMock.Setup(repo => repo.ParseData(_MockData.UnparsedContent,
        //        DateTime.Parse(validDto.StartDate), DateTime.Parse(validDto.EndDate)))
        //                        .Returns(_MockData.ParsedList);

        //    _NetworkObjectRepositoryMock.Setup(repo => repo.FilterData(_MockData.ParsedList,
        //        Enum.Parse<obj_type>(validDto.TypeFilter)))
        //                        .Returns(_MockData.FilteredList);

        //    _NetworkObjectRepositoryMock.Setup(repo => repo.GroupByField(_MockData.FilteredList,
        //        fieldFunc)).Returns(_MockData.GroupedList);

        //    // Act
        //    object objects=  _NetworkObjectRepositoryMock.Object.PopulateDatabaseAsync(validDto.DataURL,
        //        DateTime.Parse(validDto.StartDate), DateTime.Parse(validDto.EndDate),
        //        Enum.Parse<obj_type>(validDto.TypeFilter), fieldFunc);

        //    // Assert
        //    if (objects is List<NetworkObjectData> groupedData)
        //    {
        //        Assert.Equal(_MockData.GroupedList.Count, groupedData.Count());
        //    }
        //    if (objects is string error)
        //    {
        //        Assert.Equal("error", error);
        //    }
        //    //  Assert.Equal();
        //}

        [Fact]
        public void ParseData_WithNoContentToParse_ReturnsEmptyList()
        {
            // Arrange
            string content="";
            DateTime startDate = new DateTime(2020, 09, 01);
            DateTime endDate = new DateTime(2020, 09, 30);

            // Act
            var result = _NetworkObjectRepository.ParseData(content, startDate, endDate);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ParseData_WithContentToParse_ReturnsObjectList()
        {
            // Arrange
            DateTime startDate = new DateTime(2019, 05, 01);
            DateTime endDate = new DateTime(2022, 09, 30);

            // Act
            var result = _NetworkObjectRepository.ParseData(_MockData.UnparsedContent, startDate, endDate);

            // Assert
            Assert.Equal(_MockData.GroupedList.Count, result.Count);
        }

        [Fact]
        public void ParseData_WithValidDateRange_ReturnsFilteredObjects()
        {
            // Arrange
            DateTime startDate = new DateTime(2020, 05, 01);
            DateTime endDate = new DateTime(2021, 09, 30);

            // Act
            var result = _NetworkObjectRepository.ParseData(_MockData.UnparsedContent, startDate, endDate);

            // Assert
            foreach (var item in result)
            {
                Assert.True(item.PL_T >= startDate && item.PL_T <= endDate);
            }
        }

        [Fact]
        public void ParseLine_WithValidData_ReturnsParsedNetworkObjectData()
        {
            // Arrange
            string line = "Klaipėdos regiono tinklas,Namas,G,282408,0.0477,2020-09-30 00:00:00,0.0";

            // Act
            NetworkObjectData result = _NetworkObjectRepository.ParseLine(line);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Klaipėdos regiono tinklas", result.Network);
            Assert.Equal(obj_type.Namas, result.Object_Type);
            Assert.Equal(obj_gv_type.G, result.Object_GV_Type);
            Assert.Equal(282408, result.Object_Number);
            Assert.Equal(0.0477, result.Pplus);
            Assert.Equal(new DateTime(2020, 09, 30, 0, 0, 0), result.PL_T);
            Assert.Equal(0.0, result.Pminus);
        }

        [Fact]
        public void ParsePValues_WithoutPplus_ReturnsParsedPvalues()
        {
            // Arrange
            string line = "Klaipėdos regiono tinklas,Namas,G,282408,2020-09-30 00:00:00,0.0";
            string[] data = line.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            double? pPlusValue;
            double? pMinusValue;
            DateTime PLT;
            double pMinusActual = 0.0;
            DateTime PLTActual = DateTime.Parse("2020-09-30 00:00:00");

            // Act
            _NetworkObjectRepository.ParsePValues(data, out pPlusValue, out pMinusValue, out PLT);

            // Assert
            Assert.Null(pPlusValue);
            Assert.Equal(pMinusActual, pMinusValue);
            Assert.Equal(PLTActual, PLT);
        }

        [Fact]
        public void ParsePValues_WithoutPminus_ReturnsParsedPvalues()
        {
            // Arrange
            string line = "Klaipėdos regiono tinklas,Namas,G,282408,2.021,2020-09-30 00:00:00";
            string[] data = line.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            double? pPlusValue;
            double? pMinusValue;
            DateTime PLT;
            double pPlusActual = 2.021;
            DateTime PLTActual = DateTime.Parse("2020-09-30 00:00:00");

            // Act
            _NetworkObjectRepository.ParsePValues(data, out pPlusValue, out pMinusValue, out PLT);

            // Assert
            Assert.Null(pMinusValue);
            Assert.Equal(pPlusActual, pPlusValue);
            Assert.Equal(PLTActual, PLT);
        }

        [Fact]
        public void ParsePValues_WithoutPminusAndPplus_ReturnsParsedPvalues()
        {
            // Arrange
            string line = "Klaipėdos regiono tinklas,Namas,G,282408,2020-09-30 00:00:00";
            string[] data = line.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            double? pPlusValue;
            double? pMinusValue;
            DateTime PLT;
            DateTime PLTActual = DateTime.Parse("2020-09-30 00:00:00");

            // Act
            _NetworkObjectRepository.ParsePValues(data, out pPlusValue, out pMinusValue, out PLT);

            // Assert
            Assert.Null(pMinusValue);
            Assert.Null(pPlusValue);
            Assert.Equal(PLTActual, PLT);
        }

        [Fact]
        public void IsWithinDateRange_DateInsideRange_ReturnsTrue()
        {
            // Arrange
            DateTime date = DateTime.Parse("2020-09-15 00:00:00");
            DateTime start = DateTime.Parse("2020-09-01 00:00:00");
            DateTime end = DateTime.Parse("2020-09-30 00:00:00");

            // Act
            bool result= _NetworkObjectRepository.IsWithinDateRange(date,start,end);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsWithinDateRange_DateOutsideRange_ReturnsFalse()
        {
            // Arrange
            DateTime date = DateTime.Parse("2020-10-31 00:00:00");
            DateTime start = DateTime.Parse("2020-09-01 00:00:00");
            DateTime end = DateTime.Parse("2020-09-30 00:00:00");

            // Act
            bool result = _NetworkObjectRepository.IsWithinDateRange(date, start, end);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GroupByField_ValidData_ReturnsGroupedData()
        {
            // Arrange
            Func<NetworkObjectData, string> fieldSelector = data => data.Network;

            // Act
            var result = _NetworkObjectRepository.GroupByField(_MockData.ParsedList, fieldSelector);

            // Assert           
            Assert.Equal(_MockData.GroupedList.Count, result.Count);
            
            for (int i=0;i<result.Count;i++)
            {
                Assert.Equal(_MockData.GroupedList[i].Network, result[i].Network);
            }
        }

        [Fact]
        public void GroupByField_NullData_ReturnsEmptyList()
        {
            // Arrange
            List<NetworkObjectData> data = null;
            Func<NetworkObjectData, string> fieldSelector = data => data.Network;

            // Act
            var result = _NetworkObjectRepository.GroupByField(data, fieldSelector);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void FilterData_NullData_ReturnsEmptyList()
        {
            // Arrange
            List<NetworkObjectData> data = null;
            obj_type type = obj_type.Butas;

            // Act
            var result = _NetworkObjectRepository.FilterData(data, type);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void FilterData_WithData_ReturnsFilteredList()
        {
            // Arrange
            obj_type type = obj_type.Butas;

            // Act
            var result = _NetworkObjectRepository.FilterData(_MockData.ParsedList, type);

            // Assert
            foreach (var item in result)
            {
                Assert.True(item.Object_Type==type);
            }
        }

        [Fact]
        public void GetGroupingDelegate_WithValidFieldName_ReturnsDelegate()
        {
            // Arrange
            string fieldName = "Network";
            Func<NetworkObjectData, string> fieldFunc = (data) => {
                return data.Network.ToString();
            };

            // Act
            var result = _NetworkObjectRepository.GetGroupingDelegate<string>(fieldName);

            // Assert
            var data = new NetworkObjectData { Network="Vilnius"};
            var fieldFuncResult = fieldFunc(data);
            var resultFuncResult = result(data);

            Assert.Equal(fieldFuncResult, resultFuncResult);
        }

        [Fact]
        public void GetGroupingDelegate_WithInvalidFieldName_ReturnsNull()
        {
            // Arrange
            string fieldName = "Badname";
            Func<NetworkObjectData, string> fieldFunc = (data) => {
                return data.Network.ToString();
            };

            // Act
            var result = _NetworkObjectRepository.GetGroupingDelegate<string>(fieldName);

            // Assert
            Assert.Null(result);
        }
    }
}
