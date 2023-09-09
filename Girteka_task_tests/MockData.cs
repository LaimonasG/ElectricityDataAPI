using Girteka_task.data.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Girteka_task_tests
{
    public class MockData
    {
        public List<NetworkObjectData> ParsedList { get; set; }
        public List<NetworkObjectData> FilteredList { get; set; }
        public List<NetworkObjectData> GroupedList { get; set; }
        public string UnparsedContent { get; set; }

        public MockData()
        {
            ParsedList = new List<NetworkObjectData>
            {
                new NetworkObjectData
                {
                    Id = new Random().Next(100),
                    Network = "Kėdainių apskritis",
                    Object_Type = obj_type.Butas,
                    Object_GV_Type = obj_gv_type.G,
                    Object_Number = new Random().Next(1, 100),
                    Pplus = null,
                    PL_T = new DateTime(2020,09,01),
                    Pminus = new Random().NextDouble()
                },
                new NetworkObjectData
                {
                    Id = new Random().Next(100),
                    Network = "Kauno apskritis",
                    Object_Type = obj_type.Butas,
                    Object_GV_Type = obj_gv_type.NeGV,
                    Object_Number = new Random().Next(1, 100),
                    Pplus = null,
                    PL_T = new DateTime(2020,09,20),
                    Pminus = null
                },
                 new NetworkObjectData
                {
                    Id = new Random().Next(100),
                    Network = "Vilniaus apskritis",
                    Object_Type = obj_type.Butas,
                    Object_GV_Type = obj_gv_type.G,
                    Object_Number = new Random().Next(1, 100),
                    Pplus = new Random().NextDouble(),
                    PL_T = new DateTime(2020,09,21),
                    Pminus = new Random().NextDouble()
                },
                  new NetworkObjectData
                {
                    Id = new Random().Next(100),
                    Network = "Kauno apskritis",
                    Object_Type = obj_type.Butas,
                    Object_GV_Type = obj_gv_type.N,
                    Object_Number = new Random().Next(1, 100),
                    Pplus = new Random().NextDouble(),
                    PL_T = new DateTime(2020, 09, 02),
                    Pminus = null
                },
                   new NetworkObjectData
                {
                    Id = new Random().Next(100),
                    Network = "Panevėžio apskritis",
                    Object_Type = obj_type.Butas,
                    Object_GV_Type = obj_gv_type.G,
                    Object_Number = new Random().Next(1, 100),
                    Pplus = new Random().NextDouble(),
                    PL_T = new DateTime(2020, 10, 01),
                    Pminus = new Random().NextDouble()
                },
                    new NetworkObjectData
                {
                    Id = new Random().Next(100),
                    Network = "Šiaulių apskritis",
                    Object_Type = obj_type.Butas,
                    Object_GV_Type = obj_gv_type.NeGV,
                    Object_Number = new Random().Next(1, 100),
                    Pplus = null,
                    PL_T = new DateTime(2020, 07, 01),
                    Pminus = new Random().NextDouble()
                },
                     new NetworkObjectData
                {
                    Id = new Random().Next(100),
                    Network = "Kauno apskritis",
                    Object_Type = obj_type.Butas,
                    Object_GV_Type = obj_gv_type.G,
                    Object_Number = new Random().Next(1, 100),
                    Pplus = null,
                    PL_T = new DateTime(2020, 12, 01),
                    Pminus = new Random().NextDouble()
                },
                      new NetworkObjectData
                {
                    Id = new Random().Next(100),
                    Network = "Marijampolės apskritis",
                    Object_Type = obj_type.Namas,
                    Object_GV_Type = obj_gv_type.N,
                    Object_Number = new Random().Next(1, 100),
                    Pplus = new Random().NextDouble(),
                    PL_T = new DateTime(2020, 09, 01),
                    Pminus = null
                },
                       new NetworkObjectData
                {
                    Id = new Random().Next(100),
                    Network = "Vilniaus apskritis",
                    Object_Type = obj_type.Sodas,
                    Object_GV_Type = obj_gv_type.G,
                    Object_Number = new Random().Next(1, 100),
                    Pplus = new Random().NextDouble(),
                    PL_T = new DateTime(2020, 09, 10),
                    Pminus = new Random().NextDouble()
                },
                        new NetworkObjectData
                {
                    Id = new Random().Next(100),
                    Network = "Panevėžio apskritis",
                    Object_Type = obj_type.Namas,
                    Object_GV_Type = obj_gv_type.G,
                    Object_Number = new Random().Next(1, 100),
                    Pplus = new Random().NextDouble(),
                    PL_T = new DateTime(2020, 09, 05),
                    Pminus = null
                }
            };

            FilteredList = new List<NetworkObjectData>
            {
                new NetworkObjectData
                {
                    Id = new Random().Next(100),
                    Network = "Kėdainių apskritis",
                    Object_Type = obj_type.Butas,
                    Object_GV_Type = obj_gv_type.G,
                    Object_Number = new Random().Next(1, 100),
                    Pplus = null,
                    PL_T = new DateTime(2020,09,01),
                    Pminus = new Random().NextDouble()
                },
                new NetworkObjectData
                {
                    Id = new Random().Next(100),
                    Network = "Kauno apskritis",
                    Object_Type = obj_type.Butas,
                    Object_GV_Type = obj_gv_type.NeGV,
                    Object_Number = new Random().Next(1, 100),
                    Pplus = null,
                    PL_T = new DateTime(2020,09,20),
                    Pminus = null
                },
                 new NetworkObjectData
                {
                    Id = new Random().Next(100),
                    Network = "Vilniaus apskritis",
                    Object_Type = obj_type.Butas,
                    Object_GV_Type = obj_gv_type.G,
                    Object_Number = new Random().Next(1, 100),
                    Pplus = new Random().NextDouble(),
                    PL_T = new DateTime(2020,09,21),
                    Pminus = new Random().NextDouble()
                },
                  new NetworkObjectData
                {
                    Id = new Random().Next(100),
                    Network = "Kauno apskritis",
                    Object_Type = obj_type.Butas,
                    Object_GV_Type = obj_gv_type.N,
                    Object_Number = new Random().Next(1, 100),
                    Pplus = new Random().NextDouble(),
                    PL_T = new DateTime(2020, 09, 02),
                    Pminus = null
                }
            };

            GroupedList = new List<NetworkObjectData>
            {
                 new NetworkObjectData
    {
        Id = 1,
        Network = "Kėdainių apskritis",
        Object_Type = obj_type.Butas,
        Object_GV_Type = obj_gv_type.G,
        Object_Number = 1,
        Pplus = null,
        PL_T = new DateTime(2020, 09, 01),
        Pminus = 0.5
    },
    new NetworkObjectData
    {
        Id = 2,
        Network = "Kauno apskritis",
        Object_Type = obj_type.Butas,
        Object_GV_Type = obj_gv_type.NeGV,
        Object_Number = 2,
        Pplus = null,
        PL_T = new DateTime(2020, 09, 20),
        Pminus = null
    },
    new NetworkObjectData
    {
        Id = 4,
        Network = "Kauno apskritis",
        Object_Type = obj_type.Butas,
        Object_GV_Type = obj_gv_type.N,
        Object_Number = 4,
        Pplus = 0.3,
        PL_T = new DateTime(2020, 09, 02),
        Pminus = null
    },
    new NetworkObjectData
    {
        Id = 7,
        Network = "Kauno apskritis",
        Object_Type = obj_type.Butas,
        Object_GV_Type = obj_gv_type.G,
        Object_Number = 7,
        Pplus = null,
        PL_T = new DateTime(2020, 12, 01),
        Pminus = 0.6
    },
    new NetworkObjectData
    {
        Id = 3,
        Network = "Vilniaus apskritis",
        Object_Type = obj_type.Butas,
        Object_GV_Type = obj_gv_type.G,
        Object_Number = 3,
        Pplus = 0.75,
        PL_T = new DateTime(2020, 09, 21),
        Pminus = 0.25
    },
    new NetworkObjectData
    {
        Id = 9,
        Network = "Vilniaus apskritis",
        Object_Type = obj_type.Sodas,
        Object_GV_Type = obj_gv_type.G,
        Object_Number = 9,
        Pplus = 0.9,
        PL_T = new DateTime(2020, 09, 10),
        Pminus = 0.3
    },
    new NetworkObjectData
    {
        Id = 5,
        Network = "Panevėžio apskritis",
        Object_Type = obj_type.Butas,
        Object_GV_Type = obj_gv_type.G,
        Object_Number = 5,
        Pplus = 0.45,
        PL_T = new DateTime(2020, 10, 01),
        Pminus = 0.1
    },
    new NetworkObjectData
    {
        Id = 10,
        Network = "Panevėžio apskritis",
        Object_Type = obj_type.Namas,
        Object_GV_Type = obj_gv_type.G,
        Object_Number = 10,
        Pplus = 0.1,
        PL_T = new DateTime(2020, 09, 05),
        Pminus = null
    },
    new NetworkObjectData
    {
        Id = 6,
        Network = "Šiaulių apskritis",
        Object_Type = obj_type.Butas,
        Object_GV_Type = obj_gv_type.NeGV,
        Object_Number = 6,
        Pplus = null,
        PL_T = new DateTime(2020, 07, 01),
        Pminus = 0.4
    },
    
    new NetworkObjectData
    {
        Id = 8,
        Network = "Marijampolės apskritis",
        Object_Type = obj_type.Namas,
        Object_GV_Type = obj_gv_type.N,
        Object_Number = 8,
        Pplus = 0.2,
        PL_T = new DateTime(2020, 09, 01),
        Pminus = null
    },
            };

            UnparsedContent = "TINKLAS,OBT_PAVADINIMAS,OBJ_GV_TIPAS,OBJ_NUMERIS,P+,PL_T,P-\r\n"+
                "Kėdainių apskritis,Butas,G,282408,,09/01/2020 00:00:00,0.12345\r\n" +
                "Kauno apskritis,Butas,Ne GV,672336,0.294,09/20/2020 00:00:00,\r\n" +
                "Vilniaus apskritis,Butas,G,143710,0.1969,09/21/2020 00:00:00,0.0\r\n" +
                "Kauno apskritis,Butas,N,157872,0.9133,09/02/2020 00:00:00,0.0\r\n" +
                "Panevėžio apskritis,Butas,G,388680,0.0989,10/01/2020 00:00:00,0.0\r\n" +
                "Šiaulių apskritis,Butas,Ne GV,393360,,07/01/2020 00:00:00,0.12345\r\n" +
                "Kauno apskritis,Butas,G,158472,0.0289,12/01/2020 00:00:00,0.0\r\n" +
                "Marijampolės apskritis,Namas,N,123456,0.54321,09/01/2020 00:00:00,0.0\r\n" +
                "Vilniaus apskritis,Sodas,G,654321,0.98765,09/10/2020 00:00:00,0.0\r\n" +
                "Panevėžio apskritis,Namas,G,234567,0.33333,09/05/2020 00:00:00,0.0";
        }
    }
}
