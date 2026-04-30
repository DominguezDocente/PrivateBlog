using PrivateBlog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Domain.Entities.Users
{
    public class User
    {
        public string Id { get; set; }
        public string FisrtName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PhoneNumber { get; set; }

        private User()
        {            
        }

        public static User Reconstitute(string id, string firstName, string lastName, string userName, string email, bool emailConfirmed, string? phoneNumber)
        {
            ValideteId(id);
            ValideteNames(firstName, lastName);
            ValideteEmail(email);

            return new User
            {
                Id = id,
                FisrtName = firstName,
                LastName = lastName,
                UserName = userName,
                Email = email,
                EmailConfirmed = emailConfirmed,
                PhoneNumber = phoneNumber
            };
        }

        private static void ValideteId(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) 
            {
                throw new BussinesRuleException("El id es requerido");
            }
        }

        private static void ValideteNames(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new BussinesRuleException("El nombre es requerido");
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new BussinesRuleException("El apellido es requerido");
            }

            if (firstName.Length > 64)
            {
                throw new BussinesRuleException("El nombre no puede tener más de 64 caracteres");
            }

            if (lastName.Length > 64)
            {
                throw new BussinesRuleException("El apellido no puede tener más de 64 caracteres");
            }
        }

        private static void ValideteEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new BussinesRuleException("El correo electrónico es requerido");
            }

            // TODO: Validar el formato del correo electrónico
        }
    }
}
