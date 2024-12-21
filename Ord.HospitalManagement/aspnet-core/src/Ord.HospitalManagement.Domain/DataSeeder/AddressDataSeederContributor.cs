using Ord.HospitalManagement.DomainServices;
using Ord.HospitalManagement.Entities.Address;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Ord.HospitalManagement.DataSeeder
{
    public class AddressDataSeederContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Province, int> _provinceRepository;
        private readonly IGenerateCode _generateCode;

        public AddressDataSeederContributor(IRepository<Province, int> provinceRepository, IGenerateCode generateCode)
        {
            _provinceRepository = provinceRepository;
            _generateCode = generateCode;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _provinceRepository.GetCountAsync() <= 0)
            {
                await _provinceRepository.InsertAsync(new Province
                {
                    Code = _generateCode.AutoGenerateCode(PrefixGencode.PrefixGencode.PROV),
                    Name = "Province 1"
                }, autoSave: true);
                //
                await _provinceRepository.InsertAsync(new Province
                {
                    Code = _generateCode.AutoGenerateCode(PrefixGencode.PrefixGencode.PROV),
                    Name = "Province 2"
                }, autoSave: true);
            }

            
        }
    }
}
