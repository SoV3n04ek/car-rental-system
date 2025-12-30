namespace CarRentalSystem
{
    public class InMemoryClientRepository : IClientRepository
    {
        private readonly List<Client> _clients = new();

        public void Add(Client client) => _clients.Add(client);
        public Client GetByPhone(string phone) => _clients.FirstOrDefault(c => c.PhoneNumber == phone);
        public bool Exists(string phone) => _clients.Any(c => c.PhoneNumber == phone);
        public Client GetById(Guid id) => _clients.FirstOrDefault(c => c.Id == id);
    }
}
