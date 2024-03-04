using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practica20240304.Models;

namespace Practica20240304.Controllers
{
    public class FacturasController : Controller
    {
        private readonly Practica20240304Context _context;

        public FacturasController(Practica20240304Context context)
        {
            _context = context;
        }

        // GET: Facturas
        public async Task<IActionResult> Index()
        {
              return _context.Facturas != null ? 
                          View(await _context.Facturas.ToListAsync()) :
                          Problem("Entity set 'Practica20240304Context.Facturas'  is null.");
        }

        // GET: Facturas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Facturas == null)
            {
                return NotFound();
            }

            var factura = await _context.Facturas
                .Include(s=> s.DetFacturas)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (factura == null)
            {
                return NotFound();
            }

            return View(factura);
        }

        // GET: Facturas/Create
        public IActionResult Create()
        {
            var factura=new Factura();
            factura.Fecha = DateTime.Now;
            factura.DetFacturas=new List<DetFactura>();
            factura.DetFacturas.Add(new DetFactura
            {
                Cantidad = 1
            });
            ViewBag.Accion = "Create";
            return View(factura);
        }

        // POST: Facturas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Correlativo,Fecha,DetFacturas")] Factura factura)
        {
            if (ModelState.IsValid)
            {
                _context.Add(factura);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(factura);
        }
        [HttpPost]
        public ActionResult AgregarDetalles([Bind("Id,Correlativo,Fecha,DetFacturas")] Factura factura, string accion)
        {
            factura.DetFacturas.Add(new DetFactura { Cantidad = 1 });
            ViewBag.Accion = accion;
            return View(accion, factura);
        }
        public ActionResult EliminarDetalles([Bind("Id,Correlativo,Fecha,DetFacturas") ] Factura factura,
            int index, string accion)
        {
            var det = factura.DetFacturas[index];
            if(accion=="Edit" && det.Id>0)
            {
                det.Id = det.Id * -1;
            }
            else
            {
                factura.DetFacturas.RemoveAt(index);
            }
            
            ViewBag.Accion = accion;
            return View(accion, factura);
        }
        // GET: Facturas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Facturas == null)
            {
                return NotFound();
            }

            var factura = await _context.Facturas
                .Include(s => s.DetFacturas)
                .FirstAsync(s => s.Id == id);
            if (factura == null)
            {
                return NotFound();
            }
            ViewBag.Accion = "Edit";
            return View(factura);
        }

        // POST: Facturas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Correlativo,Fecha,DetFacturas")] Factura factura)
        {
            if (id != factura.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var facturaUpdate= await _context.Facturas
                        .Include(s=> s.DetFacturas)
                        .FirstAsync(s=> s.Id==factura.Id);
                    var detNew = factura.DetFacturas.Where(s => s.Id == 0);
                    foreach(var d in detNew)
                    {
                        facturaUpdate.DetFacturas.Add(d);
                    }                    
                    var detUpdate = factura.DetFacturas.Where(s => s.Id > 0);
                    foreach(var d in detUpdate)
                    {
                        var det= facturaUpdate.DetFacturas.FirstOrDefault(s=> s.Id== d.Id);
                        det.Cantidad = d.Cantidad;
                        det.Precio = d.Precio;
                        det.Producto = d.Producto;
                    }
                    var delDet = factura.DetFacturas.Where(s=> s.Id<0);
                    foreach (var d in delDet)
                    {
                        d.Id = d.Id * -1;
                        var det = facturaUpdate.DetFacturas.FirstOrDefault(s => s.Id == d.Id);
                        facturaUpdate.DetFacturas.Remove(det);
                    }
                    _context.Update(facturaUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacturaExists(factura.Id))
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
            return View(factura);
        }

        // GET: Facturas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Facturas == null)
            {
                return NotFound();
            }

            var factura = await _context.Facturas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (factura == null)
            {
                return NotFound();
            }

            return View(factura);
        }

        // POST: Facturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Facturas == null)
            {
                return Problem("Entity set 'Practica20240304Context.Facturas'  is null.");
            }
            var factura = await _context.Facturas.FindAsync(id);
            if (factura != null)
            {
                _context.Facturas.Remove(factura);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacturaExists(int id)
        {
          return (_context.Facturas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
