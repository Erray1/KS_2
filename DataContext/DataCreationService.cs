using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Collections.Concurrent;
using System.Drawing;
using System.Net;
using Task2.DataContext.Models;
using Task2.DataContext.Utilities;
using Task2.HttpModels;
using Task2.HttpModels.Data;

namespace Task2.DataContext;

public sealed class DataCreationService
{
    private readonly AppDbContext _db;
    private readonly DataValidator _validator;
    public DataCreationService(AppDbContext db, DataValidator validator)
    {
        _db = db;
        _validator = validator;
    }
    public async Task DeleteAllAsync()
    {
        _db.Districts.RemoveRange(_db.Districts);
        _db.Estates.RemoveRange(_db.Estates);
        _db.Materials.RemoveRange(_db.Materials);
        _db.Realtors.RemoveRange(_db.Realtors);
        _db.Sales.RemoveRange(_db.Sales);
        _db.Scores.RemoveRange(_db.Scores);
        _db.ScoreCriterias.RemoveRange(_db.ScoreCriterias);
        _db.Types.RemoveRange(_db.Types);
        await _db.SaveChangesAsync();
    }

    public async Task<List<string>> CreateEntitiesAsync(CreateRequest request)
    {
        string? message = _validator.Validate(request);
        if (!String.IsNullOrEmpty(message)) return new() { message };

        ConcurrentBag<string> errors = new();

        await createEntities(request, errors);

        bool affected = (await _db.SaveChangesAsync()) == 1;
        return errors.ToList();
    }

    private async Task createEntities(CreateRequest request, ConcurrentBag<string> errors)
    {
        // await createDistricts(request.Districts, errors);
        //await createEstateTypes(request.EstateTypes, errors);
        // await createMaterials(request.Materials, errors);
        // await createCriterias(request.Criterias, errors);
        // await createRealtors(request.Realtors, errors);
        // await createEstates(request.Estates, errors);
        await createScores(request.Estates.Select(x => x.Scores).Aggregate((prev, next) => prev.Concat(next).ToList()));

    }

    private async Task createDistricts(List<DistrictData> districts, ConcurrentBag<string> errors)
    {
        foreach (var district in districts)
        {
            await _db.Districts.AddAsync(new()
            {
                Name = district.Name
            });
        }
        await _db.SaveChangesAsync();
    }

    private async Task createEstateTypes(List<EstateTypeData> estateTypes, ConcurrentBag<string> errors)
    {
        foreach (var type in estateTypes)
        {
            await _db.Types.AddAsync(new()
            {
                Name = type.Name
            });
        }
        await _db.SaveChangesAsync();
    }

    private async Task createMaterials(List<MaterialData> materials, ConcurrentBag<string> errors)
    {
        foreach (var material in materials)
        {
            await _db.Materials.AddAsync(new()
            {
                Name = material.Name
            });
        }
        await _db.SaveChangesAsync();
    }

    private async Task createCriterias(List<ScoreCriteriaData> criterias, ConcurrentBag<string> errors)
    {
        foreach (var criteria in criterias)
        {
            await _db.ScoreCriterias.AddAsync(new()
            {
                Name = criteria.Name
            });
        }
        await _db.SaveChangesAsync();
    }

    private async Task createEstates(List<EstateData> estates, ConcurrentBag<string> errors)
    {
        bool raisedErrors = false;

        foreach (var estate in estates)
        {
            var address = estate.Address;
            var area = estate.Area;
            var datePosted = estate.DatePosted;
            var description = estate.Description ?? string.Empty;
            var district = await _db.Districts.FirstAsync(x => x.Name == estate.District);
            var floor = estate.Floor;
            var materials = await _db.Materials.Where(x => estate.Materials.Contains(x.Name)).ToListAsync();
            var price = estate.Price;
            var roomsNumber = estate.RoomsNumber;
            var type = await _db.Types.FirstAsync(x => x.Name == estate.Type);

            var estateCreated = await _db.Estates.AddAsync(new()
            { // Add sale, scores
                Address = address,
                Area = area,
                DatePosted = datePosted,
                Description = description,
                District = district,
                Floor = floor,
                Materials = materials,
                Price = price,
                RoomsNumber = roomsNumber,
                Type = type,
            });

            List<Score> scoresCreated = new();
            foreach (var score in estate.Scores)
            {
                var criteria = await _db.ScoreCriterias.FirstAsync(x => x.Name == score.Criteria);
                var scoreCreated = await _db.Scores.AddAsync(new Score
                {
                    ScoreDate = score.CreationDate,
                    EstateID = estateCreated.Entity.ID,
                    Criteria = criteria,
                    Value = score.Score
                });

                scoresCreated.Add(scoreCreated.Entity);
            }
            estateCreated.Entity.Scores = scoresCreated;
            if (estate.Sale is not null)
            {
                var realtor = await _db.Realtors.FirstAsync(x => estate.Sale.RealtorFCs == x.Surname + " " + x.Firstname + " " + x.Lastname);
                var saleCreated = await _db.Sales.AddAsync(new Sale
                {
                    Estate = estateCreated.Entity,
                    Price = estate.Sale.Price,
                    Realtor = realtor,
                    SaleDate = estate.Sale.Date
                });
                realtor.Sales.Add(saleCreated.Entity);
                estateCreated.Entity.Sale = saleCreated.Entity;
            }
        }
    }

    private async Task createScores(List<ScoreData> scores)
    {

    }
    private T createErrorAndGetEmptyObject<T>(string error, ConcurrentBag<string> errors, in bool flag) where T : ApplicationEntityBase, new()
    {
        return new T();
    }

    private async Task createRealtors(List<RealtorData> realtors, ConcurrentBag<string> errors)
    {
        bool raisedErrors = false;
        foreach (var realtor in realtors)
        {
            await _db.Realtors.AddAsync(new Realtor
            {
                Firstname = realtor.Firstname,
                Lastname = realtor.Lastname,
                Surname = realtor.Surname,
                PhoneNumber = realtor.PhoneNumber
            });
        }
        await _db.SaveChangesAsync();
    }


}