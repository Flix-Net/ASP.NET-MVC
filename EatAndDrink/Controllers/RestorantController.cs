using EatAndDrink.Data;
using EatAndDrink.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EatAndDrink.Controllers
{
    public class RestorantController : Controller
    {
        private ApplicationDbContext _dbContext;
        public RestorantController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View(this._dbContext.Restorants.ToList());
        }

        public class RestorantCreateViewModel
        {
            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "PhoneNumber")]
            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "Address")]
            public string Address { get; set; }

        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult New()
        {
            return View(new RestorantCreateViewModel());
        }

        public async Task<IActionResult> Create(RestorantCreateViewModel model)
        {
            var restorantNew = new Restorant
            {
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address
            };
            _dbContext.Restorants.Add(restorantNew);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(Restorant model)
        {
            this._dbContext.Restorants.Update(model);
            this._dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult Edit(int Id)
        {
            return View(this._dbContext.Restorants.Find(Id));
        }

        //Delete Client
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var restorant = this._dbContext.Restorants.Find(id);
            this._dbContext.Restorants.Remove(restorant);
            this._dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}