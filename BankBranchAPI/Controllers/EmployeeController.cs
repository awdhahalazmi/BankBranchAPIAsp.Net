using BankBranchAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        [Authorize]
        public async Task<ActionResult<PageListResult<AddEmployeeForm>>> GetAll(int page = 1, string searchTerm ="", bool isAscending = true)
        {

            if (isAscending)
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    return _context.Employees
                        .Where(c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                         .OrderBy(c => c.Id)
                .Select(b => new AddEmployeeForm

                {
                    Name = b.Name,
                    Position = b.Position,
                    CivilId = b.CivilId,
                    BankId = b.BankBranch.Id,
                    Id = b.Id

                }).ToPageList(page, 1);
                }

                return _context.Employees
                     .OrderBy(c => c.Id)
                .Select(b => new AddEmployeeForm

                {
                    Name = b.Name,
                    Position = b.Position,
                    CivilId = b.CivilId,
                    BankId = b.BankBranch.Id,
                    Id = b.Id

                }).ToPageList(page, 1);
            
        }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                return _context.Employees
                    .Where(c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                  .OrderByDescending(c => c.Id)
            .Select(b => new AddEmployeeForm

            {
                Name = b.Name,
                Position = b.Position,
                CivilId = b.CivilId,
                BankId = b.BankBranch.Id,
                Id = b.Id

            }).ToPageList(page, 1);
            }

            return _context.Employees
                                  .OrderByDescending(c => c.Id)


            .Select(b => new AddEmployeeForm


            {
                Name = b.Name,
                Position = b.Position,
                CivilId = b.CivilId,
                BankId = b.BankBranch.Id,
                Id = b.Id

            }).ToPageList(page, 1);
        }

            [HttpGet("{id}")]
            [Authorize]
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
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

