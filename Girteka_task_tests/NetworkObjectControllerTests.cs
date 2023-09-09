using Girteka_task;
using Girteka_task.Controllers;
using Girteka_task.data.entities;
using Girteka_task.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using static Girteka_task.Data.DTOS.NetworkObjectDataDTOs;
using Assert = Xunit.Assert;

namespace Girteka_task_tests
{
    public class NetworkObjectControllerTests
    {
        private readonly Mock<INetworkObjectRepository> _NetworkObjectRepositoryMock;
        private readonly NetworkObjectController _Controller;
        private readonly MockData _MockObjectList;

        public NetworkObjectControllerTests()
        {
            _NetworkObjectRepositoryMock = new Mock<INetworkObjectRepository>();
            _Controller = new NetworkObjectController(_NetworkObjectRepositoryMock.Object);
            _MockObjectList= new MockData();
        }

        //[Fact]
        //public async Task PopulateDatabase_WithValidParams_ReturnsAddedObjects()
        //{
        //    // Arrange
        //    var validDto = new PostNetObjDTO(
        //        DataURL: "https://data.gov.lt/dataset/1975/download/10746/2020-06.csv",
        //        StartDate: "2020-09-01",
        //        EndDate: "2020-09-30",
        //        GroupingField: "Network",
        //        TypeFilter: "Butas"
        //    );

        //    Func<NetworkObjectData, string> fieldFunc = (data) => {
        //        return data.Network.ToString();
        //    };
        //    _NetworkObjectRepositoryMock.Setup(repo => repo.GetGroupingDelegate<string>(validDto.GroupingField))
        //                        .Returns(fieldFunc);

        //    _NetworkObjectRepositoryMock.Setup(repo => repo.PopulateDatabaseAsync(
        //    validDto.DataURL, DateTime.Parse(validDto.StartDate), DateTime.Parse(validDto.EndDate),
        //    Enum.Parse<obj_type>(validDto.TypeFilter), fieldFunc))
        //    .ReturnsAsync(_MockObjectList.ParsedList);

        //    // Act
        //    var result = await _Controller.PopulateDatabase(validDto);

        //    // Assert
        //    var actionResult = Assert.IsType<OkObjectResult>(result);
        //    Assert.IsType<List<NetworkObjectData>>(actionResult.Value);
        //}

        [Fact]
        public async Task PopulateDatabase_WithIncorrectDateFormat_ReturnsExpectedErrorMessage()
        {
            // Arrange
            var validDto = new PostNetObjDTO(
                DataURL: "https://data.gov.lt/dataset/1975/download/10746/2020-06.csv",
                StartDate: "BadFormat", // incorrect date format
                EndDate: "-2020-09-30", // incorrect date format
                GroupingField: "Network",
                TypeFilter: "Butas"
            );
            var expectedErrorMessage = "Start date field format incorrect";
            DateTime.TryParse(validDto.StartDate, out DateTime startDate);
            DateTime.TryParse(validDto.EndDate, out DateTime endDate);


            Func<NetworkObjectData, string> fieldFunc = (data) => {
                return data.Network.ToString();
            };

            _NetworkObjectRepositoryMock.Setup(repo => repo.GetGroupingDelegate<string>(validDto.GroupingField))
                                .Returns(fieldFunc);

            _NetworkObjectRepositoryMock.Setup(repo => repo.PopulateDatabaseAsync(
            validDto.DataURL, startDate, endDate,Enum.Parse<obj_type>(validDto.TypeFilter), fieldFunc))
            .ReturnsAsync(_MockObjectList.ParsedList);

            // Act
            var result = await _Controller.PopulateDatabase(validDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorMessage = Assert.IsType<string>(badRequestResult.Value);

            Assert.Equal(expectedErrorMessage, errorMessage);
        }

        [Fact]
        public async Task PopulateDatabase_WithIncorrectTypeFilter_ReturnsErrorMessage()
        {
            // Arrange
            var validDto = new PostNetObjDTO(
                DataURL: "https://data.gov.lt/dataset/1975/download/10746/2020-06.csv",
                StartDate: "2020-09-01",
                EndDate: "2020-09-30",
                GroupingField: "Network",
                TypeFilter: "Garažas" //incorrect type
            );

            var expectedErrorMessage = "Type filter field value incorrect";
            Enum.TryParse<obj_type>(validDto.TypeFilter, out obj_type typeFilterParsed);

            Func<NetworkObjectData, string> fieldFunc = (data) => {
                return data.Network.ToString();
            };

            _NetworkObjectRepositoryMock.Setup(repo => repo.GetGroupingDelegate<string>(validDto.GroupingField))
                                .Returns(fieldFunc);

            _NetworkObjectRepositoryMock.Setup(repo => repo.PopulateDatabaseAsync(
            validDto.DataURL, DateTime.Parse(validDto.StartDate), DateTime.Parse(validDto.EndDate), typeFilterParsed, fieldFunc))
            .ReturnsAsync(_MockObjectList.ParsedList);

            // Act
            var result = await _Controller.PopulateDatabase(validDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorMessage = Assert.IsType<string>(badRequestResult.Value);

            Assert.Equal(expectedErrorMessage, errorMessage);
        }

        [Fact]
        public async Task PopulateDatabase_WithIncorrectURL_ReturnsErrorMessage()
        {
            // Arrange
            var validDto = new PostNetObjDTO(
                DataURL: "//data.gov.lt/dataset/1975/download/10746/2020-06.csv", //incorrect URL format
                StartDate: "2020-09-01",
                EndDate: "2020-09-30",
                GroupingField: "Network",
                TypeFilter: "Butas" 
            );

            var expectedErrorMessage = "URL format incorrect";

            Func<NetworkObjectData, string> fieldFunc = (data) => {
                return data.Network.ToString();
            };

            _NetworkObjectRepositoryMock.Setup(repo => repo.GetGroupingDelegate<string>(validDto.GroupingField))
                                .Returns(fieldFunc);

            _NetworkObjectRepositoryMock.Setup(repo => repo.PopulateDatabaseAsync(
            validDto.DataURL, DateTime.Parse(validDto.StartDate), DateTime.Parse(validDto.EndDate),
            Enum.Parse<obj_type>(validDto.TypeFilter), fieldFunc)).ReturnsAsync(_MockObjectList.ParsedList);

            // Act
            var result = await _Controller.PopulateDatabase(validDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorMessage = Assert.IsType<string>(badRequestResult.Value);

            Assert.Equal(expectedErrorMessage, errorMessage);
        }
    }
}