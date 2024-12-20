using Microsoft.Extensions.Configuration.CommandLine;
using System.Text.Json;

namespace Razor_Projeto_Aula.Utils
{
    internal class Persistable<T>
    {
        public void Store(string path, T item)
        {
            List<T> items = new List<T>();

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    try
                    {
                        items = JsonSerializer.Deserialize<List<T>>(json);
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine("Arquivo estabelecimento não está no padrão! ");
                    }
                }
            }

            Console.WriteLine(items.Count);

            // Calcula o próximo ID
            int newId = items.Count > 0
                ? items.Select(i => GetId(i)).Max() + 1
                : 1;

            // Define o ID no item
            SetId(item, newId);

            // Adiciona o item à lista
            items.Add(item);

            string updatedJson = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, updatedJson);
        }

        public List<T> Load(string path)
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                if (!string.IsNullOrWhiteSpace(json))
                {

                    try
                    {
                        return JsonSerializer.Deserialize<List<T>>(json);
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine("Não foi possível ler o arquivo " + ex.Message);
                    }

                }
            }

            return new List<T>();
        }

        public void Update(string path, Func<T, bool> predicate, T updatedItem)
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                var items = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();

                for (int i = 0; i < items.Count; i++)
                {
                    if (predicate(items[i]))
                    {
                        items[i] = updatedItem;
                        break;
                    }
                }

                string updatedJson = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, updatedJson);
            }
        }

        public void Delete(string path, Func<T, bool> predicate)
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                var items = JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();

                items = items.Where(item => !predicate(item)).ToList();

                string updatedJson = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, updatedJson);
            }
        }

        // Método para obter o valor da propriedade "Id" usando reflection
        private int GetId(T item)
        {
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty != null && idProperty.PropertyType == typeof(int))
            {
                return (int)(idProperty.GetValue(item) ?? 0);
            }
            return 0;
        }

        // Método para definir o valor da propriedade "Id" usando reflection
        private void SetId(T item, int id)
        {
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty != null && idProperty.PropertyType == typeof(int))
            {
                idProperty.SetValue(item, id);
            }
            else
            {
                throw new InvalidOperationException("O tipo T deve conter uma propriedade 'Id' do tipo int.");
            }
        }
    }
}
