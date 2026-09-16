
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BudgetBook.Models;

[Authorize]
public class TransactionsController : Controller
{
    private readonly BudgetBookContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public TransactionsController(BudgetBookContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: TRANSACTIONS
    public async Task<IActionResult> Index()    
    {
        var userId = _userManager.GetUserId(User);
        return View(await _context.Transaction
            .Where(t => t.UserId == userId)
            .ToListAsync());
    }

    // GET: TRANSACTIONS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);
        var transaction = await _context.Transaction
            .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
        if (transaction == null)
        {
            return NotFound();
        }

        return View(transaction);
    }

    // GET: TRANSACTIONS/Create
    public IActionResult Create()
    {
        ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
        return View();
    }

    // POST: TRANSACTIONS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Type,Amount,BookingDate,CreatedAt,Description,CategoryId")] Transaction transaction)
    {
        transaction.UserId = _userManager.GetUserId(User)!;
        ModelState.Remove(nameof(Transaction.UserId));
        ModelState.Remove(nameof(Transaction.User));
        if (ModelState.IsValid)
        {
            _context.Add(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", transaction.CategoryId);
        return View(transaction);
    }

    // GET: TRANSACTIONS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);
        var transaction = await _context.Transaction.FindAsync(id);
        if (transaction == null || transaction.UserId != userId)
        {
            return NotFound();
        }
        ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", transaction.CategoryId);
        return View(transaction);
    }

    // POST: TRANSACTIONS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Type,Amount,BookingDate,CreatedAt,Description,CategoryId")] Transaction transaction)
    {
        if (id != transaction.Id)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);
        if (!await _context.Transaction.AnyAsync(t => t.Id == id && t.UserId == userId))
        {
            return NotFound();
        }

        transaction.UserId = userId!;
        ModelState.Remove(nameof(Transaction.UserId));
        ModelState.Remove(nameof(Transaction.User));
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(transaction);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(transaction.Id))
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
        ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", transaction.CategoryId);
        return View(transaction);
    }

    // GET: TRANSACTIONS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);
        var transaction = await _context.Transaction
            .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
        if (transaction == null)
        {
            return NotFound();
        }

        return View(transaction);
    }

    // POST: TRANSACTIONS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var userId = _userManager.GetUserId(User);
        var transaction = await _context.Transaction
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (transaction != null)
        {
            _context.Transaction.Remove(transaction);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TransactionExists(int? id)
    {
        return _context.Transaction.Any(e => e.Id == id);
    }
}
