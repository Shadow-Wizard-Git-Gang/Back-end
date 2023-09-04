using MongoDB.Driver;
using MongoDbGenericRepository;
using VC.Models;
using VC.Repositories.IRepositories;

namespace VC.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private IMongoDbContext _mongoDb { get; }
        private IMongoCollection<Company> _companies { get; }

        public CompanyRepository(IMongoDbContext mongoDb)
        {
            _mongoDb = mongoDb;
            _companies = _mongoDb.GetCollection<Company>();
        }

        public async Task CreateCompanyAsync(Company company)
        {
            await _companies.InsertOneAsync(company);
        }

        public async Task<Company> GetCompanyAsync(string id)
        {
            var result = await _companies.Find(e => e.Id == id).FirstOrDefaultAsync();

            return result;
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync(int page, int limit)
        {
            int startIndex = (page - 1) * limit;

            var result = await _companies
               .Find(e => true)
               .Skip(startIndex)
               .Limit(limit)
               .ToListAsync();

            return result;
        }

        public async Task UpdateCompanyAsync(Company company)
        {
            var updateSettings = Builders<Company>.Update
                .Set(e => e.Name, company.Name)
                .Set(e => e.Employees, company.Employees);

            await _companies.UpdateOneAsync(e => e.Id == company.Id, updateSettings);
        }

        public async Task DeleteCompanyAsync(string id)
        {
            await _companies.DeleteOneAsync(e => e.Id == id);
        }
    }
}
