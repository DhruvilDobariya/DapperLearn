using Dapper;
using DapperLearn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperLearn.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class ProductController : Controller
    {
        private readonly IConfiguration _config;
        public ProductController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAllProducts()
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            if (connection.State != ConnectionState.Open)
                connection.Open();

            IEnumerable<Product> products = connection.Query<Product>("Select * from Product");

            // QueryMany:
            // Let's assume we have one more table which is Customer
            // Now if we want to select multiple table then...
            //var result = connection.QueryMultiple("Select * from Product; Select * from Customer");

            //List<Product> products = result.Read<Product>().ToList();
            //List<Customer> customers = result.Read<Customer>().ToList();

            if (connection.State != ConnectionState.Closed)
                connection.Close();

            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Product> GetProductById(int id)
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            if (connection.State != ConnectionState.Open)
                connection.Open();

            // If table has no row then QueryFirst throw the exception "System.InvalidOperationException" 
            //Product product = connection.QueryFirst<Product>("Select * from Product Where Id = " + id);

            // If table has no row then QueryFirstOrDefault return null
            Product product = connection.QueryFirstOrDefault<Product>("Select * from Product Where Id = " + id);

            // QuerySingle and QuerySingleOrDefault
            // If table contains zero rows then QuerySingle throw the exception and QuerySingleOrDefault return null
            // Product product = connection.QuerySingleOrDefault<Product>("Select * from Product Where Id = " + id);

            if (connection.State != ConnectionState.Closed)
                connection.Close();

            return Ok(product);
        }

        [HttpPost]
        public ActionResult<string> AddProduct(Product product)
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            if (connection.State != ConnectionState.Open)
                connection.Open();

            int result = connection.Execute("Insert into Product (Name, Price, Quantity) values (@Name, @Price, @Quantity)", product);

            if (connection.State != ConnectionState.Closed)
                connection.Close();

            if(result != 0)
            {
                return Ok("Product added sucessfully.");
            }
            else
            {
                return BadRequest("Product dosen't inserted.");
            }
        }

        [HttpPost("AddProducts")]
        public ActionResult<string> AddProducts(List<Product> products)
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            if (connection.State != ConnectionState.Open)
                connection.Open();

            int result = connection.Execute("Insert into Product (Name, Price, Quantity) Values (@Name, @Price, @Quantity)", products);

            if (connection.State != ConnectionState.Closed)
                connection.Close();

            if (result != 0)
            {
                return Ok("Product added sucessfully.");
            }
            else
            {
                return BadRequest("Product dosen't inserted.");
            }
        }
        [HttpPut]
        public ActionResult<string> UpdateProduct(Product product)
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            if (connection.State != ConnectionState.Open)
                connection.Open();

            // Below two syntax not working
            // int result = connection.Execute($"Update Product set Name = {product.Name}, Price = {product.Price}, Quantity = {product.Quantity} where Id = {product.Id}");

            // int result = connection.Execute("Update Product set Name = " + product.Name + ", Price = " + product.Price + ", Quantity = " + product.Quantity + " where Id = " + product.Id);

            // Below two syntax working
            // int result = connection.Execute("Update Product set Name = @name, Price = @price, Quantity = @quantity where Id = @id", new { @id = product.Id, @name = product.Name, @price = product.Price, @quantity = product.Quantity});

            int result = connection.Execute("Update Product set Name = @Name, Price = @Price, Quantity = @Quantity where Id = @Id", product);

            // If we want to update multiple collection then...
            // Assume that updateProducts contains all updated product which we want to update in table.
            // IEnumerable<Product> updateProducts = null;
            // var result = connection.Execute("Update Product set Name = @Name, Price = @Price, Quantity = @Quantity where Id = @Id", updateProducts);

            if (connection.State != ConnectionState.Closed)
                connection.Close();

            if (result != 0)
            {
                return Ok(product);
            }
            else
            {
                return BadRequest("Product dosen't update.");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult<string> DeleteProduct(int id)
        {
            SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            if (connection.State != ConnectionState.Open)
                connection.Open();

            int result = connection.Execute($"Delete from Product Where Id = {id}");

            if (connection.State != ConnectionState.Closed)
                connection.Close();

            if (result != 0)
            {
                return Ok("Product deleted successfully.");
            }
            else
            {
                return BadRequest("Product dosen't deleted.");
            }
        }
    }
}
