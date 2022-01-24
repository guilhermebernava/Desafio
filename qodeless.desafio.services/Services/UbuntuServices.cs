using qodeless.desafio.domain.Entities;
using qodeless.desafio.domain.Interfaces;
using qodeless.desafio.domain.Validators;
using qodeless.desafio.services.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace qodeless.desafio.services.Services
{
    public class UbuntuServices : IUbuntuServices
    {
        private IUbuntuRepository _ubuntuRepository;
        public UbuntuServices(IUbuntuRepository ubuntuRepository)
        {
            _ubuntuRepository = ubuntuRepository;
        }
        #region CRUD
        public bool Add(Ubuntu vm)
        {
            var validationResult = new UbuntuAddValidator().Validate(vm);
            if (validationResult.IsValid)
            {
                _ubuntuRepository.Add(vm);
                return true;
            }
            return false;
        }

        public void Dispose() => GC.SuppressFinalize(this);

        public IEnumerable<Ubuntu> GetAll() => _ubuntuRepository.GetAll();


        public Ubuntu GetById(Guid id) => _ubuntuRepository.GetById(id);

        public bool Remove(Guid id)
        {
            _ubuntuRepository.ForceDelete(id);
            return true;
        }

        public bool Update(Ubuntu vm)
        {
            var validationResult = new UbuntuUpdateValidator().Validate(vm);
            if (validationResult.IsValid)
            {
                _ubuntuRepository.Update(vm);
                return true;
            }
            return false;
        }

        public String sha256_hash(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
        #endregion
    }
}
