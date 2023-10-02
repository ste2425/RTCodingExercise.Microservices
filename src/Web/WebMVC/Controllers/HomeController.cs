
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using RTCodingExercise.Microservices.Models;
using RTCodingExercise.Microservices.Services;

namespace RTCodingExercise.Microservices.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IConfiguration config, ITokenService tokenService, IUserRepository userRepository)
        {
            _logger = logger;
            _config = config;
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> PerformLogin(UserModel userModel)
        {
            if (string.IsNullOrEmpty(userModel.UserName) || string.IsNullOrEmpty(userModel.Password))
                return RedirectToAction("Error");

            var validUser = GetUser(userModel);

            if (validUser == null)
                return RedirectToAction("Error");


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, validUser.UserName),
                new Claim(ClaimTypes.Role, validUser.Role),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index");
        }

        private UserDTO GetUser(UserModel userModel)
        {
            // Write your code here to authenticate the user     
            return _userRepository.GetUser(userModel);
        }

        [Authorize]
        [Route("mainwindow")]
        [HttpGet]
        public IActionResult MainWindow()
        {
            ViewBag.Message = "hello";

            return View();
        }

        public IActionResult Error()
        {
            ViewBag.Message = "An error occured...";
            return View();
        }
    }
}