using AppLogin.Libraries.Filtro;
using AppLogin.Models.Constants;
using AppLogin.Repository;
using AppLogin.Repository.Contract;
using Microsoft.AspNetCore.Mvc;

namespace AppLogin.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacao(ColaboradorTipoConstant.Gerente)]
    public class ColaboradorController : Controller
    {
        private IColaboradorRepository _colaboradorRepository;

        public ColaboradorController(IColaboradorRepository colaboradorRepository)
        {
            _colaboradorRepository = colaboradorRepository;
        }

        public IActionResult Index()
        {
            return View(_colaboradorRepository.ObterTodosColaboradores());
        }
        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }
        [HttpPost]
        
        public IActionResult Cadastrar(Models.Colaborador colaborador)
        {
            colaborador.Tipo = ColaboradorTipoConstant.Comum;
            _colaboradorRepository.Cadastrar(colaborador);
            TempData["MSG_S"] = "Registro salvo com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult Atualizar(int id)
        {
            Models.Colaborador colaborador = _colaboradorRepository.ObterColaborador(id);
            return View(colaborador);
        }
        [HttpPost]
        public IActionResult Atualizar([FromForm] Models.Colaborador colaborador)
        {
            ModelState.Remove(nameof(colaborador.CPF));
            ModelState.Remove(nameof(colaborador.Tipo));
            ModelState.Remove(nameof(colaborador.Telefone));

            if (string.IsNullOrWhiteSpace(colaborador.Senha))
            {
                ModelState.Remove(nameof(colaborador.Senha));
            }

            if (ModelState.IsValid)
            {
                Models.Colaborador existente = _colaboradorRepository.ObterColaborador(colaborador.Id);
                existente.Nome = colaborador.Nome;
                existente.Email = colaborador.Email;

                if (!string.IsNullOrWhiteSpace(colaborador.Senha))
                {
                    existente.Senha = colaborador.Senha;
                }

                _colaboradorRepository.Atualizar(existente);
                TempData["MSG_S"] = "Registro salvo com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(colaborador);
        }
        public IActionResult Excluir(int id)
        {
            _colaboradorRepository.Excluir(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
