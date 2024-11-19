using Microsoft.AspNetCore.Mvc;
using CrudApi.Models;
using CrudApi.Services;
using CrudApi.Dtos;

namespace CrudApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KaryawanController : ControllerBase
{
    private readonly IKaryawanService _karyawanService;
    private readonly ILogger<KaryawanController> _logger;

    public KaryawanController(IKaryawanService karyawanService, ILogger<KaryawanController> logger)
    {
        _karyawanService = karyawanService;
        _logger = logger;
    }
    private KaryawanDto MapToDto(Karyawan karyawan)
    {
        return new KaryawanDto
        {
            Nik = karyawan.Nik,
            Name = karyawan.Name,
            TanggalLahir = karyawan.TanggalLahir,
            Alamat = karyawan.Alamat,
            Negara = karyawan.Negara,
            JenisKelamin = karyawan.JenisKelamin
        };
    }

    // GET: api/karyawan
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Karyawan>>> GetKaryawan()
    {
        try
        {
            var karyawan = await _karyawanService.GetAllKaryawanAsync();
            return Ok(new
            {
                success = true,
                message = "Data retrieved successfully",
                data = karyawan
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving karyawan data");
            return StatusCode(500, new
            {
                success = false,
                message = "Error retrieving data",
                error = ex.Message
            });
        }
    }

    // GET: api/karyawan/{nik}
    [HttpGet("{nik}")]
    [ProducesResponseType(typeof(Karyawan), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Karyawan>> GetKaryawanByNik(string nik)
    {
        try
        {
            var karyawan = await _karyawanService.GetKaryawanByNikAsync(nik);
            if (karyawan == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"Karyawan with NIK {nik} not found"
                });
            }

            return Ok(new
            {
                success = true,
                message = "Data retrieved successfully",
                data = karyawan
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving karyawan with NIK {Nik}", nik);
            return StatusCode(500, new
            {
                success = false,
                message = "Error retrieving data",
                error = ex.Message
            });
        }
    }

    // POST: api/karyawan
    [HttpPost]
    [ProducesResponseType(typeof(KaryawanDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<KaryawanDto>> CreateKaryawan(KaryawanCreateDto karyawanDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid model state",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            var karyawan = new Karyawan
            {
                Nik = karyawanDto.Nik,
                Name = karyawanDto.Name,
                TanggalLahir = karyawanDto.TanggalLahir,
                Alamat = karyawanDto.Alamat,
                Negara = karyawanDto.Negara,
                JenisKelamin = karyawanDto.JenisKelamin,
                UserId = karyawanDto.UserId
            };

            var createdKaryawan = await _karyawanService.CreateKaryawanAsync(karyawan);
            var responseDto = MapToDto(createdKaryawan);
            
            return CreatedAtAction(
                nameof(GetKaryawanByNik), 
                new { nik = responseDto.Nik }, 
                new { 
                    success = true,
                    message = "Karyawan created successfully",
                    data = responseDto 
                }
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating karyawan");
            return StatusCode(500, new
            {
                success = false,
                message = "Error creating karyawan",
                error = ex.Message
            });
        }
    }

    // PUT: api/karyawan/{nik}
    [HttpPut("{nik}")]
    [ProducesResponseType(typeof(Karyawan), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateKaryawan(string nik, Karyawan karyawan)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid model state",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            var updatedKaryawan = await _karyawanService.UpdateKaryawanAsync(nik, karyawan);
            if (updatedKaryawan == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"Karyawan with NIK {nik} not found"
                });
            }

            return Ok(new
            {
                success = true,
                message = "Karyawan updated successfully",
                data = updatedKaryawan
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating karyawan with NIK {Nik}", nik);
            return StatusCode(500, new
            {
                success = false,
                message = "Error updating karyawan",
                error = ex.Message
            });
        }
    }

    // DELETE: api/karyawan/{nik}
    [HttpDelete("{nik}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteKaryawan(string nik)
    {
        try
        {
            var result = await _karyawanService.DeleteKaryawanAsync(nik);
            if (!result)
            {
                return NotFound(new
                {
                    success = false,
                    message = $"Karyawan with NIK {nik} not found"
                });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting karyawan with NIK {Nik}", nik);
            return StatusCode(500, new
            {
                success = false,
                message = "Error deleting karyawan",
                error = ex.Message
            });
        }
    }
}