using qodeless.desafio.domain.Entities;
using qodeless.desafio.domain.Interfaces;
using qodeless.desafio.Infra.CrossCutting.identity.Data;

namespace qodeless.desafio.crosscutting.identity.Repositories
{
    public class UbuntuRepository : Repository<Ubuntu>, IUbuntuRepository
    {
        public UbuntuRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
