namespace Girteka_task.Data.DTOS
{
    public class NetworkObjectDataDTOs
    {
        public record PostNetObjDTO(string DataURL, string StartDate,string EndDate, string TypeFilter,string GroupingField);
    }
}
