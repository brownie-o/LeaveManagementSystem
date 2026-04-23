using AutoMapper;
using LeaveManagementSystem.Web.Models.LeaveAllocations;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations;

public class LeaveAllocationsService(ApplicationDbContext _context, IHttpContextAccessor _httpContextAccessor, UserManager<ApplicationUser> _userManager, IMapper _mapper) : ILeaveAllocationsService
{
    public async Task AllocateLeave(string employeeId)
    {
        // get all the leave types that have no allocations for the employee
        var leaveTypes = await _context.LeaveTypes
            .Where(q => !q.LeaveAllocations.Any(x => x.EmployeeId == employeeId))
            .ToListAsync();

        // get the current period based on the year
        var currentDate = DateTime.Now;
        var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);
        var monthsRemaining = period.EndDate.Month - currentDate.Month;

        // calculate leave based on number of months left in the period

        // foreach leave type, create an allocation entry
        foreach (var leaveType in leaveTypes)
        {
            // Works, but not the best practice. Instead of checking if the allocation ex
            //var allocationExists = await AllocationExists(employeeId, period.Id, leaveType.Id);
            //if (allocationExists)
            //{
            //    continue; // skip to the next leave type if allocation already exists
            //}

            var accrualRate = decimal.Divide(leaveType.NumberOfDays, 12);
            var leaveAllocation = new LeaveAllocation
            {
                EmployeeId = employeeId,
                LeaveTypeId = leaveType.Id, // deal with the foreign key(leaveTypeId) instead of the object(leaveType)
                PeriodId = period.Id,
                Days = (int)Math.Ceiling(accrualRate * monthsRemaining),
            };

            _context.Add(leaveAllocation); // track the changes in the context, but not yet save to the database
        }

        await _context.SaveChangesAsync();
    }

    public async Task<EmployeeAllocationVM> GetEmployeeAllocations(string? userId)
    {
        var user = string.IsNullOrEmpty(userId) ? await GetUser() : await _userManager.FindByIdAsync(userId);

        var allocations = await GetAllocations(user.Id);
        var allocationVmList = _mapper.Map<List<LeaveAllocation>, List<LeaveAllocationVM>>(allocations);
        var leaveTypesCount = await _context.LeaveTypes.CountAsync();

        var employeeVm = new EmployeeAllocationVM
        {
            DateOfBirth = user.DateOfBirth,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Id = user.Id,
            LeaveAllocations = allocationVmList,
            IsCompletedAllocation = leaveTypesCount == allocations.Count
        };

        return employeeVm;
    }

    public async Task<List<EmployeeListVM>> GetEmployees()
    {
        var users = await _userManager.GetUsersInRoleAsync(Roles.Employee);
        // _mapper.Map<Source, Destination>(data): take a list of ApplicationUser objs and convert them to a list of EmployeeListVM objs
        // (users.ToList()): converts the IList to List
        var employees = _mapper.Map<List<ApplicationUser>, List<EmployeeListVM>>(users.ToList());

        return employees; 
    }

    public async Task<LeaveAllocationEditVM> GetEmployeeAllocation(int allocationId)
    {
        var allocation = await _context.LeaveAllocations
            .Include(q => q.LeaveType)
            .Include(q => q.Employee)
            .FirstOrDefaultAsync(q => q.Id == allocationId);

        var model = _mapper.Map<LeaveAllocationEditVM>(allocation);

        return model;
    }

    public async Task EditAllocation(LeaveAllocationEditVM allocationEditVM)
    {
        //var leaveAllocation = await GetEmployeeAllocation(allocationEditVM.Id);
        //if (leaveAllocation == null)
        //{
        //    throw new Exception("Leave allocation record does not exist.");
        //}
        //leaveAllocation.Days = allocationEditVM.Days;

        //option 1 _context.Update(leaveAllocation); => write a whole update query to update every field of the leaveAllocation
        //option 2 _context.Entry(leaveAllocation).State = EntityState.Modified; => only look at the fields that have been modified and update those fields in the database
        // await _context.SaveChangesAsync(); => save the changes to the database

        //option 3 => execute an update query directly in the database without loading the entity into memory, which is more efficient for updating a single field
        await _context.LeaveAllocations
            .Where(q => q.Id == allocationEditVM.Id)
            .ExecuteUpdateAsync(s => s.SetProperty(e => e.Days, allocationEditVM.Days));
    }

    private async Task<ApplicationUser> GetUser()
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
        //if (user == null)
        //{
        //    throw new UnauthorizedAccessException("User not found");
        //}
        return user;
    }
    private async Task<List<LeaveAllocation>> GetAllocations(string? userId)
    {
        var currentDate = DateTime.Now;

        var leaveAllocations = await _context.LeaveAllocations
            .Include(q => q.LeaveType) // eager loading of the related LeaveType entity
            .Include(q => q.Period)
            .Where(q => q.EmployeeId == userId && q.Period.EndDate.Year == currentDate.Year)
            .ToListAsync();

        return leaveAllocations;
    }

    private async Task<bool> AllocationExists(string userId, int periodId, int LeaveTypeId)
    {
        var exists = await _context.LeaveAllocations.AnyAsync(q => 
            q.EmployeeId == userId 
            && q.LeaveTypeId == LeaveTypeId
            && q.PeriodId == periodId
        );

        return exists;
    }
}
