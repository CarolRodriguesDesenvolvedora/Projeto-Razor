using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor_Projeto_Aula.Models;
using System.ComponentModel.DataAnnotations;

namespace Razor_Projeto_Aula.Pages
{
    /// <summary>
    /// Controller responsável por gerenciar a página de Clientes.
    /// Implementa operações de CRUD (Create, Read, Update, Delete).
    /// </summary>
    public class Clientes : PageModel
    {
        /// <summary>
        /// Propriedade para armazenar o nome do cliente.
        /// </summary>

        [Required(ErrorMessage = "O campo nome não pode ser vazio.")]
        public string Nome { get; set; }

        /// <summary>
        /// Propriedade para armazenar o email do cliente.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Propriedade para armazenar o telefone do cliente.
        /// </summary>
        public string Telefone { get; set; }

        /// <summary>
        /// Propriedade para armazenar o ID do cliente.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Lista de clientes carregados do armazenamento.
        /// </summary>
        public List<Cliente> clientes { get; set; } = new List<Cliente>();

        /// <summary>
        /// Objeto usado para armazenar os dados do cliente enviados pelo formulário.
        /// </summary>
        [BindProperty]
        public Cliente novoCliente { get; set; }

        /// <summary>
        /// Método executado ao acessar a página (GET).
        /// </summary>
        /// <param name="id">ID opcional do cliente para edição.</param>
        public void OnGet(int? id)
        {
            // Carrega a lista de clientes do armazenamento.
            clientes = Cliente.load();

            // Se um ID foi fornecido, tenta localizar o cliente correspondente.
            if (id.HasValue)
            {
                var clienteSelecionado = clientes.FirstOrDefault(c => c.Id == id.Value);
                if (clienteSelecionado != null)
                {
                    // Preenche as propriedades para exibição no formulário.
                    Nome = clienteSelecionado.Nome;
                    Email = clienteSelecionado.Email;
                    Telefone = clienteSelecionado.Telefone;
                    Id = clienteSelecionado.Id;
                }
            }
        }

        /// <summary>
        /// Método executado ao enviar o formulário para criar um cliente (POST).
        /// </summary>
        /// <returns>Redireciona para a página atual após salvar o cliente.</returns>
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (novoCliente != null)
            {
                // Salva o novo cliente no armazenamento.
                novoCliente.store();
            }

            // Redireciona de volta para a página atual.
            return RedirectToPage();
        }

        /// <summary>
        /// Método executado ao enviar o formulário para editar um cliente (POST).
        /// </summary>
        /// <param name="id">ID do cliente a ser editado.</param>
        /// <returns>Redireciona para a página atual após a edição.</returns>
        public IActionResult OnPostEdit(int id)
        {
            // Carrega a lista de clientes do armazenamento.
            clientes = Cliente.load();

            // Localiza o cliente a ser editado.
            var cliente = clientes.FirstOrDefault(c => c.Id == id);

            if (cliente != null)
            {
                // Atualiza os dados do cliente.
                cliente.Nome = Request.Form["Nome"];
                cliente.Email = Request.Form["Email"];
                cliente.Telefone = Request.Form["Telefone"];

                // Salva as alterações.
                Cliente.Update(c => c.Id == id, cliente);
            }

            // Redireciona de volta para a página atual.
            Console.WriteLine("estou aqui");
            return Redirect("/Clientes");
        }

        /// <summary>
        /// Método executado ao enviar o formulário para excluir um cliente (POST).
        /// </summary>
        /// <param name="id">ID do cliente a ser excluído.</param>
        /// <returns>Redireciona para a página atual após a exclusão.</returns>
        public IActionResult OnPostDelete(int id)
        {
            // Exclui o cliente com o ID fornecido.
            Cliente.Delete(c => c.Id == id);

            // Redireciona para a página atual.
            return RedirectToPage();
        }
    }
}
