using System.Text.Json;

namespace PaymentPlatform
{
    public class DataStore
    {
        public string FilePath = "data.json";

        public List<Provider> GetAll()
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                var providers = JsonSerializer.Deserialize<List<Provider>>(json);
                return providers ?? [];
            }
            return [];
        }

        public Provider? GetById(Guid id)
        {
            var providers = GetAll();
            return providers.FirstOrDefault(p => p.Id == id);
        }

        public void SaveAll(List<Provider> providers)
        {
            var json = JsonSerializer.Serialize(providers);
            File.WriteAllText(FilePath, json);
        }

        public void Add(Provider provider)
        {
            var providers = GetAll();
            providers.Add(provider);
            SaveAll(providers);
        }

        public bool Remove(Guid id)
        {
            var providers = GetAll();
            var removed = providers.RemoveAll(a => a.Id == id) > 0; //error message on frontend
            if (removed) SaveAll(providers);
            return removed;
        }

        public bool Update(Provider updated, Guid id)
        {
            var providers = GetAll();
            var index = providers.FindIndex(a => a.Id == id);
            if (index == -1) return false; //error message on frontend
            updated.Id = id;
            providers[index] = updated;
            SaveAll(providers);
            return true;

        }
    }
}
