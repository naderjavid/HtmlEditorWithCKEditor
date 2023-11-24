using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HtmlEditor.DB;
using HtmlEditor.Entities;
using HtmlEditor.Models;

namespace HtmlEditor.Controllers
{
    public class HtmlPagesController : Controller
    {
        private readonly EditorDbContext _context;

        public HtmlPagesController(EditorDbContext context)
        {
            _context = context;
        }

        // GET: HtmlPages
        public async Task<IActionResult> Index()
        {
            var htmlPages = await _context
                .HtmlPags
                .Select(p => new HtmlPageDto
                {
                    Id = p.Id,
                    HtmlCode = p.HtmlCode,
                    CreatedAt = p.CreatedAt,
                })
                .ToListAsync();
              return View(htmlPages);
        }

        // GET: HtmlPages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HtmlPags == null)
            {
                return NotFound();
            }

            var htmlPage = await _context.HtmlPags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (htmlPage == null)
            {
                return NotFound();
            }

            var htmlPageDto = new HtmlPageDto
            {
                Id = htmlPage.Id,
                HtmlCode = htmlPage.HtmlCode,
                CreatedAt = htmlPage.CreatedAt,
            };
            return View(htmlPageDto);
        }

        // GET: HtmlPages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HtmlPages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HtmlCode")] CreateHtmlPage input)
        {
            HtmlPageDto result = new HtmlPageDto();
            if (ModelState.IsValid)
            {
                var htmlPage =
                    new HtmlPage
                    {
                        HtmlCode = input.HtmlCode,
                        CreatedAt = DateTime.Now
                    };   
                _context.Add(htmlPage);
                await _context.SaveChangesAsync();

                result.Id = htmlPage.Id;
                result.HtmlCode = htmlPage.HtmlCode;
                result.CreatedAt = htmlPage.CreatedAt;

                return RedirectToAction(nameof(Index));
            }
            return View(result);
        }

        // GET: HtmlPages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HtmlPags == null)
            {
                return NotFound();
            }

            var htmlPage = await _context.HtmlPags.FindAsync(id);
            if (htmlPage == null)
            {
                return NotFound();
            }

            var htmlPageDto = new UpdateHtmlPage
            {
                Id = htmlPage.Id,
                HtmlCode = htmlPage.HtmlCode,
            };
            return View(htmlPageDto);
        }

        // POST: HtmlPages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,HtmlCode")] UpdateHtmlPage input)
        {
            if (id != input.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!HtmlPageExists(input.Id))
                {
                    return NotFound();
                }
                try
                {
                    var htmlPage = _context.HtmlPags.First(p => p.Id == input.Id);
                    htmlPage.HtmlCode = input.HtmlCode;
                    _context.Update(htmlPage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(input);
        }

        // GET: HtmlPages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HtmlPags == null)
            {
                return NotFound();
            }

            var htmlPage = await _context.HtmlPags
                .FirstOrDefaultAsync(m => m.Id == id);
            if (htmlPage == null)
            {
                return NotFound();
            }

            var htmlPageDto = new HtmlPageDto
            {
                Id = htmlPage.Id,
                HtmlCode = htmlPage.HtmlCode,
                CreatedAt = htmlPage.CreatedAt,
            };

            return View(htmlPageDto);
        }

        // POST: HtmlPages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HtmlPags == null)
            {
                return Problem("Entity set 'EditorDbContext.HtmlPags'  is null.");
            }
            var htmlPage = await _context.HtmlPags.FindAsync(id);
            if (htmlPage != null)
            {
                _context.HtmlPags.Remove(htmlPage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HtmlPageExists(int id)
        {
          return (_context.HtmlPags?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
