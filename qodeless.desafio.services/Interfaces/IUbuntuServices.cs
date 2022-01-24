using qodeless.desafio.domain.Entities;
using System;
using System.Collections.Generic;

namespace qodeless.desafio.services.Interfaces
{
    public interface IUbuntuServices : IDisposable
    {
        IEnumerable<Ubuntu> GetAll();
        Ubuntu GetById(Guid id);
        bool Add(Ubuntu vm);
        bool Update(Ubuntu vm);
        bool Remove(Guid id);
        String sha256_hash(string value);


    }
}
