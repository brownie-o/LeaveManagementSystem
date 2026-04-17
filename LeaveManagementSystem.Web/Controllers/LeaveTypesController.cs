using LeaveManagementSystem.Web.Models.LeaveTypes;
using LeaveManagementSystem.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Controllers
{
    public class LeaveTypesController(ILeaveTypeService _leaveTypeService) : Controller
    {
        //// dependency injection of the database context to access the database
        //private readonly ApplicationDbContext _context; // _context is the db connection
        //private readonly IMapper _mapper;
        private const string NameExistsValidationMessage = "This leave type already exists in the database";

        //public LeaveTypesController(ApplicationDbContext context, IMapper mapper) 
        //{
        //    _context = context;
        //    this._mapper = mapper;
        //}

        // GET: LeaveTypes
        public async Task<IActionResult> Index()
        {
            var viewData = await _leaveTypeService.GetAll();
            // return the view model to the view
            return View(viewData);
        }

        // GET: LeaveTypes/Details/5
        public async Task<IActionResult> Details(int? id) // [id] == asp-route-[id] in Index.cshtml
        {
            if (id == null)
            {
                return NotFound();
            }

            // Parameterization (User input is NOT directly concatenated into SQL strings. Instead, it is passed as a separate parameter)
            // - key for preventing SQL injection (EF core translates LINQ query into SQL)
            // Select * from LeaveTypes WHERE Id = [id]

            //var leaveType = await _context.LeaveTypes
            //    .FirstOrDefaultAsync(m => m.Id == id);

            var leaveType = await _leaveTypeService.Get<LeaveTypeReadOnlyVM>(id.Value);

            if (leaveType == null)
            {
                return NotFound();
            }

            //var viewData = _mapper.Map<LeaveTypeReadOnlyVM>(leaveType);

            return View(leaveType);
        }

        // GET: LeaveTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LeaveTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken] // protect against cross-site request forgery (CSRF) attacks
        public async Task<IActionResult> Create(LeaveTypeCreateVM leaveTypeCreate)
        {
            // adding custom validation and model state error
            if (await _leaveTypeService.CheckIfLeaveTypeNameExists(leaveTypeCreate.Name))
            {
                ModelState.AddModelError(nameof(leaveTypeCreate.Name), NameExistsValidationMessage);
            }

            if (ModelState.IsValid)
            {
                await _leaveTypeService.Create(leaveTypeCreate);
                return RedirectToAction(nameof(Index));
            }
            return View(leaveTypeCreate); // if the model state is not valid, return the view(Create.cshtml) with the current data(leaveTypeCreate) to show validation errors
        }

        // GET: LeaveTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var leaveType = await _context.LeaveTypes.FindAsync(id);
            var leaveType = await _leaveTypeService.Get<LeaveTypeEditVM>(id.Value);

            if (leaveType == null)
            {
                return NotFound();
            }

            // map the data model(leaveType) to the view model (LeaveTypeEditVM) for the Edit view
            //var viewData = _mapper.Map<LeaveTypeEditVM>(leaveType); 

            return View(leaveType);
        }

        // POST: LeaveTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveTypeEditVM leaveTypeEdit)
        {
            if (id != leaveTypeEdit.Id)
            {
                return NotFound();
            }

            if (await _leaveTypeService.CheckIfLeaveTypeNameExistsForEdit(leaveTypeEdit))
            {
                ModelState.AddModelError(nameof(leaveTypeEdit.Name), NameExistsValidationMessage);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _leaveTypeService.Edit(leaveTypeEdit);
                }
                catch (DbUpdateConcurrencyException) // handle concurrency issues (e.g., when multiple users try to edit the same record at the same time)
                {
                    if (!_leaveTypeService.LeaveTypeExists(leaveTypeEdit.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(leaveTypeEdit);
        }

        // GET: LeaveTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var leaveType = await _context.LeaveTypes
            //    .FirstOrDefaultAsync(m => m.Id == id);

            var leaveType = await _leaveTypeService.Get<LeaveTypeReadOnlyVM>(id.Value);

            if (leaveType == null)
            {
                return NotFound();
            }

            //var viewData = _mapper.Map<LeaveTypeReadOnlyVM>(leaveType);

            return View(leaveType);
        }

        // POST: LeaveTypes/Delete/5
        [HttpPost]
        // override the action name for the method (rename DeleteConfirmed to Delete)
        [ActionName("Delete")] // to be called from the form in Delete.cshtml by Delete(asp-action="Delete"), to ensure the action name is same as the route
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _leaveTypeService.Remove(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
