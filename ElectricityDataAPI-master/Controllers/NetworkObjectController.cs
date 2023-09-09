using Girteka_task.data;
using Girteka_task.data.entities;
using Girteka_task.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;
using static Girteka_task.Data.DTOS.NetworkObjectDataDTOs;

namespace Girteka_task.Controllers
{
    [ApiController]
    [Route("api")]
    public class NetworkObjectController : ControllerBase
    {
        private readonly INetworkObjectRepository _NetworkObjectRepository;

        public NetworkObjectController(INetworkObjectRepository networkObjectRepository)
        {
            _NetworkObjectRepository = networkObjectRepository;
        }

        [HttpGet]
        [Route("aggregatedData")]
        public async Task<ActionResult<IEnumerable<NetworkObjectData>>> GetMany()
        {
            try
            {
                var objects = await _NetworkObjectRepository.GetAsync();
                Log.Information("Network object data retrieved successfully => {@objects}", objects);
                return Ok(objects);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while retrieving network object data");
                return StatusCode(500, "An error occurred while retrieving network object data");
            }
        }

        [HttpPost]
        [Route("RetrieveData")]
        public async Task<IActionResult> PopulateDatabase([FromForm]PostNetObjDTO dto)
        {
            try
            {
                //input data validation
                Uri uriResult;
                bool isUrlValid = Uri.TryCreate(dto.DataURL, UriKind.Absolute, out uriResult) &&
                                 (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                if (!isUrlValid)
                {
                    Log.Error("URL format incorrect => {@dto.DataURL}", dto.DataURL);
                    return BadRequest("URL format incorrect");
                }
                if(!DateTime.TryParse(dto.StartDate,out DateTime startDate))
                {
                    Log.Error("Start date field format incorrect => {@dto.StartDate}", dto.StartDate);
                    return BadRequest("Start date field format incorrect"); 
                }
                if (!DateTime.TryParse(dto.EndDate, out DateTime endDate))
                {
                    Log.Error("End date field format incorrect => {@dto.EndDate}", dto.EndDate);
                    return BadRequest("End date field format incorrect");
                }
                if (!Enum.TryParse(dto.TypeFilter, out obj_type parsedEnumValue))
                {
                    Log.Error("Type filter field value incorrect => {@dto.TypeFilter}", dto.TypeFilter);
                    return BadRequest("Type filter field value incorrect");
                }
                Func<NetworkObjectData, string>? fieldSelector = _NetworkObjectRepository.GetGroupingDelegate<string>(dto.GroupingField);
                if (fieldSelector==null)
                {
                    Log.Error("Grouping field value incorrect => {@dto.GroupingField}", dto.GroupingField);
                    return BadRequest("Grouping field value incorrect");
                }

                object result = await _NetworkObjectRepository.PopulateDatabaseAsync(dto.DataURL, startDate, endDate, parsedEnumValue, fieldSelector);

                // If the result is a string, it means there was an error
                if (result is string errorMessage)
                {
                    Log.Error("An error occured while accessing the URL => {@errorMessage}", errorMessage);
                    return BadRequest(errorMessage); // Return a 400 Bad Request with the error message
                }

                // If the result is a List<NetworkObjectData>, return it
                if (result is List<NetworkObjectData> groupedData)
                {
                    Log.Information("Object data saved in database successfuly => {@groupedData}", groupedData);
                    Console.WriteLine("Duomenu kiekis: {0}",groupedData.Count);
                    return Ok(groupedData); // Return a 200 OK with the aggregated list
                }

                // If the result is neither a string nor a list, handle accordingly
                Log.Error("Request returned status code 500");
                return StatusCode(500); // Return a 500 Internal Server Error
            }
            catch (Exception ex)
            {
                Log.Information("Request returned status code 500 with exception => {@ex.Message}", ex.Message);
                return StatusCode(500, ex.Message); 
            }
        }


    }
}
