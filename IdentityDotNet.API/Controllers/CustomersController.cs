using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityDotNet.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityDotNet.API.Controllers
{
    [Authorize]
    [Route("api/[Controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly BankContext _context;
        public CustomersController(BankContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Customer> GetCustomers()
        {
            return _context.Customers;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] long id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer([FromRoute] long id, [FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var _customer = await _context.Customers.FindAsync(id);
            if (_customer == null)
                return NotFound();

            _customer.FirstName = customer.FirstName;
            _customer.LastName = customer.LastName;

            _context.Update(_customer);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var _customer = await _context.Customers.FindAsync(customer.Id);
            if (_customer != null)
                return BadRequest(ModelState);

            await _context.AddAsync(customer);

            await _context.SaveChangesAsync();

            return Ok(customer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] long id)
        {

            var _customer = await _context.Customers.FindAsync(id);
            if (_customer == null)
                return NotFound();

            _context.Remove(_customer);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}