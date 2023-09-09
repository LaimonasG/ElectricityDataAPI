using Girteka_task.data;
using Girteka_task.data.entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Girteka_task.Data.Repositories
{
    public interface INetworkObjectRepository
    {
        Task<IReadOnlyList<NetworkObjectData>> GetAsync();
        Task<string> FetchDataAsync(string dataUrl);
        Task<object> PopulateDatabaseAsync<T>(string dataUrl, DateTime startDate, DateTime endDate, obj_type typeFilter,
            Func<NetworkObjectData, T> fieldSelector);
        List<NetworkObjectData> ParseData(string content, DateTime startDate, DateTime endDate);
        NetworkObjectData ParseLine(string line);
        void ParsePValues(string[] data, out double? pPlusValue, out double? pMinusValue, out DateTime PLT);
        bool IsWithinDateRange(DateTime date, DateTime startDate, DateTime endDate);
        List<NetworkObjectData> FilterData(List<NetworkObjectData> data, obj_type type);
        List<NetworkObjectData> GroupByField<T>(List<NetworkObjectData> data, Func<NetworkObjectData, T> fieldSelector);
        Func<NetworkObjectData, T> GetGroupingDelegate<T>(string fieldName);
    }

    public class NetworkObjectRepository : INetworkObjectRepository
    {
        private readonly Context _DbContext;

        public NetworkObjectRepository(Context context)
        {
            _DbContext = context;
        }

        public async Task<IReadOnlyList<NetworkObjectData>> GetAsync()
        {
            return await _DbContext.Objects.ToListAsync();
        }

        public async Task<object> PopulateDatabaseAsync<T>(string dataUrl, DateTime startDate, DateTime endDate, obj_type typeFilter, Func<NetworkObjectData, T> fieldSelector)
        {
            try
            {
                string content = await FetchDataAsync(dataUrl);
                var parsedData = ParseData(content, startDate, endDate);
                List<NetworkObjectData> filteredData = FilterData(parsedData, typeFilter);
                List<NetworkObjectData> groupedData = GroupByField(filteredData, fieldSelector);
                _DbContext.Objects.AddRange(groupedData);
                await _DbContext.SaveChangesAsync();
                return groupedData;
            }
            catch (HttpRequestException ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> FetchDataAsync(string dataUrl)
        {

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(dataUrl);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }

        }

        public List<NetworkObjectData> ParseData(string content, DateTime startDate, DateTime endDate)
        {
            string[] lines = content.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();
            var result = new List<NetworkObjectData>();
            foreach (var line in lines)
            {
                NetworkObjectData entity = ParseLine(line);
                if (IsWithinDateRange(entity.PL_T, startDate, endDate))
                {
                    result.Add(entity);
                }
            }
            return result;
        }

        public NetworkObjectData ParseLine(string line)
        {
            string[] data = line.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            double? pPlusValue;
            double? pMinusValue;
            DateTime PLT;

            if (data.Count() < 5 || data.Count() > 7)
                return null;

            ParsePValues(data, out pPlusValue, out pMinusValue, out PLT);

            string formattedGvTypeString = data[2].Replace(" ", "");

            return new NetworkObjectData
            {
                Network = data[0],
                Object_Type = Enum.Parse<obj_type>(data[1]),
                Object_GV_Type = Enum.Parse<obj_gv_type>(formattedGvTypeString),
                Object_Number = int.Parse(data[3]),
                Pplus = pPlusValue,
                PL_T = PLT,
                Pminus = pMinusValue
            };
        }

        public void ParsePValues(string[] data, out double? pPlusValue, out double? pMinusValue, out DateTime PLT)
        {
            pPlusValue = null;
            pMinusValue = null;
            PLT = DateTime.MinValue;

            if (data.Length == 5)  //Pplus and Pminus are null
            {
                PLT = DateTime.Parse(data[4]);
                return;
            }
            if (data.Length == 6)  //Pplus or Pminus is null
            {
                if (!double.TryParse(data[4], out _))  //Pplus is null
                {
                    pMinusValue = double.Parse(data[5]);
                    PLT = DateTime.Parse(data[4]);
                }
                else //Pminus is null
                {
                    pPlusValue = double.Parse(data[4]);
                    PLT = DateTime.Parse(data[5]);
                }
                return;
            }
            if (data.Length == 7)  //All values are not null
            {
                pMinusValue = double.Parse(data[6]);
                pPlusValue = double.Parse(data[4]);
                PLT = DateTime.Parse(data[5]);
            }
        }

        public bool IsWithinDateRange(DateTime date, DateTime startDate, DateTime endDate)
        {
            return (date <= endDate && date >= startDate);
        }

        public List<NetworkObjectData> FilterData(List<NetworkObjectData> data, obj_type type)
        {
            if (data == null)
                return new List<NetworkObjectData> { };

            var result = new List<NetworkObjectData>();
            foreach (var obj in data)
            {
                if (obj.Object_Type == type)
                    result.Add(obj);
            }
            return result;
        }

        public List<NetworkObjectData> GroupByField<T>(List<NetworkObjectData> data, Func<NetworkObjectData, T> fieldSelector)
        {
            if (data == null)
                return new List<NetworkObjectData> { };

            var result = data.GroupBy(fieldSelector)
                             .SelectMany(group => group)
                             .ToList();
            return result;
        }

        public Func<NetworkObjectData, T> GetGroupingDelegate<T>(string fieldName)
        {
            switch (fieldName)
            {
                case "Network":
                    return item => (T)(object)item.Network;
                case "Object Type":
                    return item => (T)(object)item.Object_Type;
                case "Object GV Type":
                    return item => (T)(object)item.Object_GV_Type;
                case "Object Number":
                    return item => (T)(object)item.Object_Number;
                case "P+":
                    return item => (T)(object)item.Pplus;
                case "PL_T":
                    return item => (T)(object)item.PL_T;
                case "P-":
                    return item => (T)(object)item.Pminus;
                default:
                    return null;
            }
        }
    }
}
