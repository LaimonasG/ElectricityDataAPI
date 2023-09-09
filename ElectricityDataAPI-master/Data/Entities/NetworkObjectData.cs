namespace Girteka_task.data.entities
{
    public class NetworkObjectData
    {
        public int Id { get; set; }

        public string Network { get; set; }

        public obj_type Object_Type { get; set; }

        public obj_gv_type Object_GV_Type { get; set; }

        public int Object_Number { get; set; }

        public double? Pplus { get; set; }

        public DateTime PL_T { get; set; }

        public double? Pminus { get; set; }
    }

    public enum obj_type
    {
       Namas,
       Butas,
       Sodas
    }

    public enum obj_gv_type
    {
        G,
        NeGV,
        N
    }
}
