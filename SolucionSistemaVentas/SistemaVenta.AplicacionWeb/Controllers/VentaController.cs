﻿using Microsoft.AspNetCore.Mvc;

using AutoMapper;
using SistemaVenta.AplicacionWeb.Models.ViewModels;
using SistemaVenta.AplicacionWeb.Utilidades.Response;
using SistemaVenta.BLL.Interfaces;
using SistemaVenta.Entity;
using NuGet.Protocol.Plugins;

namespace SistemaVenta.AplicacionWeb.Controllers
{
    public class VentaController : Controller
    {

        private readonly IVentaService _ventaServicio;
        private readonly IMapper _mapper;

        public VentaController(IVentaService ventaServicio, IMapper mapper)
        {
            _ventaServicio = ventaServicio;
            _mapper = mapper;
        }

        public IActionResult NuevaVenta()
        {
            return View();
        }

        public IActionResult HistorialVenta()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerProductos(string busqueda)
        {
            List<VMProducto> vmListaProductos = _mapper.Map<List<VMProducto>>(await _ventaServicio.ObtenerProductos(busqueda));
            return StatusCode(StatusCodes.Status200OK, vmListaProductos);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarVenta([FromBody] VMVenta modelo)
        {
            GenericResponse<VMVenta>gResponse = new GenericResponse<VMVenta>();

            try
            {
                modelo.IdUsuario = 1;

                Venta venta_creada = await _ventaServicio.Registrar(_mapper.Map<Venta>(modelo));
                modelo = _mapper.Map<VMVenta>(venta_creada);

                gResponse.Estado = true;
                gResponse.Objeto = modelo;
            }
            catch(Exception ex)
            {

                gResponse.Estado = true;
                gResponse.Mensaje = ex.Message;
            }

            return StatusCode(StatusCodes.Status200OK, gResponse);
        }

        [HttpGet]
        public async Task<IActionResult> Historial(string numeroVenta, string fechaInicio,string fechaFin)
        {
            List<VMVenta> vmHistorialVenta = _mapper.Map<List<VMVenta>>(await _ventaServicio.Historial(numeroVenta, fechaInicio,fechaFin));

            return StatusCode(StatusCodes.Status200OK, vmHistorialVenta);
        }

    }
}
