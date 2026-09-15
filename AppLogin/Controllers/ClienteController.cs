using AppLogin.Repository.Contract;
using Microsoft.AspNetCore.Mvc;
using AppLogin.Models;
using AppLogin.Models.Constants;

namespace AppLogin.Controllers
{
    [Area("Colaborador")]
    public class ClienteController : Controller
    {
        private IClienteRepository _clienteRepository;
        public ClienteController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }
        public IActionResult Index()
        {
            return View(_clienteRepository.ObterTodosClientes());
        }
        public IActionResult Cadastrar()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Cadastrar([FromForm] Cliente cliente)
        {
            cliente.Situacao = SituacaoConstant.Ativo;
            _clienteRepository.Cadastrar(cliente);
            return RedirectToAction(nameof(Cadastrar));
        }
    }
}
