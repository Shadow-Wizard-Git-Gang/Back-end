using AutoMapper;
using MongoDB.Driver;
using VC.Helpers.Exceptions;
using VC.Models;
using VC.Models.DTOs.CompanyDTOs;
using VC.Repositories.IRepositories;
using VC.Services.IServices;

namespace VC.Services
{
    public class CompanyService : ICompanyService
    {
        private ICompanyRepository _companyRepository { get; }
        private IMapper _mapper { get; }

        public CompanyService(
            ICompanyRepository companyRepository,
            IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task<Company> CreateCompanyAsync(CompanyCreateRequestDTO createRequest)
        {
            var company = _mapper.Map<Company>(createRequest);

            company.Employees = new List<string>();

            await _companyRepository.CreateCompanyAsync(company);

            return company;
        }

        public async Task<Company> GetCompanyAsync(string id)
        {
            var company = await GetCompany(id);
            return company;
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync(int page, int limit)
        {
            var companies = await _companyRepository.GetCompaniesAsync(page, limit);
            return companies;
        }

        public async Task<Company> UpdateCompanyAsync(string id, CompanyUpdateRequestDTO createRequest)
        {
            var company = await GetCompany(id);

            _mapper.Map(createRequest, company);
            
            await _companyRepository.UpdateCompanyAsync(company);

            return company;
        }

        public async Task DeleteCompanyAsync(string id)
        {
            await GetCompany(id);

            await _companyRepository.DeleteCompanyAsync(id);
        }

        //helper methods

        private async Task<Company> GetCompany(string id)
        {
            var company = await _companyRepository.GetCompanyAsync(id);

            if (company == null)
            {
                throw new UserNotFoundException("User not found");
            }

            return company;
        }
    }
}
