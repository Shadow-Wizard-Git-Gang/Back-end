using VC.Models;
using VC.Models.DTOs.CompanyDTOs;

namespace VC.Services.IServices
{
    public interface ICompanyService
    {
        public Task<Company> CreateCompanyAsync(CompanyCreateRequestDTO createRequest);
        public Task<Company> GetCompanyAsync(string id);
        public Task<IEnumerable<Company>> GetCompaniesAsync(int page, int limit);
        public Task<Company> UpdateCompanyAsync(string id, CompanyUpdateRequestDTO updateRequest);
        public Task DeleteCompanyAsync(string id);

    }
}
