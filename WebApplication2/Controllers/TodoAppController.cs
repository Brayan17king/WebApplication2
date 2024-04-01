using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoAppController : ControllerBase
    {
        //obtener los detalles de la conexión
        private IConfiguration _configuration;

        //Uso de la inyeccion de dependencia para acceder a los detalles de la conexión
        public TodoAppController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetNotes")]

        //Metodo para obtener los datos
        public JsonResult GetNotes()
        {
            //Consulta de seleccion para obtener los datos 
            string query = "select * from dbo.Notes ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("todoAppDBCon");
            SqlDataReader myReader;
            using(SqlConnection mycon =new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using(SqlCommand myCommand = new SqlCommand(query, mycon)) 
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
                
            }
            return new JsonResult(table);
        }

        ///////////////////////////////////////////////
        [HttpPost]
        [Route("AddNotes")]

        //Metodo para Agregar nuevos datos
        public JsonResult AddNotes([FromForm] string newNotes)
        {
            //Inserta dentro de nuestra base de datos los nuevos valores
            string query = "insert into dbo.Notes values (@newNotes) ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("todoAppDBCon");
            SqlDataReader myReader;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@newNotes", newNotes);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }

            }
            return new JsonResult("Added Successfully");
        }

        ///////////////////////////////////////////////////
        [HttpDelete]
        [Route("DeleteNotes")]

        //Metodo para Eliminar los datos
        public JsonResult AddNotes(int id)
        {
            //Selecciona el id de la nota a eliminar  
            string query = "delete from dbo.Notes where id = @id ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("todoAppDBCon");
            SqlDataReader myReader;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }

            }
            return new JsonResult("Delete Successfully");
        }

    }

  }

