using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task2.DataContext;
using Task2.DataContext.Utilities;

namespace Task2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DataController : ControllerBase
{
    private readonly AppDbContext _db;
    public DataController(AppDbContext db)
    {
        _db = db;
    }
    [HttpGet("ex1")]
    public async Task<IActionResult> Exercise1([FromQuery] string district, [FromQuery] int from, [FromQuery] int to)
    {
        return Ok(await _db.Estates
            .Where(e => e.District.Name == district &&
                from < e.Price && e.Price < to)
            .OrderByDescending(x => x.Price)
            .Select(x => new
            {
                Address = x.Address,
                Area = x.Area,
                Floor = x.Floor
            })
            .ToListAsync());
    }
    [HttpGet("ex2")]
    public async Task<IActionResult> Exercise2([FromQuery] int roomsCount)
    {
        return Ok(await _db.Realtors
            .Where(x => x.Sales
                .Where(s => s.Estate.RoomsNumber == roomsCount)
                .Count() != 0)
            .Select(x => new
            {
                Firstname = x.Firstname,
                Surname = x.Surname,
                Lastname = x.Lastname
            })
            .ToListAsync());
    }
    [HttpGet("ex3")]
    public async Task<IActionResult> Exercise3([FromQuery] string district, [FromQuery] int roomsCount)
    {
        return Ok(await _db.Estates
            .Where(x => x.District.Name == district &&
                x.RoomsNumber == roomsCount)
            .SumAsync(x => x.Price));
    }
    [HttpGet("ex4")]
    public async Task<IActionResult> Exercise4([FromQuery] string realtorSurname)
    {
        var realtor = await _db.Realtors
            .Where(x => x.Surname == realtorSurname)
            .Include(x => x.Sales)
            .FirstOrDefaultAsync();

        if (realtor is null) { return BadRequest($"Нет риэлтора {realtorSurname}"); }
        return Ok(new Dictionary<string, decimal>
        {
            {"max", realtor.Sales.MaxBy(x => x.Price)!.Price },

            {"min", realtor.Sales.MinBy(x => x.Price)!.Price}
        });
    }
    [HttpGet("ex5")]
    public async Task<IActionResult> Exercise5([FromQuery] string criteria, [FromQuery] string realtorSurame)
    {
        return Ok(await _db.Estates
            .Where(e => e.Sale != null
            && e.Sale.Realtor.Surname == realtorSurame)
            .Include(e => e.Sale)
            .Select(x => x.Scores
                .Where(s => s.Criteria.Name == criteria))
            .AverageAsync(x => x
                .Average(y => y.Value)));
    }
    [HttpGet("ex6")]
    public async Task<IActionResult> Exercise6([FromQuery] int floor)
    {
        return Ok(await _db.Districts
            .Select(x => new
            {
                DistrictName = x.Name,
                EstatesSelectedCount = x.EstatesOfDistrict
                    .Where(e => e.Floor == floor)
                    .Count()
            })
            .ToListAsync());
    }
    [HttpGet("ex7")]
    public async Task<IActionResult> Exercise7()
    {

        return Ok(await _db.Realtors
            .Include(e => e.Sales)
            .ThenInclude(e => e.Estate)
            .ThenInclude(e => e.Type)
            .Select(x => new
            {
                FCs = x.Surname + x.Firstname + x.Lastname,
                NumberOfApartmentsSold = x.Sales
                    .Where(s => s.Estate.Type.Name == "Квартира")
                    .Count()
            })
            .ToListAsync());
    }
    [HttpGet("ex8")]
    public async Task<IActionResult> Exercise8()
    {
        return Ok(await _db.Districts
            .Include(e => e.EstatesOfDistrict)
            .Select(x => new
            {
                DistrictName = x.Name,
                MostExpensiveEstates = x.EstatesOfDistrict
                    .OrderByDescending(e => e.Price)
                    .ThenBy(e => e.Floor)
                    .Take(3)
                    .Select(e => new
                    {
                        Address = e.Address,
                        Price = e.Price,
                        Floor = e.Floor
                    })
                    .ToList()
            })
            .ToListAsync());
    }
    [HttpGet("ex9")]
    public async Task<IActionResult> Exercise9([FromQuery] string realtorFCs)
    {
        var realtor = (await _db.Realtors
            .Include(x => x.Sales)
            .SingleOrDefaultAsync(x => String.Join(' ', new string[] { x.Surname, x.Firstname, x.Lastname }) == realtorFCs));
        if (realtor is null) return NotFound($"{realtorFCs} не существует");

        return Ok(realtor.Sales
            .GroupBy(x => x.SaleDate.Year)
            .Where(x => x.Count() > 2)
            .Select(x => x.First().SaleDate.Year)
            .Order()
            .ToList());
    }
    [HttpGet("ex10")]
    public async Task<IActionResult> Exercise10()
    {
        return Ok(await _db.Estates
            .GroupBy(x => x.DatePosted.Year)
            .Where(x => 2 <= x.Count() && x.Count() <= 3)
            .Select(x => x.First().DatePosted.Year)
            .ToListAsync());
    }
    [HttpGet("ex11")]
    public async Task<IActionResult> Exercise11()
    {
        return Ok(await _db.Estates
            .Where(x => x.Sale != null && x.Sale.Price / x.Price <= 0.2m)
            .Select(x => new
            {
                Address = x.Address,
                District = x.District.Name
            })
            .ToListAsync());
    }
    [HttpGet("ex12")]
    public async Task<IActionResult> Exercise12()
    {
        return Ok((await _db.Estates
            .GroupBy(x => x.District.Name)
            .Select(area => area
                .Where(estate => estate.Price / (decimal)estate.Area < area.Sum(x => x.Price) / (decimal)area.Sum(x => x.Area)))
            .ToListAsync())
            .Aggregate((prev, next) => prev.UnionBy(next, x => x.ID))
            .Select(x => x.Address));
    }
    [HttpGet("ex13")]
    public async Task<IActionResult> Exercise13([FromQuery] int year)
    {
        return Ok(await _db.Realtors
            .Include(x => x.Sales)
            .Where(x => x.Sales
                .Where(s => s.SaleDate.Year == year)
                .Count() == 0)
            .Select(x => new
            {
                Firstname = x.Firstname,
                Surname = x.Surname,
                Lastname = x.Lastname
            })
            .ToListAsync());
    }
    [HttpGet("ex14")]
    public async Task<IActionResult> Exercise14([FromQuery] int year)
    {
        return Ok(await _db.Estates
            .Include(x => x.District)
            .Include(x => x.Sale)
            .GroupBy(x => x.District.Name)
            .Select(x => new
            {
                District = x.First().District.Name,
                CurrentYearSalesNumber = x
                    .Where(e => e.Sale != null &&
                        e.Sale.SaleDate.Year == year)
                    .Count(),
                PreviousYearSalesNumber = x
                    .Where(e => e.Sale != null &&
                        e.Sale.SaleDate.Year == year - 1)
                    .Count(),
            })
            .Select(x => new
            {
                District = x.District,
                CurrentYearSalesNumber = x.CurrentYearSalesNumber,
                PreviousYearSalesNumber = x.PreviousYearSalesNumber,
                Difference = (float)(x.CurrentYearSalesNumber - x.PreviousYearSalesNumber) / x.PreviousYearSalesNumber * 100
            })
            .ToListAsync());
    }
    [HttpGet("ex15")]
    public async Task<IActionResult> Exercise15([FromQuery] int estateId)
    {
        var estate = await _db.Estates
            .Include(x => x.Scores)
            .ThenInclude(x => x.Criteria)
            .FirstOrDefaultAsync(x => x.ID == estateId);
        if (estate is null) return BadRequest($"Нет объекта с ID {estateId}");

        return Ok(estate.Scores
            .GroupBy(x => x.Criteria.Name)
            .Select(x => new
            {
                Criteria = x.First().Criteria.Name,
                AverageScore = x.Average(y => y.Value),
                TextScore = EstateScoreMapper.ToStringRepresentation(Convert.ToInt32(x.Average(y => y.Value) / 5 * 100))
            })
            .ToList());
    }
    [HttpGet("additional")]
    public async Task<IActionResult> Additional()
    {
        return Ok(await _db.Sales
            .GroupBy(x => x.SaleDate.Year)
            .Select(x => new
            {
                Year = x.First().SaleDate.Year,
                Count = x.Count()
            })
            .ToListAsync());
    }
}

