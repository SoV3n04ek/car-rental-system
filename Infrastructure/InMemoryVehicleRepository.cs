using System.Collections.Generic;
using System.Linq;

namespace CarRentalSystem
{
    public class InMemoryVehicleRepository : IVehicleRepository
    {
        private readonly List<Vehicle> _vehicles = new();

        public void Add(Vehicle vehicle) => _vehicles.Add(vehicle);
        public Vehicle GetById(string id) => _vehicles.FirstOrDefault(vehicle => vehicle.Id == id && !vehicle.IsDeleted);
        public IEnumerable<Vehicle> GetAvailable() => _vehicles.Where(v => v.IsAvaible && !v.IsDeleted);
        
        public void Update(Vehicle vehicle) 
        {
            var existing = _vehicles.FirstOrDefault(v => v.Id == vehicle.Id && !v.IsDeleted);
            if (existing == null)
                throw new RentalDomainException("Vehicle not found.");
        }

        public void Delete(string id)
        {
            var vehicle = GetById(id);
            if (vehicle == null)
                throw new RentalDomainException("Vehicle not found or already deleted.");

            vehicle.MarkAsDeleted();
        }

        public IEnumerable<Vehicle> GetAll() => _vehicles.Where(v => !v.IsDeleted);        
    }
}
