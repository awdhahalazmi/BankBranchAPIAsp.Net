using BankBranchAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankBranchAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly BankContext _context;

        public BankController(BankContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BankBranchResponse>>> GetAll()
        {
            return  _context.Branches.Select(b => new BankBranchResponse
            {
                branchManager = b.BranchManager,
                location = b.Location,
                name = b.Name

            }).ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<BankBranchResponse> Details(int id)
        {
            var bank = _context.Branches.Find(id);
          
           
            if (bank == null)
            {
                return NotFound();
            }
            return new BankBranchResponse
            {
                branchManager = bank.BranchManager,
                location = bank.Location,
                name = bank.Name
            };
        }

       

        [HttpPost("AddBranch")]
        public async Task<ActionResult<BankBranch>> AddBranch(AddBankRequest branch)
        {
            try
            {
                var newbank = new BankBranch();
                newbank.Name = branch.Name;
                newbank.Location = branch.Location;
                newbank.BranchManager = branch.BankManager;


                _context.Branches.Add(newbank);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAll), new { id = newbank.Id }, branch);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding branch: {ex.Message}");
            }
        }

        [HttpPatch("{id}")]
        public IActionResult Edit(int id, AddBankRequest req)
        {
            var bank = _context.Branches.Find(id);
            bank.Name = req.Name;
            bank.BranchManager = req.BankManager;
            bank.Location = req.Location;
            _context.SaveChanges();

            return Created(nameof(Details), new { id = bank.Id });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var bank = _context.Branches.Find(id);
            _context.Branches.Remove(bank);
            _context.SaveChanges();

            return Ok();
        }
    }
}

