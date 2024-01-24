using System.Data;
using MySql.Data.MySqlClient;

namespace clases;

public class MySQLCon
{

	private static MySQLCon instance = null;
	private MySql.Data.MySqlClient.MySqlConnection conn;
	private MySqlCommand cs;
	string myConnectionString = "";


	private string server = "186.3.139.231";
	private string port = "3306";
	private string database = "safelandtest";
	private string user = "testUser";
	private string password = "safeland2023";

	private MySQLCon()
	{
		myConnectionString = string.Format($"server={server};uid={user};" +
		$"pwd={password};database={database};port={port};");
		startConection();
		setCall();

	}


	/// <summary>
	/// 
	/// </summary>
	/// <exception cref="MySql.Data.MySqlClient.MySqlException">
	/// Lanza en caso de falla de fallo de conexion
	/// </exception>
	public static MySQLCon getInstance()
	{
		if (instance == null)
		{
			instance = new MySQLCon();
		}
		return instance;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <exception cref="MySql.Data.MySqlClient.MySqlException">
	/// Lanza en caso de falla de fallo de conexion
	/// </exception>
	public void startConection()
	{
		conn = new MySql.Data.MySqlClient.MySqlConnection();
		conn.ConnectionString = myConnectionString;
		conn.Open();

	}


	public void setCall()
	{
		cs = new MySqlCommand("sp_getNombreByCodigo", conn);
		cs.CommandType = System.Data.CommandType.StoredProcedure;
	}


	public bool closeConection()
	{
		try
		{
			conn.Close();
			return true;
		}
		catch (Exception ex)
		{
			return false;
		}
	}

	public string getNombreByCodigo(string code)
	{

		String nombre = "";
		try
		{
			cs.Parameters.AddWithValue("@id", code);
			var results = cs.ExecuteNonQuery();
			using (MySqlDataAdapter sda = new MySqlDataAdapter(cs))
			{
				DataTable dt = new DataTable();
				sda.Fill(dt);
				nombre = dt.Rows[0][0].ToString() ?? "Gris";
			}

		}
		catch (Exception e)
		{
			nombre = "Gris";
		}
		cs.Parameters.Clear();

		return nombre;

	}
}