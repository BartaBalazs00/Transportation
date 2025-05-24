using AutoMapper;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Transportation.Data;
using Transportation.Entities.Dto;
using Transportation.Entities.Entity;
using Transportation.Logic.Dto;

namespace Transportation.Logic
{
    public class VehicleLogic
    {
        public Repository<Vehicle> repository;
        public Mapper mapper;
        public VehicleLogic(Repository<Vehicle> repository, DtoProvider provider)
        {
            this.repository = repository;
            this.mapper = provider.Mapper;
        }
        public IEnumerable<VehicleViewDto> Read()
        {
            return repository.GetAll().Select(mapper.Map<VehicleViewDto>);
        }
        public VehicleViewDto Read(string id)
        {
            return mapper.Map<VehicleViewDto>(repository.GetAll().FirstOrDefault(v => v.Id == id));
        }
        public async Task Create(VehicleCreateUpdateDto dto)
        { 
            CreateUpdateValidation(dto);
            var vehicle = mapper.Map<Vehicle>(dto);
            await repository.CreateAsync(vehicle);
        }
        public async Task Delete(string id)
        {
            await repository.DeleteByIdAsync(id);
        }
        public async Task Update(VehicleCreateUpdateDto dto, string id)
        {
            CreateUpdateValidation(dto);
            var VehicleToUpdate = repository.FindById(id);
            if (VehicleToUpdate != null)
            {
                mapper.Map(dto, VehicleToUpdate);
                await repository.UpdateAsync(VehicleToUpdate);
            }
        }
        public IEnumerable<TripSuggestionDto> GetTripSuggestion(int passengers, int distance)
        {
            var validVehicles = repository.GetAll()
                .Where(v => v.RangeKm >= distance)
                .OrderBy(v => v.PassengerCapacity)
                .ToList();
            int minLength = CombinationsMinLength(validVehicles, passengers);
            int maxLength = CombinationsMaxLength(validVehicles, passengers);
            List<List<Vehicle>> generated = new List<List<Vehicle>>();
            for (int size = minLength; size <= maxLength; size++)
            {
                generated.AddRange(GenerateCombinationsBasedOnK(validVehicles, size));
            }
            var validCombinations = generated
                .Where(c => c.Sum(v => v.PassengerCapacity) >= passengers)
                .Select(c => new TripSuggestionDto
                {
                    Vehicles = c,
                    Profit = CalculateProfit(c, distance, passengers)
                })
                .OrderByDescending(t => t.Profit)
                .Take(1000)//nagy adatt esetén ne akadjon ki a swagger
                .ToList();
            return validCombinations;
        }

        

        private double CalculateProfit(List<Vehicle> vehicles, int distance, int passengers)
        {
            var minutes = distance < 50
            ? distance * 2
            : 100 + (distance - 50) * 1;

            var hours = (int)Math.Ceiling(minutes / 30.0);

            double travelFee = 2 * distance * passengers + 2 * hours * passengers;

            double fuelCost = vehicles.Sum(v =>
            {
                double costPerKm = v.Fuel switch
                {
                    FuelType.Gasoline => 2,
                    FuelType.Electric => 1,
                    FuelType.Hybrid => 1.5
                };
                var effectiveDistance = v.Fuel == FuelType.Hybrid && distance < 50
                    ? distance / 2
                    : distance;
                return effectiveDistance * costPerKm;
            });

            return travelFee-fuelCost;
        }
        //2^N-es futásidő
        private List<List<Vehicle>> GenerateCombinations(List<Vehicle> vehicles)
        {
            var result = new List<List<Vehicle>>();
            int n = vehicles.Count;

            for (int i = 1; i < (1 << n); i++)
            {
                var combination = new List<Vehicle>();
                for (int j = 0; j < n; j++)
                {
                    if ((i & (1 << j)) != 0)
                        combination.Add(vehicles[j]);
                }
                result.Add(combination);
            }
            return result;
        }
        //k*(nCk)
        private List<List<Vehicle>> GenerateCombinationsBasedOnK(List<Vehicle> vehicles, int k)
        {
            var result = new List<List<Vehicle>>();
            int n = vehicles.Count;
            if (k > n) return result;

            var indices = Enumerable.Range(0, k).ToArray();
            bool hasNext = true;
            while (hasNext)
            {
                result.Add(indices.Select(i => vehicles[i]).ToList());

                hasNext = false;
                for (int t = k - 1; t >= 0; t--)
                {
                    if (indices[t] < n - k + t)
                    {
                        indices[t]++;
                        for (int i = t + 1; i < k; i++)
                            indices[i] = indices[i - 1] + 1;
                        hasNext = true;
                        break;
                    }
                }
            }

            return result;
        }

        //hány kocsiba fér a maximum esetben az utasok száma
        private int CombinationsMaxLength(List<Vehicle> vehicles, int passengers)
        {
            int n = vehicles.Count;
            int maxPassengerCapacity = 0;
            int i = 0;
            while (maxPassengerCapacity < passengers && i != n)
            {
                maxPassengerCapacity += vehicles[i].PassengerCapacity;
                i++;
            }
            return i;
        }
        //hány kocsiba fér a minimum esetben az utasok száma
        private int CombinationsMinLength(List<Vehicle> validVehicles, int passengers)
        {
            int i = validVehicles.Count-1;
            int minPassengerCapacity = 0;
            int count = 0;
            while (minPassengerCapacity < passengers && i != -1)
            {
                minPassengerCapacity += validVehicles[i].PassengerCapacity;
                count++;
                i--;
            }
            return count;
        }
        
        private List<List<Vehicle>> GenerateCombinationsParallelized(List<Vehicle> vehicles)
        {
            int n = vehicles.Count;
            var result = new ConcurrentBag<List<Vehicle>>();

            Parallel.For(1, 1 << n, i =>
            {
                var combo = new List<Vehicle>();

                for (int j = 0; j < n; j++)
                {
                    if ((i & (1 << j)) != 0)
                    {
                        combo.Add(vehicles[j]);
                    }
                }

                result.Add(combo);
            });

            return result.ToList();
        }
        private bool CreateUpdateValidation(VehicleCreateUpdateDto dto)
        {
            if (!FuelType.IsDefined(dto.Fuel))
            {
                throw new ValidationException($"Invalid fuel type: {dto.Fuel}");
            }
            if (dto.PassengerCapacity <= 0)
            {
                throw new ValidationException("Invalid PassengerCapacity: Must be greater then 0.");
            }
            if (dto.RangeKm <= 0)
            {
                throw new ValidationException("Invalid RangeKm: Must be greater then 0.");
            }
            return true;
        }
    }
}
