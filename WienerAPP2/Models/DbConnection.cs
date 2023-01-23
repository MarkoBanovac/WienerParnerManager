using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace WienerAPP2.Models
{
    public class DbConnection
    {
        private readonly string _connectionString;
        private MySqlConnection _connection;

        public DbConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Open()
        {
            _connection = new MySqlConnection(_connectionString);
            _connection.Open();
        }

        public void Close()
        {
            _connection.Close();
        }

        public IEnumerable<Partner> GetAllPartners()
        {
            var sql = "SELECT * FROM partnerstable";
            var partners = _connection.Query<Partner>(sql);
            return partners;
        }

        public void AddPartner(Partner partner)
        {
            var sql = "INSERT INTO `partnerstable` (`FirstName`, `LastName`, `Address`, `PartnerNumber`, `CroatianPIN`, `PartnerTypeId`, `CreatedAtUtc`, `CreateByUser`, `IsForeign`, `ExternalCode`, `Gender`) VALUES(@FirstName, @LastName, @Address, @PartnerNumber, @CroatianPIN, @PartnerTypeId, @CreatedAtUtc, @CreateByUser, @IsForeign, @ExternalCode, @Gender)";
            _connection.Execute(sql, new{ partner.FirstName, partner.LastName, partner.Address, partner.PartnerNumber, partner.CroatianPIN, partner.PartnerTypeId, partner.CreatedAtUtc, partner.CreateByUser, partner.IsForeign, partner.ExternalCode, partner.Gender });
        }

        public void AddPolicy(Policy policy)
        {
            var sql = "INSERT INTO `policytable` (`ExternalCodeFK`, `PolicyNumber`, `Value`) VALUES (@ExternalCode, @PolicyNumber, @Value)";
            _connection.Execute(sql, new { policy.ExternalCode, policy.PolicyNumber, policy.Value });
        }

        public IEnumerable<Partner> GetOnePartner(string ExternalCode)
        {
            var sql = "SELECT * FROM partnerstable WHERE ExternalCode = @ExternalCode";
            var parameters = new DynamicParameters();
            parameters.Add("@ExternalCode", ExternalCode);
            var partner = _connection.Query<Partner>(sql, parameters);
            return partner;
        }

        public IEnumerable<Policy> GetAllPartnerPolicies(string ExternalCode)
        {
            var sql = "SELECT * FROM policytable WHERE ExternalCodeFK = @ExternalCode";
            var parameters = new DynamicParameters();
            parameters.Add("@ExternalCode", ExternalCode);
            var policies = _connection.Query<Policy>(sql, parameters);
            return policies;
        }

        public T Query<T>(string sql, object parameters = null)
        {
            return _connection.QuerySingleOrDefault<T>(sql, parameters);
        }

        public int Execute(string sql, object parameters = null)
        {
            return _connection.Execute(sql, parameters);
        }
    }
}