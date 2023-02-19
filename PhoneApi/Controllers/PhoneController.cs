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
    public class PhoneController : Controller
    {
        private readonly IPhoneRepository _phoneRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public PhoneController(
            IPhoneRepository phoneRepository,
            IModelRepository modelRepository,
            IReviewRepository reviewRepository,
            IMapper mapper)
        {
            _phoneRepository = phoneRepository;
            _modelRepository = modelRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PhoneDto>))]
        public IActionResult GetPhones()
        {
            var phones = _mapper.Map<List<PhoneDto>>(_phoneRepository.GetPhones());

            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            return Ok(phones);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(PhoneDto))]
        [ProducesResponseType(400)]
        public IActionResult GetPhone(int id)
        {
            if(!_phoneRepository.PhoneExists(id))
                return NotFound();

            var phone = _mapper.Map<PhoneDto>(_phoneRepository.GetPhone(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(phone);
        }

        

        [HttpGet("{id}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetPhoneRating(int id)
        {
            if (!_phoneRepository.PhoneExists(id))
                return NotFound();

            var rating = _phoneRepository.GetPhoneRating(id);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(rating);
        }


        [HttpGet("{id}/categories")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetCategoriesOfPhone(int id)
        {
            if (!_phoneRepository.PhoneExists(id))
                return NotFound();

            var categories = _mapper.Map<List<CategoryDto>>(_phoneRepository.GetCategoriesOfPhone(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(categories);
        }

        [HttpGet("{id}/reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsOfPhone(int id)
        {
            if (!_phoneRepository.PhoneExists(id))
                return NotFound();

            var reviews = _mapper.Map<List<ReviewDto>>(_phoneRepository.GetReviewsOfPhone(id));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePhone([FromQuery] int modelId,[FromQuery] int categoryId, [FromBody] PhoneDto phoneDto)
        {
            if (phoneDto == null)
                return BadRequest(ModelState);

            var phone = _phoneRepository.GetPhones()
                .Where(c => c.Name.Trim().ToUpper() == phoneDto.Name.Trim().ToUpper())
                .FirstOrDefault();

            if (phone != null)
            {
                ModelState.AddModelError("", "Phone already exists.");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var phoneMap = _mapper.Map<Phone>(phoneDto);

            phoneMap.Model = _modelRepository.GetModel(modelId);

            if (!_phoneRepository.CreatePhone(categoryId, phoneMap))
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
        public IActionResult UpdatePhone(int id,[FromQuery] int categoryId, [FromBody] PhoneDto phoneDto)
        {
            if (phoneDto == null)
                return BadRequest(ModelState);

            if (id != phoneDto.Id)
                return BadRequest(ModelState);

            if (!_phoneRepository.PhoneExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var phoneMap = _mapper.Map<Phone>(phoneDto);
            //my way
            //phoneMap.PhoneCategories = _phoneRepository.GetPhone(id).PhoneCategories.ToList();

            if (!_phoneRepository.UpdatePhone(categoryId,phoneMap))
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
        public IActionResult DeletePhone(int id)
        {
            if (!_phoneRepository.PhoneExists(id))
                return NotFound();

            var reviewsOfPhone = _phoneRepository.GetReviewsOfPhone(id);
            var phone = _phoneRepository.GetPhone(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReviews(reviewsOfPhone.ToList()))
            {
                ModelState.AddModelError("", "Deleting reviews of phone is failed");
                return StatusCode(500, ModelState);
            }

            if (!_phoneRepository.DeletePhone(phone))
            {
                ModelState.AddModelError("", "Deleting is failed");
                return StatusCode(500, ModelState);
            }
            return NoContent();

        }

    }
}
