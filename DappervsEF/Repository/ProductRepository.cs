using Dapper;
using DappervsEF.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DappervsEF.Repository
{
    public class ProductRepository
    {
        public readonly DBContext dbContext = new DBContext();

        #region Dapper
        public Product DapperInsert(Product product)
        {
            using (var connection = new SqlConnection(dbContext.Database.Connection.ConnectionString))
            {
                try
                {
                    connection.Open();

                    var query = "INSERT INTO Product (Name, Price) VALUES (@Name, @Price); SELECT CAST(SCOPE_IDENTITY() AS int)";

                    int productId = connection.Query<int>(query, product).Single();
                    product.Id = productId;
                    return product;
                }
                catch (System.Exception)
                {
                    return new Product();
                }
            }
        }
        public string DapperInsertV2(List<Product> products)
        {
            using (var connection = new SqlConnection(dbContext.Database.Connection.ConnectionString))
            {
                try
                {
                    connection.Open();

                    var query = "INSERT INTO Product (Name, Price) VALUES (@Name, @Price); SELECT CAST(SCOPE_IDENTITY() AS int)";
                    connection.Execute(query, products);

                    return "Success " + products.Count();
                }
                catch (System.Exception e)
                {
                    return e.Message;
                }
            }
        }
        public Product DapperUpdate(Product product)
        {
            using (var connection = new SqlConnection(dbContext.Database.Connection.ConnectionString))
            {
                try
                {
                    connection.Open();

                    var query = "UPDATE Product SET Name = @Name, Price = @Price WHERE Id = @Id";
                    connection.Execute(query, product);
                    return product;
                }
                catch (System.Exception)
                {
                    return new Product();
                }
            }
        }
        public List<Product> DapperGetAll()
        {
            using (var connection = new SqlConnection(dbContext.Database.Connection.ConnectionString))
            {
                connection.Open();

                var query = "SELECT * FROM Product";

                return connection.Query<Product>(query).ToList();
            }
        }
        #endregion

        #region Entity Framework
        public Product EFInsert(Product product)
        {
            dbContext.Products.Add(product);
            dbContext.SaveChanges();
            return product;
        }
        public string EFInsertV2(List<Product> products)
        {
            try
            {
                dbContext.Products.AddRange(products);
                dbContext.SaveChanges();
                return "Success " + products.Count();
            }
            catch (System.Exception e)
            {
                return e.Message;
            }
        }

        public List<Product> EFGetAll()
        {
            var query = dbContext.Products;
            return query.ToList();
        }
        public Product EFUpdate(Product product)
        {
            var currValue = dbContext.Products.FirstOrDefault(w => w.Id == product.Id);
            dbContext.Entry(currValue).CurrentValues.SetValues(product);
            dbContext.SaveChanges();
            return product;
        }
        #endregion
    }
}