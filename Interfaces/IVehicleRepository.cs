using System.Collections.Generic;

namespace CarRentalSystem
{
    public interface IVehicleRepository
    {
        public void Add(Vehicle vehicle);
        Vehicle GetById(string id);
        IEnumerable<Vehicle> GetAvailable();
        void Update(Vehicle vehicle);
        void Delete(string id);
        IEnumerable<Vehicle> GetAll();
    }
}
