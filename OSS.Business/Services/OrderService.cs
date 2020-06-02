using OSS.Business.DTOs;
using OSS.Data;
using OSS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSS.Business.Services
{
    public class OrderService
    {
        public IEnumerable<OrderDto> GetAllByUser(UserDto user)
        {
            if (!user.IsValid())
            {
                throw new ArgumentException("Invalid user.");
            }

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var orders = unitOfWork.OrderRepository.GetAll(o => o.User.Id == user.Id);

                var result = orders.Select(o => new OrderDto
                {
                    Id = o.Id,
                    User = new UserDto
                    {
                        Id = o.User.Id,
                        Name = o.User.Name,
                        Age = o.User.Age,
                        BankBalance = o.User.BankBalance,
                        CreatedOn = o.User.CreatedOn,
                        UpdatedOn = o.User.UpdatedOn
                    },
                    Product = new ProductDto
                    {
                        Id = o.Product.Id,
                        Name = o.Product.Name,
                        Price = o.Product.Price,
                        Quantity = o.Product.Quantity,
                        Seller = new UserDto
                        {
                            Id = o.Product.Seller.Id,
                            Name = o.Product.Seller.Name,
                            Age = o.Product.Seller.Age,
                            BankBalance = o.Product.Seller.BankBalance,
                            CreatedOn = o.Product.Seller.CreatedOn,
                            UpdatedOn = o.Product.Seller.UpdatedOn
                        },
                        Description = o.Product.Description,
                        CreatedOn = o.Product.CreatedOn,
                        UpdatedOn = o.Product.UpdatedOn
                    },
                    Quantity = o.Quantity,
                    TotalPrice = o.TotalPrice,
                    Remarks = o.Remarks,
                    CreatedOn = o.CreatedOn,
                    UpdatedOn = o.UpdatedOn
                }).ToList();

                return result;
            }
        }

        public IEnumerable<OrderDto> GetAll()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var orders = unitOfWork.OrderRepository.GetAll();

                var result = orders.Select(o => new OrderDto
                {
                    Id = o.Id,
                    User = new UserDto
                    {
                        Id = o.User.Id,
                        Name = o.User.Name,
                        Age = o.User.Age,
                        BankBalance = o.User.BankBalance,
                        CreatedOn = o.User.CreatedOn,
                        UpdatedOn = o.User.UpdatedOn
                    },
                    Product = new ProductDto
                    {
                        Id = o.Product.Id,
                        Name = o.Product.Name,
                        Price = o.Product.Price,
                        Quantity = o.Product.Quantity,
                        Seller = new UserDto
                        {
                            Id = o.Product.Seller.Id,
                            Name = o.Product.Seller.Name,
                            Age = o.Product.Seller.Age,
                            BankBalance = o.Product.Seller.BankBalance,
                            CreatedOn = o.Product.Seller.CreatedOn,
                            UpdatedOn = o.Product.Seller.UpdatedOn
                        },
                        Description = o.Product.Description,
                        CreatedOn = o.Product.CreatedOn,
                        UpdatedOn = o.Product.UpdatedOn
                    },
                    Quantity = o.Quantity,
                    TotalPrice = o.TotalPrice,
                    Remarks = o.Remarks,
                    CreatedOn = o.CreatedOn,
                    UpdatedOn = o.UpdatedOn
                }).ToList();

                return result;
            }
        }

        public OrderDto GetById(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("Invalid order id.");
            }

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var order = unitOfWork.OrderRepository.GetById(id);

                if (order == null)
                {
                    return null;
                }

                OrderDto orderDto = new OrderDto
                {
                    Id = order.Id,
                    User = new UserDto
                    {
                        Id = order.User.Id,
                        Name = order.User.Name,
                        Age = order.User.Age,
                        BankBalance = order.User.BankBalance,
                        CreatedOn = order.User.CreatedOn,
                        UpdatedOn = order.User.UpdatedOn
                    },
                    Product = new ProductDto
                    {
                        Id = order.Product.Id,
                        Name = order.Product.Name,
                        Price = order.Product.Price,
                        Quantity = order.Product.Quantity,
                        Seller = new UserDto
                        {
                            Id = order.Product.Seller.Id,
                            Name = order.Product.Seller.Name,
                            Age = order.Product.Seller.Age,
                            BankBalance = order.Product.Seller.BankBalance,
                            CreatedOn = order.Product.Seller.CreatedOn,
                            UpdatedOn = order.Product.Seller.UpdatedOn
                        },
                        Description = order.Product.Description,
                        CreatedOn = order.Product.CreatedOn,
                        UpdatedOn = order.Product.UpdatedOn
                    },
                    Quantity = order.Quantity,
                    TotalPrice = order.TotalPrice,
                    Remarks = order.Remarks,
                    CreatedOn = order.CreatedOn,
                    UpdatedOn = order.UpdatedOn
                };

                return orderDto;
            }
        }

        public bool Create(OrderDto orderDto)
        {
            if (!orderDto.IsValid())
            {
                throw new ArgumentException("Invalid order.");
            }

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var user = unitOfWork.UserRepository.GetById(orderDto.User.Id);
                var product = unitOfWork.ProductRepository.GetById(orderDto.Product.Id);

                if (user == null || product == null)
                {
                    return false;
                }

                if (product.Quantity < orderDto.Quantity)
                {
                    return false;
                }

                var order = new Order()
                {
                    Id = orderDto.Id,
                    User = user,
                    Product = product,
                    Quantity = orderDto.Quantity,
                    Remarks = orderDto.Remarks,
                    CreatedOn = DateTime.Now
                };

                product.Quantity -= order.Quantity;
                order.TotalPrice = order.Quantity * product.Price;

                unitOfWork.OrderRepository.Add(order);

                return unitOfWork.Save();
            }
        }

        public bool Update(OrderDto orderDto)
        {
            if (!orderDto.IsValid())
            {
                throw new ArgumentException("Invalid order.");
            }

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var order = unitOfWork.OrderRepository.GetById(orderDto.Id);
                var user = unitOfWork.UserRepository.GetById(orderDto.User.Id);
                var product = unitOfWork.ProductRepository.GetById(orderDto.Product.Id);

                if (order == null || user == null || product == null)
                {
                    return false;
                }

                if (product.Quantity < orderDto.Quantity)
                {
                    return false;
                }

                order.Id = orderDto.Id;
                order.User = user;
                order.Product = product;
                order.Quantity = orderDto.Quantity;
                order.TotalPrice = orderDto.Quantity * product.Price;
                order.Remarks = orderDto.Remarks;
                order.UpdatedOn = DateTime.Now;

                unitOfWork.OrderRepository.Update(order);

                return unitOfWork.Save();
            }
        }

        public bool Delete(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("Invalid order id.");
            }

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                Order order = unitOfWork.OrderRepository.GetById(id);

                if (order == null)
                {
                    return false;
                }

                unitOfWork.OrderRepository.Delete(order);

                return unitOfWork.Save();
            }
        }
    }
}
