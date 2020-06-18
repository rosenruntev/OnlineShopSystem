using Castle.Core.Internal;
using OSS.Business.DTOs;
using OSS.Data;
using OSS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSS.Business.Services
{
    public class ProductService
    {
        public IEnumerable<ProductDto> GetAllByName(string name)
        {
            if (name.IsNullOrEmpty())
            {
                throw new ArgumentException("Invalid product name.");
            }

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var products = unitOfWork.ProductRepository.GetAll(p => p.Name.Contains(name));

                var result = products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Seller = new UserDto
                    {
                        Id = p.Seller.Id,
                        Name = p.Seller.Name,
                        Age = p.Seller.Age,
                        BankBalance = p.Seller.BankBalance,
                        CreatedOn = p.Seller.CreatedOn,
                        UpdatedOn = p.Seller.UpdatedOn
                    },
                    Description = p.Description,
                    CreatedOn = p.CreatedOn,
                    UpdatedOn = p.UpdatedOn
                }).ToList();

                return result;
            }
        }

        public IEnumerable<ProductDto> GetAll()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var products = unitOfWork.ProductRepository.GetAll();

                var result = products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    Seller = new UserDto
                    {
                        Id = p.Seller.Id,
                        Name = p.Seller.Name,
                        Age = p.Seller.Age,
                        BankBalance = p.Seller.BankBalance,
                        CreatedOn = p.Seller.CreatedOn,
                        UpdatedOn = p.Seller.UpdatedOn
                    },
                    Description = p.Description,
                    CreatedOn = p.CreatedOn,
                    UpdatedOn = p.UpdatedOn
                }).ToList();

                return result;
            }
        }

        public ProductDto GetById(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("Invalid product id.");
            }

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var product = unitOfWork.ProductRepository.GetById(id);

                if (product == null)
                {
                    return null;
                }

                ProductDto productDto = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    Seller = new UserDto
                    {
                        Id = product.Seller.Id,
                        Name = product.Seller.Name,
                        Age = product.Seller.Age,
                        BankBalance = product.Seller.BankBalance,
                        CreatedOn = product.Seller.CreatedOn,
                        UpdatedOn = product.Seller.UpdatedOn
                    },
                    Description = product.Description,
                    CreatedOn = product.CreatedOn,
                    UpdatedOn = product.UpdatedOn
                };

                return productDto;
            }
        }

        public bool Create(ProductDto productDto)
        {
            if (!productDto.IsValid())
            {
                throw new ArgumentException("Invalid product.");
            }

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var seller = unitOfWork.UserRepository.GetById(productDto.Seller.Id);
                if (seller == null)
                {
                    return false;
                }

                var product = new Product()
                {
                    Id = productDto.Id,
                    Name = productDto.Name,
                    Price = productDto.Price,
                    Quantity = productDto.Quantity,
                    Seller = seller,
                    Description = productDto.Description,
                    CreatedOn = DateTime.Now
                };

                unitOfWork.ProductRepository.Add(product);

                return unitOfWork.Save();
            }
        }

        public bool Update(ProductDto productDto)
        {
            if (!productDto.IsValid())
            {
                throw new ArgumentException("Invalid product.");
            }

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var product = unitOfWork.ProductRepository.GetById(productDto.Id);
                var seller = unitOfWork.UserRepository.GetById(productDto.Seller.Id);

                if (product == null || seller == null)
                {
                    return false;
                }

                product.Id = productDto.Id;
                product.Name = productDto.Name;
                product.Price = productDto.Price;
                product.Quantity = productDto.Quantity;
                product.Seller = seller;
                product.Description = productDto.Description;
                product.UpdatedOn = DateTime.Now;

                unitOfWork.ProductRepository.Update(product);

                return unitOfWork.Save();
            }
        }

        public bool Delete(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("Invalid product id.");
            }

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                Product product = unitOfWork.ProductRepository.GetById(id);

                if (product == null)
                {
                    return false;
                }

                unitOfWork.ProductRepository.Delete(product);

                return unitOfWork.Save();
            }
        }
    }
}
