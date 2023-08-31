using Microsoft.AspNetCore.Mvc;
using VC.Data;
using VC.Models.DTOs.CompanyDTOs;
using VC.Services.IServices;

namespace VC.Controllers
{
    [Route(SettingsStorage.RouteNameForCompanyController)]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private ICompanyService _companyService { get; }

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateCompanyAsync([FromBody] CompanyCreateRequestDTO createRequest)
        {
            var response = await _companyService.CreateCompanyAsync(createRequest);
            return CreatedAtAction(nameof(GetCompanyAsync), new { id = response.Id }, response);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetCompanyAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetCompanyAsync(string id)
        {
            var response = await _companyService.GetCompanyAsync(id);
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetCompaniesAsync(int page, int limit)
        {
            if (page <= 0 || limit <= 0)
            {
                return BadRequest("Page and limit must be greater than zero.");
            }

            var response = await _companyService.GetCompaniesAsync(page, limit);
            return Ok(response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateCompanyAsync(string id, [FromBody] CompanyUpdateRequestDTO updateRequest)
        {
            var response = await _companyService.UpdateCompanyAsync(id, updateRequest);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteCompanyAsync(string id)
        {
            await _companyService.DeleteCompanyAsync(id);
            return NoContent();
        }
    }
}
