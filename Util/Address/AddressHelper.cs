using BackendLaboratory.Data.Entities;
using BackendLaboratory.Data.Entities.Enums;

namespace BackendLaboratory.Util.Address
{
    public static class AddressHelper
    {
        public static ObjectLevel GetAddressLevel(int? level)
        {
            return level switch
            {
                1 => ObjectLevel.Region,
                2 => ObjectLevel.AdministrativeArea,
                3 => ObjectLevel.MunicipalArea,
                4 => ObjectLevel.RuralUrbanSettlement,
                5 => ObjectLevel.City,
                6 => ObjectLevel.Locality,
                7 => ObjectLevel.ElementOfPlanningStructure,
                8 => ObjectLevel.ElementOfRoadNetwork,
                9 => ObjectLevel.Land,
                10 => ObjectLevel.Building,
                11 => ObjectLevel.Room,
                12 => ObjectLevel.RoomInRooms,
                13 => ObjectLevel.AutonomousRegionLevel,
                14 => ObjectLevel.IntracityLevel,
                15 => ObjectLevel.AdditionalTerritoriesLevel,
                16 => ObjectLevel.LevelOfObjectsInAdditionalTerritories,
                17 => ObjectLevel.CarPlace,
                _ => ObjectLevel.Region
            };
        }

        public static string GetAddressName(string type, string name)
        {
            return type + " " + name;
        }

        public static string GetHouseName(AsHouse house)
        {
            var chainHouseNumber = new List<String>();

            if (!String.IsNullOrEmpty(house.Housenum))
            {
                chainHouseNumber.Add(house.Housenum);
            }

            if (house.Addtype1 != null)
            {
                chainHouseNumber.Add(AddressHelper.GetHouseTypeName(house.Addtype1));
                chainHouseNumber.Add(house.Addnum1 ?? AppConstants.EmptyString);
            }

            if (house.Addtype2 != null)
            {
                chainHouseNumber.Add(AddressHelper.GetHouseTypeName(house.Addtype2));
                chainHouseNumber.Add(house.Addnum2 ?? AppConstants.EmptyString);
            }

            return String.Join(" ", chainHouseNumber).Trim();
        }

        public static string GetAddressLevelName(int? level)
        {
            return level switch
            {
                1 => "Субъект РФ",
                2 => "Административный район",
                3 => "Муниципальный район",
                4 => "Сельское/городское поселение",
                5 => "Город",
                6 => "Населенный пункт",
                7 => "Элемент планировочной структуры",
                8 => "Элемент улично-дорожной сети",
                9 => "Земельный участок",
                10 => "Здание (сооружение)",
                _ => "Неизвестный тип объекта адреса"
            };
        }

        public static string GetHouseTypeName(int? type)
        {
            var name = type switch
            {
                1 => "корпус",
                2 => "строение",
                3 => "сооружение",
                4 => "литера",
                _ => String.Empty
            };
            return name;
        }
    }
}
