using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PhoneApi.Dto;
using PhoneApi.Interfaces;
using PhoneApi.Models;
using PhoneApi.Repository;

namespace PhoneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController: Controller
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IMapper _mapper;

        public CompanyController(
            ICompanyRepository companyRepository,
            IModelRepository modelRepository,
            IMapper mapper)
        {
            _companyRepository = companyRepository;
            _modelRepository = modelRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CompanyDto>))]
        public IActionResult GetCompanies()
        {
            var companies = _mapper.Map<List<CompanyDto>>(_companyRepository.GetCompanies());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(companies);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(CompanyDto))]
        [ProducesResponseType(400)]
        public IActionResult GetCompany(int id)
        {
            if (!_companyRepository.CompanyExists(id))
                return NotFound();

            var company = _mapper.Map<CompanyDto>(_companyRepository.GetCompanyById(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(company);
        }

        
        [HttpGet("{id}/models")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ModelDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetModelsFromCompany(int id)
        {
            var models = _mapper.Map<List<ModelDto>>(_companyRepository.GetModelsFromCompany(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(models);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCompany([FromBody] CompanyDto companyDto)
        {
            if (companyDto == null)
                return BadRequest(ModelState);

            var company = _companyRepository.GetCompanies()
                .Where(c => c.Name.Trim().ToUpper() == companyDto.Name.Trim().ToUpper())
                .FirstOrDefault();

            if (company != null)
            {
                ModelState.AddModelError("", "Company already exists.");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var companyMap = _mapper.Map<Company>(companyDto);

            if (!_companyRepository.CreateCompany(companyMap))
            {
                ModelState.AddModelError("", "Saving failed.");
                return StatusCode(500, ModelState);
            }
            return Ok("Successfully created");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCompany(int id, [FromBody] CompanyDto companyDto)
        {
            if (companyDto == null)
                return BadRequest(ModelState);

            if (id != companyDto.Id)
                return BadRequest(ModelState);

            if (!_companyRepository.CompanyExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var companyMap = _mapper.Map<Company>(companyDto);

            if (!_companyRepository.UpdateCompany(companyMap))
            {
                ModelState.AddModelError("", "Updating is failed!");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCompany(int id)
        {
            if (!_companyRepository.CompanyExists(id))
                return NotFound();

            var modelsOfCompany = _companyRepository.GetModelsFromCompany(id);
            var company = _companyRepository.GetCompanyById(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_modelRepository.DeleteModels(modelsOfCompany.ToList()))
            {
                ModelState.AddModelError("", "Deleting models of company is failed");
                return StatusCode(500, ModelState);
            }

            if (!_companyRepository.DeleteCompany(company))
            {
                ModelState.AddModelError("", "Deleting is failed");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }
    }
}
