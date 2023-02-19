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
    public class ModelController: Controller
    {
        private readonly IModelRepository _modelRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IPhoneRepository _phoneRepository;
        private readonly IMapper _mapper;

        public ModelController(
            IModelRepository modelRepository,
            ICompanyRepository companyRepository,
            IPhoneRepository phoneRepository,
            IMapper mapper)
        {
            _modelRepository = modelRepository;
            _companyRepository = companyRepository;
            _phoneRepository = phoneRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ModelDto>))]
        public IActionResult GetModels()
        {
            var models = _mapper.Map<List<ModelDto>>(_modelRepository.GetModels());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(models);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ModelDto))]
        [ProducesResponseType(400)]
        public IActionResult GetModel(int id)
        {
            if (!_modelRepository.ModelExists(id))
                return NotFound();

            var model = _mapper.Map<ModelDto>(_modelRepository.GetModel(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(model);
        }

        [HttpGet("{id}/phones")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PhoneDto>))]
        public IActionResult GetPhonesFromModel(int id)
        {
            var phones = _mapper.Map<List<PhoneDto>>(_modelRepository.GetPhonesFromModel(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(phones);
        }

        [HttpGet("{id}/company")]
        [ProducesResponseType(200, Type = typeof(CompanyDto))]
        public IActionResult GetCompanyByModel(int id)
        {
            var company = _mapper.Map<CompanyDto>(_modelRepository.GetCompanyByModel(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(company);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateModel([FromQuery]int companyId, [FromBody] ModelDto modelDto)
        {
            if (modelDto == null)
                return BadRequest(ModelState);
            if (!_companyRepository.CompanyExists(companyId))
            {
                ModelState.AddModelError("", "Company doesn't exist!");
                return BadRequest(ModelState);
            }

            var model = _modelRepository.GetModels()
                .Where(c => c.Name.Trim().ToUpper() == modelDto.Name.Trim().ToUpper())
                .FirstOrDefault();

            if (model != null)
            {
                ModelState.AddModelError("", "Model already exists.");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var modelMap = _mapper.Map<Model>(modelDto);

            modelMap.Company = _companyRepository.GetCompanyById(companyId);

            if (!_modelRepository.CreateModel(modelMap))
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
        public IActionResult UpdateModel(int id, [FromBody] ModelDto modelDto)
        {
            if (modelDto == null)
                return BadRequest(ModelState);

            if (id != modelDto.Id)
                return BadRequest(ModelState);

            if (!_modelRepository.ModelExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var modelMap = _mapper.Map<Model>(modelDto);

            if (!_modelRepository.UpdateModel(modelMap))
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
        public IActionResult DeleteModel(int id)
        {
            if (!_modelRepository.ModelExists(id))
                return NotFound();

            var phonesOfModel = _modelRepository.GetPhonesFromModel(id);
            var model = _modelRepository.GetModel(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_phoneRepository.DeletePhones(phonesOfModel.ToList()))
            {
                ModelState.AddModelError("", "Deleting is failed");
                return StatusCode(500, ModelState);
            }

            if (!_modelRepository.DeleteModel(model))
            {
                ModelState.AddModelError("", "Deleting is failed");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }

    }
}
