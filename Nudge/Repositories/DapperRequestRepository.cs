using Microsoft.Data.SqlClient;
using Dapper;
using Nudge.Lib.Models;
using Nudge.Lib.Dtos;

namespace Nudge.Repositories;

public class DapperRequestRepository : IRequestRepository
{
    private IConfiguration _configuration;

    public DapperRequestRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> CreateRequestAsync(CreateRequestDto createRequestDto)
    {
        Console.WriteLine("in here");
        const string query =
        """
            INSERT INTO requests (url, method, body) 
            VALUES (@Url, @Method, @Body)
        """;
        try
        {
            using var connection = new SqlConnection(_configuration["AzureSqlNudge"] ?? "");
            await connection.ExecuteAsync(query, createRequestDto);
            return true;
        }
        catch (SqlException e)
        {
            Console.Error.WriteLine(e);
            return false;
        }
    }

    public async Task UpdateRequestAsync(UpdateRequestDto updateRequestDto)
    {
        throw new NotImplementedException();
    }

    public async Task<int?> DeleteRequestAsync(int id)
    {
        using var connection = new SqlConnection(_configuration["AzureSqlNudge"] ?? "");
        const string query =
            """
                DELETE FROM requests
                WHERE request_id = @Id
            """;

        try
        {
            await connection.ExecuteAsync(query, new { Id = id });
            return id;
        }
        catch (SqlException e)
        {
            Console.Error.WriteLine(e);
            return null;
        }
    }

    public async Task<List<Request>?> GetAllRequestsAsync()
    {
        using var connection = new SqlConnection(_configuration["AzureSqlNudge"] ?? "");
        const string query =
            """
                SELECT
                    request_id AS RequestId,
                    url AS Url,
                    method AS Method,
                    body AS Body
                FROM requests
            """;
        try
        {
            var requests = await connection.QueryAsync<Request>(query);
            return requests.ToList();
        }
        catch (SqlException e)
        {
            Console.Error.WriteLine(e);
            return null;
        }
    }

    public async Task<Request?> GetRequestAsync(int id)
    {
        using var connection = new SqlConnection(_configuration["AzureSqlNudge"] ?? "");
        const string query =
            """
                SELECT
                    request_id AS RequestId,
                    url AS Url,
                    method AS Method,
                    body AS Body
                FROM requests
                WHERE request_id = @id
            """;

        try
        {
            var request = await connection.QuerySingleAsync<Request>(query);
            return request;
        }
        catch (SqlException e)
        {
            Console.Error.WriteLine(e);
            return null;
        }
    }
}