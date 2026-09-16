using BudgetBook.Models;
using BudgetBook.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        var transactions = await _context.Transaction
            .Include(t => t.Category)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.BookingDate)
            .ToListAsync();

        return View(transactions);
    }

    // GET: TRANSACTIONS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var transaction = await FindOwnTransactionAsync(id.Value);
        if (transaction == null)
        {
            return NotFound();
        }

        return View(transaction);
    }

    // GET: TRANSACTIONS/Create
    public async Task<IActionResult> Create()
    {
        var viewModel = new TransactionFormViewModel
        {
            BookingDate = DateTime.Today,
            Categories = await GetCategorySelectListAsync()
        };

        return View(viewModel);
    }

    // POST: TRANSACTIONS/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TransactionFormViewModel viewModel)
    {
        await ValidateCategoryAsync(viewModel.CategoryId, viewModel.Type);

        if (!ModelState.IsValid)
        {
            viewModel.Categories = await GetCategorySelectListAsync(viewModel.CategoryId);
            return View(viewModel);
        }

        var transaction = new Transaction
        {
            Type = viewModel.Type,
            Amount = viewModel.Amount,
            BookingDate = viewModel.BookingDate!.Value,
            CreatedAt = DateTime.Now,
            Description = viewModel.Description,
            CategoryId = viewModel.CategoryId,
            UserId = _userManager.GetUserId(User)!
        };

        _context.Add(transaction);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: TRANSACTIONS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var transaction = await FindOwnTransactionAsync(id.Value);
        if (transaction == null)
        {
            return NotFound();
        }

        var viewModel = new TransactionFormViewModel
        {
            Id = transaction.Id,
            Type = transaction.Type,
            Amount = transaction.Amount,
            BookingDate = transaction.BookingDate,
            Description = transaction.Description,
            CategoryId = transaction.CategoryId,
            Categories = await GetCategorySelectListAsync(transaction.CategoryId)
        };

        return View(viewModel);
    }

    // POST: TRANSACTIONS/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, TransactionFormViewModel viewModel)
    {
        if (id == null || id != viewModel.Id)
        {
            return NotFound();
        }

        var transaction = await _context.Transaction
            .FirstOrDefaultAsync(t => t.Id == id.Value && t.UserId == _userManager.GetUserId(User));
        if (transaction == null)
        {
            return NotFound();
        }

        await ValidateCategoryAsync(viewModel.CategoryId, viewModel.Type);

        if (!ModelState.IsValid)
        {
            viewModel.Categories = await GetCategorySelectListAsync(viewModel.CategoryId);
            return View(viewModel);
        }

        transaction.Type = viewModel.Type;
        transaction.Amount = viewModel.Amount;
        transaction.BookingDate = viewModel.BookingDate!.Value;
        transaction.Description = viewModel.Description;
        transaction.CategoryId = viewModel.CategoryId;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await TransactionExistsAsync(transaction.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: TRANSACTIONS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var transaction = await FindOwnTransactionAsync(id.Value);
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
        if (id == null)
        {
            return NotFound();
        }

        var transaction = await _context.Transaction
            .FirstOrDefaultAsync(t => t.Id == id.Value && t.UserId == _userManager.GetUserId(User));
        if (transaction == null)
        {
            return NotFound();
        }

        _context.Transaction.Remove(transaction);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<Transaction?> FindOwnTransactionAsync(int id)
    {
        return await _context.Transaction
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == _userManager.GetUserId(User));
    }

    private async Task<IEnumerable<SelectListItem>> GetCategorySelectListAsync(int? selectedCategoryId = null)
    {
        return await _context.Category
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ThenBy(c => c.Type)
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name + " - " + c.Type,
                Selected = selectedCategoryId.HasValue && c.Id == selectedCategoryId.Value
            })
            .ToListAsync();
    }

    private async Task ValidateCategoryAsync(int categoryId, string type)
    {
        var category = await _context.Category.FirstOrDefaultAsync(c => c.Id == categoryId && c.IsActive);
        if (category == null)
        {
            ModelState.AddModelError(nameof(TransactionFormViewModel.CategoryId), "Bitte eine gueltige Kategorie auswaehlen.");
            return;
        }

        if (category.Type != type)
        {
            ModelState.AddModelError(nameof(TransactionFormViewModel.CategoryId), "Die Kategorie muss zum Typ passen.");
        }
    }

    private async Task<bool> TransactionExistsAsync(int id)
    {
        return await _context.Transaction.AnyAsync(e => e.Id == id);
    }
}
