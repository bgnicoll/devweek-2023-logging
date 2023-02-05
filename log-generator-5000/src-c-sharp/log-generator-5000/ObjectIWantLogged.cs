using Bogus;
using Newtonsoft.Json;

namespace log_generator_5000
{
    public class ObjectIWantLogged
    {
        [JsonProperty("customerId")]
        public int CustomerId { get; set; }
        [JsonProperty("customerName")]
        public string CustomerName { get; set; }
        [JsonProperty("placeOfEmployment")]
        public string PlaceOfEmployment { get; set; }
        [JsonProperty("favoriteFood")]
        public string FavoriteFood { get; set; }
        [JsonProperty("favoriteColor")]
        public string FavoriteColor { get; set; }
        [JsonProperty("mostOuncesOfWaterEverConsumedInOneDay")]
        public int MostOuncesOfWaterEverConsumedInOneDay { get; set; }
        [JsonProperty("lostHopeInHumanityDateTime")]
        public DateTime LostHopeInHumanityDateTime { get; set; }

        public ObjectIWantLogged()
        {
            var faker = new Faker();
            CustomerId = faker.Random.Number(0, int.MaxValue);
            CustomerName = faker.Name.FullName();
            PlaceOfEmployment = faker.Company.CompanyName();
            FavoriteFood = faker.PickRandom(foodsList());
            FavoriteColor = faker.Commerce.Color();
            MostOuncesOfWaterEverConsumedInOneDay = faker.Random.Number(0, 256);
            LostHopeInHumanityDateTime = faker.Date.Recent();
        }

        private List<string> foodsList()
        {
            return new List<string>()
            {
                "Lobster Roll",
                "Bagel",
                "BBQ Ribs",
                "Poutine",
                "Cochinita Pibil",
                "Ceviche",
                "Feijoada",
                "Pizza",
                "Tacos",
                "Carbonara",
                "Mousaka",
                "Paella",
                "Borscht",
                "Dumplings",
                "Sushi",
                "Hamburger",
                "Bratwurst",
                "Pho",
                "Cereal",
                "Literally just shredded cheddar cheese",
                "Chili",
                "Scrambled Eggs",
                "Crab Rangoon",
                "Chips and Salsa",
                "Chocolate",
                "Blackened Salmon",
                "Potato Soup"
            };
        }
    }


}
