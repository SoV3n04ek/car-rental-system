using System;
using System.Collections.Generic;
using System.Linq;

namespace CarRentalSystem
{
    public class InMemoryClientRepository : IClientRepository
    {
        private readonly List<Client> _clients = new();

        public void Add(Client client) => _clients.Add(client);
        public Client GetByPhone(string phone) => _clients.FirstOrDefault(c => c.PhoneNumber == phone && !c.IsDeleted);
        public bool Exists(string phone) => _clients.Any(c => c.PhoneNumber == phone && !c.IsDeleted);
        public Client GetById(Guid id) => _clients.FirstOrDefault(c => c.Id == id && !c.IsDeleted);

        public void Update(Client client)
        {
            var existing = _clients.FirstOrDefault(c => c.Id == client.Id && !c.IsDeleted);
            if (existing == null)
                throw new RentalDomainException("No client found to update.");

            // In-memory repositories usually don't need additional logic for updates 
            // of reference types, but we must ensure the object exists and is not deleted.
        }

        public void Delete(string phone)
        {
            var client = GetByPhone(phone);
            if (client == null)
                throw new RentalDomainException("Client not found or already deleted.");

            client.MarkAsDeleted(); // Soft Delete
        }

        public IEnumerable<Client> GetAll() => _clients.Where(c => !c.IsDeleted);
    }
}
