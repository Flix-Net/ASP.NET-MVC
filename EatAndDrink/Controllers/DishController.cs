using EatAndDrink.Data;
using EatAndDrink.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace EatAndDrink.Controllers
{
    public class DishController : Controller
    {
        private ApplicationDbContext _dbContext;
        public DishController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View(this._dbContext.Dishes.ToList());
        }

        public class DishCreateViewModel
        {
            [Required]
            [Display(Name = "Name Dish")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Price")]
            public string Price { get; set; }

        }

        [Authorize(Roles = "Admin,Manager")]
        public IActionResult New()
        {
            return View(new DishCreateViewModel());
        }

        public async Task<IActionResult> Create(DishCreateViewModel model)
        {
            var dishNew = new Dish
            {
                Name = model.Name,
                Price = model.Price
            };
            _dbContext.Dishes.Add(dishNew);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(Dish model)
        {
            this._dbContext.Dishes.Update(model);
            this._dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult Edit(int Id)
        {
            return View(this._dbContext.Dishes.Find(Id));
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var dish = this._dbContext.Dishes.Find(id);
            this._dbContext.Dishes.Remove(dish);
            this._dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}