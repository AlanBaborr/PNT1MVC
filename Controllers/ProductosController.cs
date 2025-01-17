﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarritoMVC.Data;
using CarritoMVC.Models;
using System.Security.Policy;

namespace CarritoMVC.Controllers
{
    public class ProductosController : Controller
    {
        private readonly CarritoContext _context;

        public ProductosController(CarritoContext context)
        {
            _context = context;
        }

        // GET: Productos
        public ActionResult Index()
        {
            if (Login())
            {
                ViewBag.productos = _context.Productos.Include(p => p.Categoria);
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        public ActionResult ProductosXCategoria(int id)
        {
            ViewBag.categoria = _context.Categorias.Where(c => c.CategoriaId == id);
            ViewBag.productosXCategoria = _context.Productos.Include(p => p.Categoria).Where(p => p.CategoriaId.Equals(id));
            ViewBag.categorias = _context.Categorias.ToList();

            return View();
        }

        // GET: Productos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productos/Create
        public IActionResult Create()
        {
            if (Login())
            {
                ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Descripcion");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        // POST: Productos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoriaId,Imagen,Nombre,Descripcion,PrecioVigente,Activo,Destacado,Cantidad")] Producto producto)
        {
            if (Login())
            {
                if (ModelState.IsValid)
                {
                    _context.Add(producto);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Descripcion", producto.CategoriaId);
                return View(producto);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
           
        }

       

        // GET: Productos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (Login())
            {
                if (id == null || _context.Productos == null)
                {
                    return NotFound();
                }

                var producto = await _context.Productos.FindAsync(id);
                if (producto == null)
                {
                    return NotFound();
                }
                ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Descripcion", producto.CategoriaId);
                return View(producto);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

           
        }

        // POST: Productos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductoId,CategoriaId,Imagen,Nombre,Descripcion,PrecioVigente,Activo,Destacado,Cantidad")] Producto producto)
        {
            if (Login())
            {
                if (id != producto.ProductoId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(producto);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductoExists(producto.ProductoId))
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
                ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "Descripcion", producto.CategoriaId);
                return View(producto);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        // GET: Productos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (Login())
            {
                if (id == null || _context.Productos == null)
                {
                    return NotFound();
                }

                var producto = await _context.Productos
                    .Include(p => p.Categoria)
                    .FirstOrDefaultAsync(m => m.ProductoId == id);
                if (producto == null)
                {
                    return NotFound();
                }

                return View(producto);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
           
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (Login())
            {
                if (_context.Productos == null)
                {
                    return Problem("Entity set 'CarritoContext.Productos'  is null.");
                }
                var producto = await _context.Productos.FindAsync(id);
                if (producto != null)
                {
                    _context.Productos.Remove(producto);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        private bool ProductoExists(int id)
        {
          return _context.Productos.Any(e => e.ProductoId == id);
        }

        public bool Login()
        {
            bool l;
            if (HttpContext.Session.GetString("EmpleadoId") != null && HttpContext.Session.GetString("Admin") == true.ToString())
            {
                l = true;
            }
            else
            {
                l = false;
            }
            return l;
        }
    }
}
