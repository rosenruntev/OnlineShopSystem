using Castle.Core.Internal;
using OSS.Business.DTOs;
using OSS.Data;
using OSS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OSS.Business.Services
{
    public class UserService
    {
        public IEnumerable<UserDto> GetAllByName(string name)
        {
            if (name.IsNullOrEmpty())
            {
                throw new ArgumentException("Invalid user name.");
            }

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var users = unitOfWork.UserRepository.GetAll(u => u.Name == name);

                var result = users.Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Age = u.Age,
                    BankBalance = u.BankBalance,
                    CreatedOn = u.CreatedOn,
                    UpdatedOn = u.UpdatedOn
                });

                return result;
            }
        }

        public IEnumerable<UserDto> GetAll()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var users = unitOfWork.UserRepository.GetAll();

                var result = users.Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Age = u.Age,
                    BankBalance = u.BankBalance,
                    CreatedOn = u.CreatedOn,
                    UpdatedOn = u.UpdatedOn
                });

                return result;
            }
        }

        public UserDto GetById(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("Invalid user id.");
            }

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var user = unitOfWork.UserRepository.GetById(id);

                if (user == null)
                {
                    return null;
                }
                
                UserDto userDto = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Age = user.Age,
                    BankBalance = user.BankBalance,
                    CreatedOn = user.CreatedOn,
                    UpdatedOn = user.UpdatedOn
                };

                return userDto;
            }
        }

        public bool Create(UserDto userDto)
        {
            if (!userDto.IsValid())
            {
                throw new ArgumentException("Invalid user.");
            }

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var user = new User()
                {
                    Id = userDto.Id,
                    Name = userDto.Name,
                    Age = userDto.Age,
                    BankBalance = userDto.BankBalance,
                    CreatedOn = DateTime.Now
                };

                unitOfWork.UserRepository.Add(user);

                return unitOfWork.Save();
            }
        }

        public bool Update(UserDto userDto)
        {
            if (!userDto.IsValid())
            {
                throw new ArgumentException("Invalid user.");
            }

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var user = unitOfWork.UserRepository.GetById(userDto.Id);

                if (user == null)
                {
                    return false;
                }

                user.Id = userDto.Id;
                user.Name = userDto.Name;
                user.Age = userDto.Age;
                user.BankBalance = userDto.BankBalance;
                user.UpdatedOn = DateTime.Now;

                unitOfWork.UserRepository.Update(user);

                return unitOfWork.Save();
            }
        }

        public bool Delete(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("Invalid user id.");
            }

            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                User user = unitOfWork.UserRepository.GetById(id);

                if (user == null)
                {
                    return false;
                }

                unitOfWork.UserRepository.Delete(user);

                return unitOfWork.Save();
            }
        }
    }
}
