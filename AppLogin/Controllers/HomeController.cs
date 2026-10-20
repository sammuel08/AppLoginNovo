using AppLogin.Libraries.Filtro;
using AppLogin.Libraries.Login;
using AppLogin.Models;
using AppLogin.Models.Constants;
using AppLogin.Repository.Contract;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AppLogin.Controllers
{
    public class HomeController : Controller
    {

        // Injeção de dependência
        private IClienteRepository _clienteRepository;
        private LoginCliente _loginCliente;

        public HomeController(
            IClienteRepository clienteRepository,
            LoginCliente loginCliente)
        {
            _clienteRepository = clienteRepository;
            _loginCliente = loginCliente;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] Cliente cliente)
        {
            Cliente clienteDB = _clienteRepository.Login(cliente.Email, cliente.Senha);

            if (clienteDB.Email != null && clienteDB.Senha != null)
            {
                _loginCliente.Login(clienteDB);

                return new RedirectResult(
                    Url.Action(nameof(PainelCliente))
                );
            }
            else
            {
                // Erro na sessão
                ViewData["MSG_E"] =
                    "Usuário não localizado, por favor verifique e-mail e senha digitado";

                return View();
            }
        }
        [ClienteAutorizacao]
        public IActionResult PainelCliente()
        {
           ViewBag.Nome = _loginCliente.GetCliente().Nome;
            ViewBag.CPF = _loginCliente.GetCliente().CPF;
            ViewBag.Email = _loginCliente.GetCliente().Email;
            return View();
          
        }
        [ClienteAutorizacao]
        public IActionResult LogoutCliente()
        {
            _loginCliente.Logout();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Cadastrar()
        {
            return View();
        }
       
        [HttpPost]
        public IActionResult Cadastrar([FromForm] Cliente cliente)
        {
            // 1. Limpa as máscaras (CPF e Telefone) antes de qualquer coisa
            cliente.CPF = Regex.Replace(cliente.CPF ?? "", @"\D", "");
            cliente.Telefone = Regex.Replace(cliente.Telefone ?? "", @"\D", "");

            // 2. Situacao não vem do formulário: define e remove do ModelState
            cliente.Situacao = SituacaoConstant.Ativo;
            ModelState.Remove(nameof(cliente.Situacao));

            // 3. Valida primeiro
            if (!ModelState.IsValid)
            {
                return View(cliente);
            }

            // 4. Verifica duplicidade
            var CPFCad = _clienteRepository.BuscaCpfCliente(cliente.CPF).CPF;
            var EmailExit = _clienteRepository.BuscaEmailCliente(cliente.Email).Email;

            if (!string.IsNullOrWhiteSpace(CPFCad))
            {
                ViewData["MSG_CPF"] = "CPF já cadastrado, por favor verifique os dados digitados";
                return View(cliente);
            }

            if (!string.IsNullOrWhiteSpace(EmailExit))
            {
                ViewData["MSG_Email"] = "Email já cadastrado, por favor verifique os dados digitados";
                return View(cliente);
            }

            // 5. Salva
            _clienteRepository.Cadastrar(cliente);
            return RedirectToAction(nameof(Login));
        }
    }
}
