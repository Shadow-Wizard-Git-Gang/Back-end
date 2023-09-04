using VC.Models;

namespace VC.Repositories.IRepositories
{
    public interface ICompanyRepository
    {
        public Task CreateCompanyAsync(Company company);
        public Task<Company> GetCompanyAsync(string id);
        public Task<IEnumerable<Company>> GetCompaniesAsync(int page, int limit);
        public Task UpdateCompanyAsync(Company company);
        public Task DeleteCompanyAsync(string id);

    }
}
