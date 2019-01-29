using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;

namespace PPweb.Models
{
    public class AddressDictionary
    {
        [DataContract]
        public class ARegion
        {
            [DataMember(Name = "code")]
            public string Value
            { get; set; }
            [DataMember(Name = "name")]
            public string Name
            { get; set; }

            public ARegion(string _value, string _name)
            {
                Value = _value;
                Name = _name.ToUpper();
            }
        }
        public class RegionList : List<ARegion>
        {
            public RegionList(string fileName)
            {
                foreach (ARegion reg in ReadRegionListFromFile(fileName))
                {
                    Add(reg);
                }
            }

            private List<ARegion> ReadRegionListFromFile(string fileName)
            {
                List<ARegion> regionList = new List<ARegion>();
                DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(ARegion[]));
                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    ARegion[] _regions = (ARegion[])json.ReadObject(fs);
                    foreach (ARegion region in _regions)
                    {
                        regionList.Add(region);
                    }

                }
                if (regionList == null && regionList.Count == 0)
                    regionList = BaseRegion();
                return regionList;
            }
            private List<ARegion> BaseRegion()
            {
                List<ARegion> regionList = new List<ARegion>();
                regionList.Add(new ARegion("01_Адыгея Республика", "Республика Адыгея"));
                regionList.Add(new ARegion("01_Адыгея Республика", "Республика Адыгея"));
                regionList.Add(new ARegion("02_Башкортостан Республика", "Республика Башкортостан"));
                regionList.Add(new ARegion("03_Бурятия Республика", "республика Бурятия"));
                regionList.Add(new ARegion("04_Алтай Республика", "республика Алтай"));
                regionList.Add(new ARegion("05_Дагестан Республика", "республика Дагестан"));
                regionList.Add(new ARegion("06_Ингушетия Республика", "республика Ингушетия"));
                regionList.Add(new ARegion("07_Кабардино-Балкарская Республика", "Кабардино-Балкарская республика"));
                regionList.Add(new ARegion("08_Калмыкия Республика", "республика Калмыкия"));
                regionList.Add(new ARegion("09_Карачаево-Черкесская Республика", "Карачаево-Черкесская республика"));
                regionList.Add(new ARegion("10_Карелия Республика", "республика Карелия"));
                regionList.Add(new ARegion("11_Коми Республика", "республика Коми"));
                regionList.Add(new ARegion("12_Марий Эл Республика", "республика Марий Эл"));
                regionList.Add(new ARegion("13_Мордовия Республика", "республика Мордовия"));
                regionList.Add(new ARegion("14_Саха /Якутия/ Республика", "республика Саха/Якутия"));
                regionList.Add(new ARegion("15_Северная Осетия - Алания Республика", "республика Северная Осетия - Алания"));
                regionList.Add(new ARegion("16_Татарстан Республика", "республика Татарстан"));
                regionList.Add(new ARegion("17_Тыва Республика", "республика Тыва"));
                regionList.Add(new ARegion("18_Удмуртская Республика", "Удмуртская республика"));
                regionList.Add(new ARegion("19_Хакасия Республика", "республика Хакасия"));
                regionList.Add(new ARegion("20_Чеченская Республика", "Чеченская республика"));
                regionList.Add(new ARegion("21_Чувашская Республика - Чувашия", "Чувашская республика - Чувашия"));
                regionList.Add(new ARegion("22_Алтайский Край", "Алтайский край"));
                regionList.Add(new ARegion("23_Краснодарский Край", "Краснодарский край"));
                regionList.Add(new ARegion("24_Красноярский Край", "Красноярский край"));
                regionList.Add(new ARegion("25_Приморский Край", "Приморский край"));
                regionList.Add(new ARegion("26_Ставропольский Край", "Ставропольский край"));
                regionList.Add(new ARegion("27_Хабаровский Край", "Хабаровский край"));
                regionList.Add(new ARegion("28_Амурская Область", "Амурская область"));
                regionList.Add(new ARegion("29_Архангельская Область", "Архангельская область"));
                regionList.Add(new ARegion("30_Астраханская Область", "Астраханская область"));
                regionList.Add(new ARegion("31_Белгородская Область", "Белгородская область"));
                regionList.Add(new ARegion("32_Брянская Область", "Брянская область"));
                regionList.Add(new ARegion("33_Владимирская Область", "Владимирская область"));
                regionList.Add(new ARegion("34_Волгоградская Область", "Волгоградская область"));
                regionList.Add(new ARegion("35_Вологодская Область", "Вологодская область"));
                regionList.Add(new ARegion("36_Воронежская Область", "Воронежская область"));
                regionList.Add(new ARegion("37_Ивановская Область", "Ивановская область"));
                regionList.Add(new ARegion("38_Иркутская Область", "Иркутская область"));
                regionList.Add(new ARegion("39_Калининградская Область", "Калининградская область"));
                regionList.Add(new ARegion("40_Калужская Область", "Калужская область"));
                regionList.Add(new ARegion("41_Камчатский Край", "Камчатский край"));
                regionList.Add(new ARegion("42_Кемеровская Область", "Кемеровская область"));
                regionList.Add(new ARegion("43_Кировская Область", "Кировская область"));
                regionList.Add(new ARegion("44_Костромская Область", "Костромская область"));
                regionList.Add(new ARegion("45_Курганская Область", "Курганская область"));
                regionList.Add(new ARegion("46_Курская Область", "Курская область"));
                regionList.Add(new ARegion("47_Ленинградская Область", "Ленинградская область"));
                regionList.Add(new ARegion("48_Липецкая Область", "Липецкая область"));
                regionList.Add(new ARegion("49_Магаданская Область", "Магаданская область"));
                regionList.Add(new ARegion("50_Московская Область", "Московская область"));
                regionList.Add(new ARegion("51_Мурманская Область", "Мурманская область"));
                regionList.Add(new ARegion("52_Нижегородская Область", "Нижегородская область"));
                regionList.Add(new ARegion("53_Новгородская Область", "Новгородская область"));
                regionList.Add(new ARegion("54_Новосибирская Область", "Новосибирская область"));
                regionList.Add(new ARegion("55_Омская Область", "Омская область"));
                regionList.Add(new ARegion("56_Оренбургская Область", "Оренбургская область"));
                regionList.Add(new ARegion("57_Орловская Область", "Орловская область"));
                regionList.Add(new ARegion("58_Пензенская Область", "Пензенская область"));
                regionList.Add(new ARegion("59_Пермский Край", "Пермский край"));
                regionList.Add(new ARegion("60_Псковская Область", "Псковская область"));
                regionList.Add(new ARegion("61_Ростовская Область", "Ростовская область"));
                regionList.Add(new ARegion("62_Рязанская Область", "Рязанская область"));
                regionList.Add(new ARegion("63_Самарская Область", "Самарская область"));
                regionList.Add(new ARegion("64_Саратовская Область", "Саратовская область"));
                regionList.Add(new ARegion("65_Сахалинская Область", "Сахалинская область"));
                regionList.Add(new ARegion("66_Свердловская Область", "Свердловская область"));
                regionList.Add(new ARegion("67_Смоленская Область", "Смоленская область"));
                regionList.Add(new ARegion("68_Тамбовская Область", "Тамбовская область"));
                regionList.Add(new ARegion("69_Тверская Область", "Тверская область"));
                regionList.Add(new ARegion("70_Томская Область", "Томская область"));
                regionList.Add(new ARegion("71_Тульская Область", "Тульская область"));
                regionList.Add(new ARegion("72_Тюменская Область", "Тюменская область"));
                regionList.Add(new ARegion("73_Ульяновская Область", "Ульяновская область"));
                regionList.Add(new ARegion("74_Челябинская Область", "Челябинская область"));
                regionList.Add(new ARegion("75_Забайкальский Край", "Забайкальский край"));
                regionList.Add(new ARegion("76_Ярославская Область", "Ярославская область"));
                regionList.Add(new ARegion("77_Москва Город", "Москва"));
                regionList.Add(new ARegion("78_Санкт-Петербург Город", "Санкт-Петербург"));
                regionList.Add(new ARegion("79_Еврейская Автономная область", "Еврейская Автономная область"));
                regionList.Add(new ARegion("80_Забайкальский край Агинский Бурятский Округ", "Забайкальский край Агинский Бурятский округ"));
                regionList.Add(new ARegion("81_Коми-Пермяцкий Автономный округ", "Коми-Пермяцкий Автономный округ"));
                regionList.Add(new ARegion("82_Корякский Автономный округ", "Корякский Автономный округ"));
                regionList.Add(new ARegion("83_Ненецкий Автономный округ", "Ненецкий Автономный округ"));
                regionList.Add(new ARegion("84_Таймырский (Долгано-Ненецкий) Автономный округ", "Таймырский (Долгано-Ненецкий) Автономный округ"));
                regionList.Add(new ARegion("85_Иркутская обл Усть-Ордынский Бурятский Округ", "Иркутская область"));
                regionList.Add(new ARegion("86_Ханты-Мансийский Автономный округ - Югра Автономный округ", "Ханты-Мансийский Автономный округ - Югра"));
                regionList.Add(new ARegion("87_Чукотский Автономный округ", "Чукотский Автономный округ"));
                regionList.Add(new ARegion("88_Эвенкийский Автономный округ", "Эвенкийский Автономный округ"));
                regionList.Add(new ARegion("89_Ямало-Ненецкий Автономный округ", "Ямало-Ненецкий Автономный округ"));
                regionList.Add(new ARegion("91_Крым Республика", "Республика Крым"));
                regionList.Add(new ARegion("92_Севастополь Город", "Севастополь"));
                regionList.Add(new ARegion("99_Байконур Город", "Байконур"));

                return regionList;
            }
        }
    }
}