using BankBranchAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankBranchAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
       
            private readonly BankContext _context;

            public EmployeeController(BankContext context)
            {
                _context = context;
            }
            [HttpGet]
            public async Task<ActionResult<IEnumerable<AddEmployeeForm>>> GetAll()
            {
                return _context.Employees.Select(b => new AddEmployeeForm
                {
                    Name = b.Name,
                    Position = b.Position,
                    CivilId = b.CivilId,
                  BankId = b.BankBranch.Id,
                  Id = b.Id

                }).ToList();
            }

            [HttpGet("{id}")]
            public ActionResult<AddEmployeeForm> Details(int id)
            {
                var bankEmployee = _context.Employees.Find(id);


                if (bankEmployee == null)
                {
                    return NotFound();
                }
                return new AddEmployeeForm
                {
                    Name = bankEmployee.Name,
                    Position = bankEmployee.Position,
                    CivilId = bankEmployee.CivilId
                };
            }



        [HttpPost("AddEmployee")]
        public async Task<ActionResult<Employee>> AddEmployee(AddEmployeeForm employee)
        {
            try
            {
                var bankBranch = _context.Branches.Find(employee.BankId);

                if (bankBranch == null)
                {
                    return NotFound($"Bank branch with ID {employee.BankId} not found");
                }

                var newEmployee = new Employee
                {
                    Name = employee.Name,
                    Position = employee.Position,
                    CivilId = employee.CivilId,
                    BankBranch = bankBranch
                };

                _context.Employees.Add(newEmployee);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAll), new { id = newEmployee.Id }, employee);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding employee: {ex.Message}");
            }
        }

        [HttpPatch("{id}")]
            public IActionResult Edit(int id, AddEmployeeForm req)
            {
            try
            {
                var bankBranch = _context.Branches.Find(req.BankId);

                if (bankBranch == null)
                {
                    return NotFound($"Bank branch with ID {req.BankId} not found");
                }

                var newEmployee = new Employee
                {
                    Name = req.Name,
                    Position = req.Position,
                    CivilId = req.CivilId,
                    BankBranch = bankBranch
                };

                _context.Employees.Add(newEmployee);
                 _context.SaveChangesAsync();

                return CreatedAtAction(nameof(Details), new { id = newEmployee.Id }, req);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding employee: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
            public IActionResult Delete(int id)
            {
           // var bankBranch = _context.Branches.Find(id);

            var employee = _context.Employees.Find(id);
                _context.Employees.Remove(employee);
           //_context.Branches.Remove(employee.Id);

            _context.SaveChanges();

                return Ok();
            }
        }
    }

