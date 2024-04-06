using System.Drawing;

namespace Structs
{
    internal class Program
    {
        //Информация Файла(не папки)
        public struct FileInfo
        {
            //Имя
            public string Name;
            //Полное имя включающее путь
            public string FullName;
            //Размер
            public ulong Size;
            //Размер, на диске
            public ulong SizeOnDisk;
            //Дата создания файла
            public DateTime DateOfCreation;
        }
        //Информация о погоде на конкретной станции
        public struct WeatherStationRecord
        {
            //Время снятия измерений
            public DateTime Time;
            //Название станции
            public string StationName;
            //Температура
            public double Temperature;
            //Влажность
            public double AirHumidity;
        }

        //Информация о погоде
        public struct WeatherState
        {
            //Информация по разным станциям
            public WeatherStationRecord[] Stations;
            //Средняя Температура 
            public double AvgTemperature;
            //Средняя Влажность
            public double AvgAirHumidity;
        }
        //Информация о солдате
        public struct SoldierInfo
        {
            //Имя
            public string Name;
            //Фамилия
            public string Surname;
            //Отчество
            public string Patronymic;
            //Вес
            public double Weight;
            //Рост
            public double Height;

            //Дата прикрепления к части
            public DateTime BeginOfWorkInPart;
        }
        //Информация о части
        public struct ArmySquad
        {
            //Название отделения
            public string Name;

            //Информация а главе части
            public SoldierInfo HeadSoldier;
            //Информация о всех участниках части
            public SoldierInfo[] SoldierInfos;
        }
    }
}
