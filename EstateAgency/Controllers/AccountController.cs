using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EstateAgency.Models;
using System.Web.Security;

namespace EstateAgency.Controllers
{
   // [Authorize]
    public class AccountController : Controller
    {
        private readonly AccountRepository accountRepository;

        public AccountController()
        {
            accountRepository = new AccountRepository();
        }

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                Client user = accountRepository.LoginClient(model.Phone, model.Password);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Id.ToString(), true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }

            return View(model);
        }


        public ActionResult LoginManager()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginManager(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                Manager user = accountRepository.LoginManager(model.Phone, model.Password);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Id.ToString(), true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if(!accountRepository.ClientExist(model.Phone))
                {
                    // создаем нового пользователя
                    Client user = await accountRepository.CreateClient(model.Email, model.Password, model.Phone, model.Surname, model.Name, model.Patronymic);                  

                    // если пользователь удачно добавлен в бд
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(user.Id.ToString(), true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
            }
            return View(model);
        }


        public ActionResult RegisterManager()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterManager(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (!accountRepository.ManagerExist(model.Phone))
                {
                    // создаем нового пользователя
                    Manager user = await accountRepository.CreateManager(model.Email, model.Password, model.Phone, model.Surname, model.Name, model.Patronymic);
                    // если пользователь удачно добавлен в бд
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(user.Id.ToString(), true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
            }
            return View(model);
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}