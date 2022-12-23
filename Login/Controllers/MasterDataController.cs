using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Login.Data;
using Login.Models;

namespace Login.Controllers
{
    public class MasterDataController : Controller
    {
        private readonly DataContext _context;

        public MasterDataController(DataContext context)
        {
            _context = context;
        }

        // GET: MasterData
        public async Task<IActionResult> Index()
        {
              return View(await _context.Master_Data.ToListAsync());
        }

        // GET: MasterData/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Master_Data == null)
            {
                return NotFound();
            }

            var master_Data = await _context.Master_Data
                .FirstOrDefaultAsync(m => m.PERSONID == id);
            if (master_Data == null)
            {
                return NotFound();
            }

            return View(master_Data);
        }

        // GET: MasterData/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MasterData/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PERSONID,POPULATIONREGISTERNO,TAXREGISTERNO,PASSPORTNO,KITAS,FULL_NAME,PHONE,GENDER,EMAIL,BIRTHDATE,HIGHSCHOOL,UNIVERSITY,UNIVERSITYMAJOR,MSSCHOOL,MSMAJOR,PHDSCHOOL,PHDMAJOR,POSITION,COUNTRYCODE,PROVINCECODE,ADDRESS,CITY,POSTALCODE,BUSINESS_REG_NO,OCCUPATION,LOC_ASING,NASIONALITY,DateCreated")] Master_Data master_Data)
        {
            if (ModelState.IsValid)
            {
                _context.Add(master_Data);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(master_Data);
        }

        // GET: MasterData/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Master_Data == null)
            {
                return NotFound();
            }

            var master_Data = await _context.Master_Data.FindAsync(id);
            if (master_Data == null)
            {
                return NotFound();
            }
            return View(master_Data);
        }

        // POST: MasterData/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PERSONID,POPULATIONREGISTERNO,TAXREGISTERNO,PASSPORTNO,KITAS,FULL_NAME,PHONE,GENDER,EMAIL,BIRTHDATE,HIGHSCHOOL,UNIVERSITY,UNIVERSITYMAJOR,MSSCHOOL,MSMAJOR,PHDSCHOOL,PHDMAJOR,POSITION,COUNTRYCODE,PROVINCECODE,ADDRESS,CITY,POSTALCODE,BUSINESS_REG_NO,OCCUPATION,LOC_ASING,NASIONALITY,DateCreated")] Master_Data master_Data)
        {
            if (id != master_Data.PERSONID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(master_Data);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Master_DataExists(master_Data.PERSONID))
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
            return View(master_Data);
        }

        // GET: MasterData/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Master_Data == null)
            {
                return NotFound();
            }

            var master_Data = await _context.Master_Data
                .FirstOrDefaultAsync(m => m.PERSONID == id);
            if (master_Data == null)
            {
                return NotFound();
            }

            return View(master_Data);
        }

        // POST: MasterData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Master_Data == null)
            {
                return Problem("Entity set 'DataContext.Master_Data'  is null.");
            }
            var master_Data = await _context.Master_Data.FindAsync(id);
            if (master_Data != null)
            {
                _context.Master_Data.Remove(master_Data);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Master_DataExists(int id)
        {
          return _context.Master_Data.Any(e => e.PERSONID == id);
        }

    }
}
