using CourseWorkApp.Data;
using CourseWorkApp.Data.DTOs;
using CourseWorkApp.Data.Entity;
using CourseWorkApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWorkApp.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index(string sortBy, string currentFilter, string search, int? page)
        {
            ViewData["IdSortParm"] = string.IsNullOrEmpty(sortBy) ? "id_desc" : sortBy == "id" ? "id_desc" : "id";
            ViewData["CountSortParm"] = sortBy == "count" ? "count_desc" : "count";
            ViewData["DateSortParm"] = sortBy == "date" ? "date_desc" : "date";
            ViewData["SearchFilter"] = search;

            if (!string.IsNullOrEmpty(search))
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }

            var invoices = from x in _context.Invoices select x;

            if (!string.IsNullOrEmpty(search))
            {
                if (int.TryParse(search, out int id))
                {
                    invoices = invoices.Where(x => x.InvoiceId == id);
                }
                else
                {
                    invoices = invoices.Where(x =>
                        x.Stock.Name.ToLower().Contains(search.ToLower())
                        || x.Client.Name.ToLower().Contains(search.ToLower()));
                }

            }

            switch (sortBy)
            {
                case "id":
                    invoices = invoices.OrderBy(x => x.InvoiceId);
                    break;
                case "id_desc":
                    invoices = invoices.OrderByDescending(x => x.InvoiceId);
                    break;
                case "count":
                    invoices = invoices.OrderBy(s => s.Count);
                    break;
                case "count_desc":
                    invoices = invoices.OrderByDescending(s => s.Count);
                    break;
                case "date":
                    invoices = invoices.OrderBy(s => s.CreateDateTime);
                    break;
                case "date_desc":
                    invoices = invoices.OrderByDescending(s => s.CreateDateTime);
                    break;
                default:
                    invoices = invoices.OrderByDescending(s => s.InvoiceId);
                    break;
            }

            int pageSize = 10;

            var results = invoices
                .Include(x => x.Client)
                .Include(x => x.Stock)
                .AsNoTracking();

            return View(await PaginatedListModel<Invoice>.CreateAsync(results, page ?? 1, pageSize));
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context
                .Invoices
                .Include(x => x.Client)
                .Include(x => x.Stock)
                .FirstOrDefaultAsync(m => m.InvoiceId == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        public async Task<IActionResult> Create()
        {
            ViewData["Clients"] = await _context.Clients.ToListAsync();
            ViewData["Stocks"] = await _context.Stocks.ToListAsync();
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InvoiceDTO invoiceDto)
        {
            var client = await _context.Clients.FindAsync(invoiceDto.ClientId);
            if (client == null)
            {
                return NotFound();
            }

            var stock = await _context.Stocks.FindAsync(invoiceDto.StockId);
            if (stock == null)
            {
                return NotFound();
            }

            var invoice = new Invoice
            {
                Client = client,
                Stock = stock,
                Count = invoiceDto.Count,
                CreateDateTime = DateTime.Now,
            };

            if (ModelState.IsValid)
            {
                _context.Add(invoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context
                .Invoices
                .Include(x => x.Client)
                .Include(x => x.Stock)
                .FirstAsync(x => x.InvoiceId == id);
            if (invoice == null)
            {
                return NotFound();
            }

            ViewData["Clients"] = await _context.Clients.ToListAsync();
            ViewData["Stocks"] = await _context.Stocks.ToListAsync();

            return View(new InvoiceDTO
            {
                InvoiceId = invoice.InvoiceId,
                ClientId = invoice.Client.ClientId,
                StockId = invoice.Stock.StockId,
                Count = invoice.Count
            });
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, InvoiceDTO invoiceDto)
        {
            if (id != invoiceDto.InvoiceId)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.FindAsync(invoiceDto.InvoiceId);
            if (invoice == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(invoiceDto.ClientId);
            if (client == null)
            {
                return NotFound();
            }

            var stock = await _context.Stocks.FindAsync(invoiceDto.StockId);
            if (stock == null)
            {
                return NotFound();
            }
            invoice.Client = client;
            invoice.Stock = stock;
            invoice.Count = invoiceDto.Count;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.InvoiceId))
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
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context
                .Invoices
                .Include(x => x.Client)
                .Include(x => x.Stock)
                .FirstOrDefaultAsync(m => m.InvoiceId == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.InvoiceId == id);
        }
    }
}
