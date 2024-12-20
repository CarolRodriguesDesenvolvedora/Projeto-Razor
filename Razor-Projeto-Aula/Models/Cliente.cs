using Razor_Projeto_Aula.Utils;

namespace Razor_Projeto_Aula.Models
{

    
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }

        public void store()
        {
            Persistable<Cliente> persistable = new Persistable<Cliente>();
            persistable.Store("./cliente.json", this);
        }
        public static List<Cliente> load() //static = não retorna um objeto e sim da classe 
        {
            Persistable<Cliente> persistable = new Persistable<Cliente>();
            return persistable.Load("./cliente.json");
        }

        public static void Delete(Func<Cliente, bool> predicate) //predicate?
        {
            Persistable<Cliente> persistable = new Persistable<Cliente>();
            persistable.Delete("./cliente.json", predicate);
        }

        public static void Update(Func<Cliente, bool> predicate, Cliente updatedCliente)
        {
            Persistable<Cliente> persistable = new Persistable<Cliente>();
            persistable.Update("./cliente.json", predicate, updatedCliente);
        }
    }

}
