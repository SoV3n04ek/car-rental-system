using System.Linq;

namespace CarRentalSystem
{
    public class InMemoryVehicleRepository : IVehicleRepository
    {
        private readonly List<Vehicle> _vehicles = new();

        public void Add(Vehicle vehicle) => _vehicles.Add(vehicle);
        public Vehicle GetById(string id) => _vehicles.FirstOrDefault(vehicle => vehicle.Id == id);
        public IEnumerable<Vehicle> GetAvailable() => _vehicles.Where(v => v.IsAvaible);
        public void Update(Vehicle vehicle) 
        {
            // this method is empty because in c# in-memory
            // repository returns a reference to a object
            // if you change a object = it changes in repository
        }

        public IEnumerable<Vehicle> GetVehicles() => _vehicles;   
        
    }
}
