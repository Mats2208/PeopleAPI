using Microsoft.AspNetCore.Mvc;
using PeopleAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace MiApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonasController : ControllerBase
    {
        // "Base de datos" en memoria (se pierde al reiniciar)
        private static readonly List<Persona> _personas = new();
        private static int _nextId = 1;

        // GET /api/personas -> listar todo
        [HttpGet]
        public ActionResult<List<Persona>> GetAll()
        {
            return Ok(_personas.OrderBy(p => p.Id).ToList());
        }

        // GET /api/personas/{id} -> obtener por id
        [HttpGet("{id:int}")]
        public ActionResult<Persona> GetById(int id)
        {
            var persona = _personas.FirstOrDefault(p => p.Id == id);
            if (persona == null) return NotFound(new { message = "No encontrada" });
            return Ok(persona);
        }

        // POST /api/personas -> crear
        [HttpPost]
        public ActionResult<Persona> Create([FromBody] Persona data)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(data.Name))
                return BadRequest(new { message = "Name es requerido." });

            if (data.Age < 0 || data.Age > 130)
                return BadRequest(new { message = "Age debe estar entre 0 y 130." });

            var nueva = new Persona
            {
                Id = _nextId++,
                Name = data.Name.Trim(),
                Age = data.Age,
                LastName = string.IsNullOrWhiteSpace(data.LastName) ? null : data.LastName.Trim()
            };

            _personas.Add(nueva);
            return CreatedAtAction(nameof(GetById), new { id = nueva.Id }, nueva);
        }

        // PUT /api/personas/{id} -> actualizar (reemplazo completo)
        [HttpPut("{id:int}")]
        public ActionResult<Persona> Update(int id, [FromBody] Persona data)
        {
            var persona = _personas.FirstOrDefault(p => p.Id == id);
            if (persona == null) return NotFound(new { message = "No encontrada" });

            if (string.IsNullOrWhiteSpace(data.Name))
                return BadRequest(new { message = "Name es requerido." });

            if (data.Age < 0 || data.Age > 130)
                return BadRequest(new { message = "Age debe estar entre 0 y 130." });

            persona.Name = data.Name.Trim();
            persona.Age = data.Age;
            persona.LastName = string.IsNullOrWhiteSpace(data.LastName) ? null : data.LastName.Trim();

            return Ok(persona);
        }

        // DELETE /api/personas/{id} -> eliminar
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var persona = _personas.FirstOrDefault(p => p.Id == id);
            if (persona == null) return NotFound(new { message = "No encontrada" });

            _personas.Remove(persona);
            return NoContent();
        }
    }
}
