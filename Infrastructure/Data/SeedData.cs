using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public static class SeedData
    {
        public static void Seed(ModelBuilder builder)
        {
            //Users Seed
            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();

            var _user1 = new ApplicationUser { Id = Guid.NewGuid(), UserName = "user1@gmail.com", Email = "user1@gmail.com" };
            _user1.PasswordHash = passwordHasher.HashPassword(_user1, "user@123");

            var _user2 = new ApplicationUser { Id = Guid.NewGuid(), UserName = "user2@gmail.com", Email = "user2@gmail.com" };
            _user2.PasswordHash = passwordHasher.HashPassword(_user2, "user@123");

            builder.Entity<ApplicationUser>().HasData(_user1);
            builder.Entity<ApplicationUser>().HasData(_user2);

            //Customers Seed
            var customers = new List<Customer>()
            {
                new Customer(){ CustomerId = Guid.NewGuid().ToString(), Name = "User1", EmailAddress = "user1@gmail.com", PhoneNumber = "123456789" , MemberCode = "member_001", UserId = _user1.Id.ToString() },
                new Customer(){ CustomerId = Guid.NewGuid().ToString(), Name = "User2", EmailAddress = "user2@gmail.com", PhoneNumber = "123456789" , MemberCode = "member_002", UserId = _user2.Id.ToString() }
            };

            builder.Entity<Customer>().HasData(customers);

            //ProductType Seed

            var _alcoholType = new ProductType() { ProductTypeId = Guid.NewGuid().ToString(), Code = "Alcohol", Descripton = "Alcohol" };
            var _nonAlcoholType = new ProductType() { ProductTypeId = Guid.NewGuid().ToString(), Code = "Non-Alcohol", Descripton = "Non-Alcohol" };


            builder.Entity<ProductType>().HasData(new List<ProductType>() { _alcoholType, _nonAlcoholType});

            //Product Seed
            var pepsi = new Product()
            {
                ProductId = Guid.NewGuid().ToString(),
                Code = "pepsi",
                Description = "Pepsi",
                ProductTypeId = _nonAlcoholType.ProductTypeId,
                Quantity = 100,
                SellingPrice = 100,
                BuyingPrice = 90
            };

            var jackdaniel = new Product()
            {
                ProductId = Guid.NewGuid().ToString(),
                Code = "jackdaniel",
                Description = "Jack Daniel",
                ProductTypeId = _alcoholType.ProductTypeId,
                Quantity = 50,
                SellingPrice = 500,
                BuyingPrice = 400
            };

            builder.Entity<Product>().HasData(new List<Product>() { pepsi, jackdaniel });

        }
    }
}
