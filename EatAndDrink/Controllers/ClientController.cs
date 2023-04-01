using EatAndDrink.Data;
using EatAndDrink.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EatAndDrink.Controllers
{
    public class ClientController : Controller
    {
        private ApplicationDbContext _dbContext;
        public ClientController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }


        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Index()
        {
            return View(this._dbContext.Clients.ToList());
        }

        //List<Client> client = new List<Client>();

        public class ClientCreateViewModel
        {
            [Required]
            [Display(Name = "First name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last name")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "Address")]
            public string Address { get; set; }
        }

        public IActionResult New()
        {
            return View(new ClientCreateViewModel());
        }

        public async Task<IActionResult> Create(ClientCreateViewModel model)
        {
                var clientNew = new Client
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address
                };
                _dbContext.Clients.Add(clientNew);
            _dbContext.SaveChanges();
            
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(Client model)
        {
                this._dbContext.Clients.Update(model);
                this._dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int Id)
        {
            return View(this._dbContext.Clients.Find(Id));
        }

        //Delete Client
        public async Task<IActionResult> Delete(int id)
        {
           var client = this._dbContext.Clients.Find(id);
           this._dbContext.Clients.Remove(client);
           this._dbContext.SaveChanges();
           return RedirectToAction("Index");
        }

    }




}