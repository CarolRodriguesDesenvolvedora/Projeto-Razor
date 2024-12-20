using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor_Projeto_Aula.Models;
using System.ComponentModel.DataAnnotations;

namespace Razor_Projeto_Aula.Pages
{
    /// <summary>
    /// Controller respons�vel por gerenciar a p�gina de Clientes.
    /// Implementa opera��es de CRUD (Create, Read, Update, Delete).
    /// </summary>
    public class Clientes : PageModel
    {
        /// <summary>
        /// Propriedade para armazenar o nome do cliente.
        /// </summary>

        [Required(ErrorMessage = "O campo nome n�o pode ser vazio.")]
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
        /// Objeto usado para armazenar os dados do cliente enviados pelo formul�rio.
        /// </summary>
        [BindProperty]
        public Cliente novoCliente { get; set; }

        /// <summary>
        /// M�todo executado ao acessar a p�gina (GET).
        /// </summary>
        /// <param name="id">ID opcional do cliente para edi��o.</param>
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
                    // Preenche as propriedades para exibi��o no formul�rio.
                    Nome = clienteSelecionado.Nome;
                    Email = clienteSelecionado.Email;
                    Telefone = clienteSelecionado.Telefone;
                    Id = clienteSelecionado.Id;
                }
            }
        }

        /// <summary>
        /// M�todo executado ao enviar o formul�rio para criar um cliente (POST).
        /// </summary>
        /// <returns>Redireciona para a p�gina atual ap�s salvar o cliente.</returns>
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

            // Redireciona de volta para a p�gina atual.
            return RedirectToPage();
        }

        /// <summary>
        /// M�todo executado ao enviar o formul�rio para editar um cliente (POST).
        /// </summary>
        /// <param name="id">ID do cliente a ser editado.</param>
        /// <returns>Redireciona para a p�gina atual ap�s a edi��o.</returns>
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

                // Salva as altera��es.
                Cliente.Update(c => c.Id == id, cliente);
            }

            // Redireciona de volta para a p�gina atual.
            Console.WriteLine("estou aqui");
            return Redirect("/Clientes");
        }

        /// <summary>
        /// M�todo executado ao enviar o formul�rio para excluir um cliente (POST).
        /// </summary>
        /// <param name="id">ID do cliente a ser exclu�do.</param>
        /// <returns>Redireciona para a p�gina atual ap�s a exclus�o.</returns>
        public IActionResult OnPostDelete(int id)
        {
            // Exclui o cliente com o ID fornecido.
            Cliente.Delete(c => c.Id == id);

            // Redireciona para a p�gina atual.
            return RedirectToPage();
        }
    }
}
