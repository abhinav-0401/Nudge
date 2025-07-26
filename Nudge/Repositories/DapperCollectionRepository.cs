using Microsoft.Data.SqlClient;
using Dapper;
using Nudge.Lib.Dtos;
using Nudge.Lib.Models;

namespace Nudge.Repositories;

public class DapperCollectionRepository : ICollectionRepository
{
    private IConfiguration _configuration;    

    public DapperCollectionRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> CreateCollectionAsync(CreateCollectionDto createCollectionDto)
    {
        const string query =
        """
            INSERT INTO collections (name)
            VALUES (@Name)
        """;

        try
        {
            using var connection = new SqlConnection(_configuration["AzureSqlNudge"] ?? "");
            await connection.ExecuteAsync(query, createCollectionDto);
            return true;
        }
        catch (SqlException e)
        { 
            Console.Error.WriteLine(e);
            return false;
        }
    }

    public async Task<int?> DeleteCollectionAsync(int id)
    {
        const string deleteCollectionQuery =
        """
            DELETE FROM collections
            WHERE collection_id = @Id
        """;

        const string deleteRequestsQuery =
        """
            DELETE FROM requests
            WHERE collection_id = @Id
        """;

        try
        {
            using var connection = new SqlConnection(_configuration["AzureSqlNudge"] ?? "");
            await connection.ExecuteAsync(deleteCollectionQuery, new { Id = id });
            await connection.ExecuteAsync(deleteRequestsQuery, new { Id = id });
            return id;
        }
        catch (SqlException e)
        { 
            Console.Error.WriteLine(e);
            return null;
        }
    }    

    public async Task<List<Collection>?> GetAllCollectionsAsync()
    {
        const string query =
        """
            SELECT collection_id AS CollectionId, name AS Name FROM collections
        """;

        try
        {
            using var connection = new SqlConnection(_configuration["AzureSqlNudge"] ?? "");
            var collections = await connection.QueryAsync<Collection>(query);
            return collections.ToList();
        }
        catch (SqlException e)
        {
            Console.Error.WriteLine(e);
            return null;
        }
    }

    public async Task<Collection?> GetCollectionAsync(int id)
    {
        const string query =
        """
            SELECT
                c.collection_id AS CollectionId, c.name AS Name,
                r.request_id AS RequestId, r.url AS Url, r.method AS Method, r.body AS Body, r.collection_id AS CollectionId
            FROM collections c
            LEFT JOIN requests r
            ON c.collection_id = r.collection_id
            WHERE c.collection_id = @Id
        """;

        try
        {
            var collectionMap = new Dictionary<int, Collection>();

            using var connection = new SqlConnection(_configuration["AzureSqlNudge"] ?? "");
            var collection = await connection.QueryAsync<Collection, Request, Collection>(
                query,
                (collection, request) =>
                {
                    if (!collectionMap.TryGetValue(collection.CollectionId, out var existing))
                    {
                        existing = collection;
                        existing.Requests = new List<Request>();
                        collectionMap[collection.CollectionId] = existing;
                    }

                    if (request != null && request.CollectionId != 0)
                    {
                        existing.Requests?.Add(request);
                    }

                    return existing;
                },
                param: new { Id = id },
                splitOn: "RequestId"
            );
            return collection.FirstOrDefault();
        }
        catch (SqlException e)
        {
            Console.Error.WriteLine(e);
            return null;
        }
    }

    public Task UpdateCollectionAsync(UpdateRequestDto updateRequestDto)
    {
        throw new NotImplementedException();
    }
}