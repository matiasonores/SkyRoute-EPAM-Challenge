using SkyRoute.Application.DTOs;
using SkyRoute.Application.Interfaces;
using SkyRoute.Domain.Enums;

namespace SkyRoute.Infraestructure.Providers
{
    public static class FlightGenerator
    {
        public static List<ProviderFlightResponse> GenerateFlights(FlightSearchRequest request,FlightProviderSettings settings,IProviderPricingStrategy pricingStrategy)
        {
            var random = new Random(BuildSeed(request, settings.ProviderName));
            var flightCount =random.Next(settings.MinFlights,settings.MaxFlights + 1);
            var flights = new List<ProviderFlightResponse>();
            TimeOnly[] departureSlots = { new(0, 0), new(1, 30), new(2, 30), new(6, 0), new(8, 30), new(11, 0), new(14, 0), new(17, 30), new(20, 0), new(22, 30) };
            var leg = request.Legs.First();

            for (var i = 0; i < flightCount; i++)
            {
                var passengers = request.Passengers;
                var duration = GenerateFlightDuration(leg.Origin.Country.Code, leg.Destination.Country.Code, random);
                var slot = departureSlots[random.Next(departureSlots.Length)];
                var departure = leg.DepartureDate.ToDateTime(slot);
                var arrival = departure.AddMinutes(duration);
                var isInternational = !leg.Origin.Country.Code.Equals(leg.Destination.Country.Code, StringComparison.OrdinalIgnoreCase);
                var baseFare = GenerateBaseFare(request.CabinClass, duration, isInternational, random);
                var finalPrice = pricingStrategy.CalculatePrice(baseFare);
                var airline = GenerateAirline(leg.Origin.Country.Code,leg.Destination.Country.Code);
                flights.Add(new ProviderFlightResponse
                {
                    Provider = settings.ProviderName,
                    Airline = airline,
                    FlightCode = GenerateFlightCode(settings, request, i),
                    OriginCode = leg.Origin.Code,
                    OriginName = leg.Origin.Name,
                    OriginCountry = leg.Origin.Country.Code,
                    DestinationCode = leg.Destination.Code,
                    DestinationName = leg.Destination.Name,
                    DestinationCountry = leg.Destination.Country.Code,
                    DepartureTime = departure,
                    ArrivalTime = arrival,
                    DurationMinutes = duration,
                    Cabin = ResolveCabin(request, settings).ToString(),
                    Passengers = passengers,
                    PricePerPassenger = finalPrice,
                    TotalAmount = finalPrice * passengers
                });
            }

            return flights;
        }

        private static decimal GenerateBaseFare(CabinClass cabinClass, int durationMinutes, bool isInternational, Random random)
        {
            decimal pricePerHour = isInternational ? 40m : 25m;

            var durationHours = (decimal)durationMinutes / 60m;

            var baseFare = durationHours * pricePerHour;

            var multiplier =ResolveCabinMultiplier(cabinClass);

            var variation = 0.90m +((decimal)random.NextDouble() * 0.20m);

            return Math.Round(baseFare *multiplier *variation,2);
        }
        private static decimal ResolveCabinMultiplier(CabinClass cabinClass)
        {
            return cabinClass switch
            {
                CabinClass.Economy => 1.0m,
                CabinClass.Business => 2.0m,
                CabinClass.First => 3.5m,
                _ => 1.0m
            };
        }
        private static string ResolveCabin(FlightSearchRequest request, FlightProviderSettings settings)
        {
            var cabin = request.CabinClass;

            if (settings.SupportedCabins.Contains(cabin))
            {
                return cabin.ToString();
            }

            return settings.SupportedCabins.First().ToString();
        }
        private static string GenerateFlightCode(FlightProviderSettings settings,FlightSearchRequest request,int index)
        {
            return $"{settings.FlightPrefix}" +$"{1000 + index}" +$"{Math.Abs(BuildSeed(request,settings.ProviderName)) % 100}";
        }
        private static string GenerateAirline(string originCountry, string destinationCountry)
        {
            string[] argentinianAirlines = { "Aerolíneas Argentinas", "Flybondi", "JetSMART Argentina" };
            string[] usAirlines = { "American Airlines", "Delta Air Lines", "United Airlines" };
            Random random = new();

            originCountry = originCountry.ToUpperInvariant();
            destinationCountry = destinationCountry.ToUpperInvariant();

            if ((originCountry == "AR" && destinationCountry == "US") || (originCountry == "US" && destinationCountry == "AR"))
            {
                return argentinianAirlines[0]; //Aerolíneas Argentinas
            }

            if (originCountry == "AR" && destinationCountry == "AR")
            {
                return argentinianAirlines[random.Next(argentinianAirlines.Length)];
            }

            if (originCountry == "US" && destinationCountry == "US")
            {
                return usAirlines[random.Next(usAirlines.Length)];
            }

            return argentinianAirlines[random.Next(argentinianAirlines.Length)];
        }
        private static int GenerateFlightDuration(string originCountry, string destinationCountry, Random random)
        {
            int[] domesticDurations = { 60, 80, 90, 110 };
            int[] internationaDurations = { 480, 600, 720 };
            if (originCountry == "AR" && destinationCountry == "AR")
            {
                
                return domesticDurations[random.Next(domesticDurations.Length)];
            }

            if (originCountry == "US" && destinationCountry == "US")
            {
                return domesticDurations[random.Next(domesticDurations.Length)] * 2;
            }

            return internationaDurations[random.Next(internationaDurations.Length)];
        }
        private static int BuildSeed(FlightSearchRequest request,string providerName)
        {
            var hash = new HashCode();

            hash.Add(providerName);
            hash.Add(request.Passengers);
            hash.Add(request.CabinClass);
            hash.Add(request.FlightType);
            hash.Add(request.FlexDates);

            foreach (var leg in request.Legs)
            {
                hash.Add(leg.Origin.Code);
                hash.Add(leg.Destination.Code);
                hash.Add(leg.DepartureDate);
            }

            return hash.ToHashCode();
        }

        
    }

}
