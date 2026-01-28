namespace CarRentalSystem
{
    public interface IClientRepository
    {
        Client GetById(Guid id);
        Client GetByPhone(string phone);
        void Add(Client client);
        bool Exists(string phone);
        void Update(Client client);
        void Delete(string phone);
    }
}
