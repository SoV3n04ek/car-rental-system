namespace CarRentalSystem
{
    public class InMemoryClientRepository : IClientRepository
    {
        private readonly List<Client> _clients = new();

        public void Add(Client client) => _clients.Add(client);
        public Client GetByPhone(string phone) => _clients.FirstOrDefault(c => c.PhoneNumber == phone);
        public bool Exists(string phone) => _clients.Any(c => c.PhoneNumber == phone);
        public Client GetById(Guid id) => _clients.FirstOrDefault(c => c.Id == id);

        public void Update(Client client)
        {
            var existing = GetById(client.Id);
            if (existing == null)
                throw new InvalidOperationException("Клієнта не знайдено для оновлення.");

            // В InMemory ми зазвичай просто оновлюємо поля, 
            // або замінюємо об'єкт у списку, якщо він був замінений повністю
            // Але оскільки це посилання (reference), зміни в об'єкті вже відображені в списку.
        }

        public void Delete(string phone)
        {
            var client = GetByPhone(phone);
            if (client == null || client.IsDeleted)
                throw new InvalidOperationException("Клієнта не знайдено або він вже видалений.");

            client.MarkAsDeleted(); // Виконуємо Soft Delete
        }


    }
}
